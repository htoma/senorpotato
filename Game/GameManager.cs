using System;
using System.Linq;
using Azure;
using Game.Generators;
using Game.Utils;

namespace Game
{
    public class GameManager
    {
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

            return new Game(shuffled.Take(TeamPlayerCount), shuffled.Skip(shuffled.Count-TeamPlayerCount));
        }
    }
}
