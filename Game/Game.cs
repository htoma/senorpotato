using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Game
{
    public class Game
    {
        public Game()
        {
        }

        public Game(int id, IEnumerable<Player> attackers, IEnumerable<Player> defenders)
        {
            Id = id;
            GameStatus = EGameStatus.NotStarted;
            Turn = ETurn.First;
            Score = new Score();

            FirstPlayers = attackers.ToList();
            SecondPlayers = defenders.ToList();
        }

        public int Id { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public EGameStatus GameStatus { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ETurn Turn { get; set; }

        public Score Score { get; set; }

        public List<Player> FirstPlayers { get; set; }
        public List<Player> SecondPlayers { get; set; }
    }
}
