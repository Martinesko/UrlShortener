using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Data.Models
{
    public class Url
    {
        [Key]
        public int Id { get; set; }
        public string OriginalUrl { get; set; } = string.Empty;
        public string ShortenedUrl { get; set; } = string.Empty;
        public string StatsUrl { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<UrlOpen> UrlOpens = new HashSet<UrlOpen>();
    }
}
