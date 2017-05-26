using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Game
{
    public class SpecialSkill
    {
        public enum EAffected
        {
            Self,
            OwnTeam,
            OpponentTeam
        }

        public int Id { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public EAffected Affected { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]

        public ESkill Skill { get; set; }
        public int Value { get; set; }        
    }
}