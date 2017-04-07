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

            if (!new[] { EGameStatus.FirstHalf, EGameStatus.SecondHalf }.Contains(game.GameStatus))
            {
                //playing?
                return false;
            }

            var gamePlayer = game.GetPlayer(game.Turn);
            var card = GetCurrentActionCard(gamePlayer);
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

            if (!new[] { EGameStatus.FirstHalf, EGameStatus.SecondHalf }.Contains(game.GameStatus))
            {
                //playing?
                return false;
            }

            var gamePlayer = game.GetPlayer(game.Turn);
            var card = GetCurrentActionCard(gamePlayer);
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

        private static void EvaluateRound(Game game)
        {
            //set uncofirmed for each player, the turn or phase changes
            new[] {ETurn.First, ETurn.Second}.ToList().ForEach(turn => game.GetPlayer(turn).Confirmed = false);

            switch (game.GameStatus)
            {
                    case EGameStatus.NotStarted:
                        game.GameStatus = EGameStatus.FirstHalf;
                        AllocateActionCards(game);
                        break;
                    case EGameStatus.FirstHalf:
                        EvaluateActionCard();
                        if (game.Time >= HalfTimeMark)
                        {
                            game.GameStatus = EGameStatus.HalfTime;
                        }
                        break;
                    case EGameStatus.HalfTime:
                        game.Time = HalfTimeMark;
                        game.GameStatus = EGameStatus.SecondHalf;
                        break;
                    case EGameStatus.SecondHalf:
                        EvaluateActionCard();
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

        private static ActionCard GetCurrentActionCard(GamePlayer player)
        {            
            var card = player.ActionCards.FirstOrDefault(x => !x.Played && x.Id == player.CurrentCard);
            return card;
        }

        private static void EvaluateActionCard()
        {
            //todo(htoma): mark action card as played
            //todo(htoma): switch game turn after evaluating a card

            throw new NotImplementedException();
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
