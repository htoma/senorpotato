namespace PotatoApi.Models
{
    public class Captain
    {
        public enum EAffected
        {
            Self,
            OpponentPlayer,
            OpponentTeam
        }

        public EAffected Affected { get; set; }
        public ESkill Skill { get; set; }
    }
}