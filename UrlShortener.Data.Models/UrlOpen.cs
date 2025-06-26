using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrlShortener.Data.Models
{
    public class UrlOpen
    {
        [Key]
        public int Id { get; set; }

        public string IpAddress { get; set; } = string.Empty; 
        
        public DateTime OpendAt { get; set; } = DateTime.UtcNow;
        
        [ForeignKey(nameof(Url))]
        public int UrlId { get; set; }

        public Url Url { get; set; } = null!;
    }
}
