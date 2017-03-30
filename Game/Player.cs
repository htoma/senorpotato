namespace Game
{
    public class Player
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Country { get; set; }
        public int Overall { get; set; }
        public Skills Skills { get; set; }
        public Captain Captain { get; set; }
        public bool IsCaptain { get; set; }
    }
}