using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PotatoApi.Models
{
    public class Captain
    {
        public enum EAffected
        {
            Self,
            OwnTeam,
            OpponentTeam
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public EAffected Affected { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]

        public ESkill Skill { get; set; }
        public int Value { get; set; }
    }
}