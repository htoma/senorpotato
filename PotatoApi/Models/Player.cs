namespace PotatoApi.Models
{
    public class Player
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Country { get; set; }
        public int Overall { get; set; }
        public Skills Skills { get; set; }
    }
}