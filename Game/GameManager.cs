using System;
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
            if (game == null || game.GameStatus != EGameStatus.NotStarted)
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

        private static void Save(Game game)
        {
            BlobManager.Upload(GetBlobName(game.Id), game);
        }

        private static string GetBlobName(int id)
        {
            return $"{GameBlockPrefix}{id}";
        }

        public static bool Confirm(int gameId, ETurn turn)
        {
            var game = Load(gameId);
            if (game == null || game.GameStatus != EGameStatus.NotStarted)
            {
                return false;
            }

            game.GetPlayer(turn).Confirmed = true;
            if (game.GetPlayer(1 - turn).Confirmed)
            {
                game.GameStatus = EGameStatus.Running;
            }

            Save(game);

            return true;
        }
    }
}
