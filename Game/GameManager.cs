﻿using System;
using System.Linq;
using Azure;
using Game.Generators;
using Game.Utils;

namespace Game
{
    public static class GameManager
    {
        private const string GameBlockPrefix = "game_";

        private const int TeamPlayerCount = 5;

        public static Game CreateGame()
        {
            var players = TableManager.Get<PlayerEntity>(TableData.PlayersTable).ToList();
            if (players.Count < TeamPlayerCount)
            {
                throw new ArgumentException($"Number of generated players should be greater than {TeamPlayerCount}");
            }

            var captains = TableManager.Get<CaptainEntity>(TableData.CaptainsTable).ToList();

            var allPlayers = PlayerGenerator.Generate(Seeder.Random(), players, captains);
            var shuffled = RandomSelectors.Shuffle(Seeder.Random(), allPlayers).ToList();

            var gameId = BlobManager.GetNextId();

            var game = new Game(gameId, shuffled.Take(TeamPlayerCount), shuffled.Skip(shuffled.Count - TeamPlayerCount));
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
                        //todo(htoma): associate action cards to each player
                        break;
                    case EGameStatus.FirstHalf:
                        //todo(htoma): update score, player stamina
                        //todo(htoma): based on time or other condition, switch status to HalfTime
                        break;
                    case EGameStatus.HalfTime:
                        game.GameStatus = EGameStatus.SecondHalf;
                        break;
                    case EGameStatus.SecondHalf:
                        //todo(htoma): update score, player stamina
                        //todo(htoma): based on time or other condition, switch status to Ended and EndGame()
                        break;
                    default:
                        throw new NotImplementedException();
            }
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

            return !new[] {EGameStatus.NotStarted, EGameStatus.HalfTime}.Contains(game.GameStatus);

            //note(htoma): changing captain should be allowed during substitution?
        }
    }
}
