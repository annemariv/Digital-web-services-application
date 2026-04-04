namespace ClientsideWebApp.Models
{
    public class DigitalServiceModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string ShortDescription { get; set; }
        public List<string> Bullets { get; set; }
    }
}