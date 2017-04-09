using System.Collections.Generic;
using Azure;

namespace Game.Cards
{
    public class ActionCard
    {
        public ActionCard()
        {
            
        }

        public ActionCard(EActionCardType type, int duration)
        {
            Attackers = new List<Player>();
            Defenders = new List<Player>();
            Id = BlobManager.GetNextId();
            PlayerLimit = (int) type;
            Duration = duration;
        }

        public enum EActionCardType
        {
            Penalty = 1,
            Freekick = 2,           
            Attack = 4
        }

        public int Id { get; set; }

        public int Duration { get; set; }

        public bool Played { get; set; }

        public int PlayerLimit { get; set; }

        public List<Player> Attackers { get; set; }

        public List<Player> Defenders { get; set; }

        public Score Score { get; set; }
    }
}