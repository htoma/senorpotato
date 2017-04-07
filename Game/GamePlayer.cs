using System.Collections.Generic;
using System.Linq;
using Game.Cards;

namespace Game
{
    public class GamePlayer
    {
        public GamePlayer()
        {
            
        }

        public GamePlayer(IEnumerable<Player> players)
        {
            Players = players.ToList();            
        }

        public bool Confirmed { get; set; }

        public List<Player> Players { get; set; }

        public List<ActionCard> ActionCards { get; set; }

        public int CurrentCard { get; set; }
    }
}