using System;
using System.Linq;
using Azure;
using Game.Generators;
using Game.Utils;

namespace Game
{
    public class GameManager
    {
        private const string GameBlockPrefix = "game_";

        private const int TeamPlayerCount = 5;

        public Game InitializeGame()
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

        private static void Save(Game game)
        {
            BlobManager.Upload(GetBlobName(game.Id), game);
        }

        private static string GetBlobName(int id)
        {
            return $"{GameBlockPrefix}{id}";
        }        
    }
}
