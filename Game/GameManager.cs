using System;
using System.Linq;
using Azure;
using Game.Cards;
using Game.Generators;
using Game.Utils;

namespace Game
{
    public static class GameManager
    {
        private const string GameBlockPrefix = "game_";

        private const int TeamPlayerCount = 5;

        private const int ActionCardsCount = 4;

        private const int HalfTimeMark = 45;
        private const int EndOfGameMark = 90;

        public static Game CreateGame()
        {
            var players = TableManager.Get<PlayerEntity>(TableData.PlayersTable).ToList();
            if (players.Count < TeamPlayerCount)
            {
                throw new ArgumentException($"Number of generated players should be greater than {TeamPlayerCount}");
            }

            var captains = TableManager.Get<CaptainEntity>(TableData.CaptainsTable).ToList();

            var allPlayers = PlayerGenerator.Generate(players, captains);
            var shuffled = RandomSelectors.Shuffle(Seeder.Random(), allPlayers).ToList();

            var game = new Game(shuffled.Take(TeamPlayerCount), shuffled.Skip(shuffled.Count - TeamPlayerCount));
            Save(game);

            return game;
        }

        public static Game Load(int id)
        {
            return BlobManager.Get<Game>(GetBlobName(id));
        }

        public static void DeleteGame(int id)
        {
            BlobManager.Delete(GetBlobName(id));
        }

        public static void Delete()
        {
            BlobManager.DeleteWithPrefix(GameBlockPrefix);
        }

        public static bool SetCaptain(int gameId, ETurn turn, int playerId)
        {
            var game = Load(gameId);
            if (game == null || !CanChooseCaptain(game, turn))
            {
                return false;
            }
            var players = game.GetPlayer(turn).Players;
            var player = players.FirstOrDefault(x => x.Id == playerId);
            if (player == null)
            {
                return false;
            }
            players.ForEach(x => x.IsCaptain = false);
            player.IsCaptain = true;

            Save(game);

            return true;
        }

        public static bool Confirm(int gameId, ETurn turn)
        {
            var game = Load(gameId);
            if (game == null)
            {
                return false;
            }

            if (game.GetPlayer(1 - turn).Confirmed)
            {
                EvaluateRound(game);
            }
            else
            {
                game.GetPlayer(turn).Confirmed = true;
            }

            Save(game);

            return true;
        }

        public static bool AddPlayer(int gameId, ETurn turn, int actionCardId, int playerId)
        {
            var game = Load(gameId);
            if (game == null)
            {
                return false;
            }

            if (!CanPlayActionCard(game))
            {
                return false;
            }
            
            var card = GetCurrentActionCard(game);
            if (card == null || card.Id != actionCardId)
            {
                //wrong card
                return false;
            }

            //get attackers or defenders, depending on the turn. If game turn is identical to turn, then attackers, otherwise defenders
            var cardPlayers = turn == game.Turn ? card.Attackers : card.Defenders;
            if (cardPlayers.Count == card.PlayerLimit)
            {
                //player limit reach
                return false;
            }

            if (cardPlayers.Any(x => x.Id == playerId))
            {
                //player already selected on the card
                return false;
            }

            //check against remaining players in team if there's one with the id and left stamina
            var firstOrSecond = turn == ETurn.First ? game.First : game.Second;
            var playerSelected = firstOrSecond.Players.FirstOrDefault(x => x.Id == playerId && x.Skills.Stamina > 0);
            if (playerSelected == null)
            {
                //inexistent or too tired
                return false;
            }

            cardPlayers.Add(playerSelected);

            Save(game);

            return true;
        }

        public static bool RemovePlayer(int gameId, ETurn turn, int actionCardId, int playerId)
        {
            var game = Load(gameId);
            if (game == null)
            {
                return false;
            }

            if (!CanPlayActionCard(game))
            {
                return false;
            }

            var card = GetCurrentActionCard(game);
            if (card == null || card.Id != actionCardId)
            {
                //wrong card
                return false;
            }

            //get attackers or defenders, depending on the turn. If game turn is identical to turn, then attackers, otherwise defenders
            var cardPlayers = turn == game.Turn ? card.Attackers : card.Defenders;
            var player = cardPlayers.FirstOrDefault(x => x.Id == playerId);
            if (player == null)
            {
                //player not found on the card
                return false;
            }

            cardPlayers.Remove(player);

            Save(game);

            return true;
        }

        public static bool PlayActionCard(int gameId, ETurn turn, int actionCardId)
        {
            var game = Load(gameId);
            if (game == null)
            {
                return false;
            }

            if (!CanPlayActionCard(game))
            {
                return false;
            }

            if (game.Turn != turn)
            {
                //not your turn
                return false;
            }

            var gamePlayer = game.GetPlayer(game.Turn);
            var card = gamePlayer.ActionCards.FirstOrDefault(x => x.Id == actionCardId);
            if (card == null)
            {
                //you don't have this card
                return false;
            }

            gamePlayer.CurrentCard = card.Id;

            Save(game);

            return true;
        }

        private static bool CanPlayActionCard(Game game)
        {
            return new[] {EGameStatus.FirstHalf, EGameStatus.SecondHalf}.Contains(game.GameStatus);
        }

