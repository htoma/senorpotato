using System.Collections.Generic;

namespace Game.Cards
{
    public class ActionCard
    {
        public ActionCard()
        {
            
        }

        public ActionCard(EActionCardType type)
        {
            Attackers = new List<Player>();
            Defenders = new List<Player>();
            PlayerLimit = (int) type;
        }

        public enum EActionCardType
        {
            Penalty = 1,
            FreeKick = 2,
            Attack = 3,
            CounterAttack = 4,
            Max
        }

        public int PlayerLimit { get; set; }

        public List<Player> Attackers { get; set; }
        public List<Player> Defenders { get; set; }

        public bool AddAttacker(Player player)
        {
            if (Attackers.Count < PlayerLimit)
            {
                Attackers.Add(player);
                return true;
            }
            return false;
        }

        public bool AddDefender(Player player)
        {
            if (Defenders.Count < PlayerLimit)
            {
                Defenders.Add(player);
                return true;
            }
            return false;
        }
    }
}