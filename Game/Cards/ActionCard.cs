using Azure;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Game.Cards
{
    public class ActionCard
    {
        public ActionCard()
        {
            
        }

        public ActionCard(EActionCardType type, int duration)
        {
            Id = BlobManager.GetNextId();
            PlayerLimit = (int) type;
            Duration = duration;
        }

        public enum EActionCardType
        {
            Attack = 1,
            Freekick = 2,
            Penalty = 3
        }

        public int Id { get; set; }

        public int Duration { get; set; }

        public bool Played { get; set; }

        public int PlayerLimit { get; set; }

        public GamePlayer First { get; set; }

        public GamePlayer Second { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ETurn Owner { get; set; }
        
        public Score Score { get; set; }
    }
}