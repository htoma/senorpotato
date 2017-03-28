using System;
using Azure;
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

        public static Captain FromEntity(CaptainEntity entity)
        {
            return new Captain
            {
                Affected = (EAffected) Enum.Parse(typeof(EAffected), entity.Affected),
                Skill = (ESkill) Enum.Parse(typeof(ESkill), entity.Skill),
                Value = entity.Value
            };
        }
    }
}