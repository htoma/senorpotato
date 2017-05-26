namespace Game
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Country { get; set; }
        public int Overall { get; set; }
        public Skills Skills { get; set; }
        public SpecialSkill SpecialSkill { get; set; }
        public bool IsCaptain { get; set; }
    }
}