        private static void EvaluateRound(Game game)
        {
            //set uncofirmed for each player, the turn or phase changes
            new[] {ETurn.First, ETurn.Second}.ToList().ForEach(turn => game.GetPlayer(turn).Confirmed = false);

            switch (game.GameStatus)
            {
                    case EGameStatus.NotStarted:
                        game.GameStatus = EGameStatus.FirstHalf;
                        game.Turn = ETurn.First;
                        AllocateActionCards(game);
                        break;
                    case EGameStatus.FirstHalf:
                        EvaluateActionCard(game);
                        if (game.Time >= HalfTimeMark)
                        {
                            game.GameStatus = EGameStatus.HalfTime;
                        }
                        break;
                    case EGameStatus.HalfTime:
                        game.Time = HalfTimeMark;
                        game.GameStatus = EGameStatus.SecondHalf;
                        game.Turn = ETurn.Second;
                        break;
                    case EGameStatus.SecondHalf:
                        EvaluateActionCard(game);
                        if (game.Time >= EndOfGameMark)
                        {
                            game.GameStatus = EGameStatus.Ended;
                            EndGame();
                        }
                        break;
                    default:
                        throw new NotImplementedException();
            }
        }

        private static ActionCard GetCurrentActionCard(Game game)
        {
            var gamePlayer = game.GetPlayer(game.Turn);
            var card = gamePlayer.ActionCards.FirstOrDefault(x => !x.Played && x.Id == gamePlayer.CurrentCard);
            return card;
        }

        private static void EvaluateActionCard(Game game)
        {
            var card = GetCurrentActionCard(game);
            card.Played = true;
            game.Time += card.Duration;
            game.Turn = 1 - game.Turn;

            ComputeCardScore(card);
            UpdatePlayerStamina(card);

            if (card.Score.First > 0 || card.Score.Second > 0)
            {
                game.Score.GoalScored(card.Score.First > 0 ? game.Turn : (1 - game.Turn));
            }
        }

        private static void UpdatePlayerStamina(ActionCard card)
        {
            foreach (var player in card.Attackers.Concat(card.Defenders))
            {
                if (player.Skills.Stamina > 0)
                {
                    player.Skills.Stamina--;
                }
                else if (player.Skills.Stamina == 0 &&
                         player.IsCaptain &&
                         player.Captain.Skill == ESkill.Stamina &&
                         player.Captain.Value > 0)
                {
                    player.Captain.Value--;
                    //if player has no stamina left, but his captain skill is stamina and it applies to himself
                    //and there's something left
                }
            }
        }

        private static void ComputeCardScore(ActionCard card)
        {
            var attack = card.Attackers.Sum(x => x.Skills.Attack);
            var defence = card.Defenders.Sum(x => x.Skills.Defence);

            var captainAttack = card.Attackers.FirstOrDefault(x => x.IsCaptain);
            if (captainAttack != null)
            {
                if (captainAttack.Captain.Skill == ESkill.Attack)
                {
                    attack += captainAttack.Captain.Value;
                }
                else if (captainAttack.Captain.Skill == ESkill.Penalty &&
                         card.PlayerLimit == (int) ActionCard.EActionCardType.Penalty)
                {
                    attack += captainAttack.Captain.Value;
                }
                else if (captainAttack.Captain.Skill == ESkill.Defence &&
                        captainAttack.Captain.Affected == Captain.EAffected.OpponentTeam)
                {
                    defence += captainAttack.Captain.Value;
                }
            }

            var captainDefence = card.Defenders.FirstOrDefault(x => x.IsCaptain);
            if (captainDefence != null)
            {
                if (captainDefence.Captain.Skill == ESkill.Defence)
                {
                    defence += captainDefence.Captain.Value;
                }
                else if (captainDefence.Captain.Skill == ESkill.Attack &&
                         captainDefence.Captain.Affected == Captain.EAffected.OpponentTeam)
                {
                    attack += captainDefence.Captain.Value;
                }
            }

            if (attack != defence)
            {
                bool firstScored = attack > defence;
                card.Score = new Score(firstScored ? 1 : 0, firstScored ? 0 : 1);
            }
        }

        private static void EndGame()
        {
            throw new NotImplementedException();
        }

        private static void AllocateActionCards(Game game)
        {
            game.First.ActionCards = CardGenerator.GenerateActionCards(ActionCardsCount).ToList();
            game.Second.ActionCards = CardGenerator.GenerateActionCards(ActionCardsCount).ToList();
        }

        private static void Save(Game game)
        {
            BlobManager.Upload(GetBlobName(game.Id), game);
        }

        private static string GetBlobName(int id)
        {
            return $"{GameBlockPrefix}{id}";
        }

        private static bool CanChooseCaptain(Game game, ETurn turn)
        {
            var player = game.GetPlayer(turn);
            if (player.Confirmed)
            {
                //already confirmed for the current phase
                return false;
            }

            return new[] {EGameStatus.NotStarted, EGameStatus.HalfTime}.Contains(game.GameStatus);

            //note(htoma): changing captain should be allowed during substitution?
        }        
    }
}
