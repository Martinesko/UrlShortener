using System.ComponentModel.DataAnnotations.Schema;

namespace UrlShortener.Data.Models
{
    public class UrlOpen
    {
        public int Id { get; set; }

        [ForeignKey("Url")]
        public int UrlId { get; set;}
        public Url Url { get; set; }
        public string IpAddress { get; set; } = string.Empty;   
        public DateTime OpendAt { get; set; } = DateTime.UtcNow;
    }
}
