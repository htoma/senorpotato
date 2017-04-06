using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Game
{
    public class Game
    {
        public Game()
        {
        }

        public Game(int id, IEnumerable<Player> firstPlayers, IEnumerable<Player> secondPlayers)
        {
            Id = id;
            GameStatus = EGameStatus.NotStarted;
            Turn = ETurn.First;
            Score = new Score();

            First = new GamePlayer(firstPlayers);
            Second = new GamePlayer(secondPlayers);
        }

        public int Id { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public EGameStatus GameStatus { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ETurn Turn { get; set; }

        public Score Score { get; set; }

        public GamePlayer First { get; set; }

        public GamePlayer Second { get; set; }

        public GamePlayer GetPlayer(ETurn turn)
        {
            if (turn == ETurn.First)
            {
                return First;
            }
            return Second;
        }
    }
}
