using System.Collections.Generic;

namespace Game.Cards
{
    public class ActionCard
    {
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
            CounterAttack = 4
        }

        public int PlayerLimit { get; }

        public List<Player> Attackers { get; }
        public List<Player> Defenders { get; }

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