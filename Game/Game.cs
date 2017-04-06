using System.Collections.Generic;
using Azure;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Game
{
    public class Game
    {
        public Game()
        {
        }

        public Game(IEnumerable<Player> firstPlayers, IEnumerable<Player> secondPlayers)
        {
            Id = BlobManager.GetNextId();
            GameStatus = EGameStatus.NotStarted;
            Turn = ETurn.First;
            Score = new Score();

            First = new GamePlayer(firstPlayers);
            Second = new GamePlayer(secondPlayers);
        }

        public int Id { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public EGameStatus GameStatus { get; set; }

        public int Time { get; set; }

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
