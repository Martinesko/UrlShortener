namespace UrlShortener.Data.Models
{
    public class UrlOpen
    {
        public int Id { get; set; }
        public int urlId { get; set; }
        public string IpAddress { get; set; } = string.Empty;   
        public DateTime OpendAt { get; set; }
    }
}
