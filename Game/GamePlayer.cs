using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Game
{
    public class GamePlayer
    {
        public GamePlayer()
        {
            
        }

        public GamePlayer(ETurn turn, IEnumerable<Player> players)
        {
            Turn = turn;
            Players = players.ToList();            
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public ETurn Turn { get; set; }

        public List<Player> Players { get; set; }

        public bool Confirmed { get; set; }
    }
}