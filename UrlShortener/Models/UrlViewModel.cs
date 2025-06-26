namespace UrlShortener.Models
{
    public class UrlViewModel
    {
        public string Url { get; set; } = string.Empty;
        public string ShortenedUrl { get; set; } = string.Empty;
        public string StatsUrl { get; set; } = string.Empty;
    }
}
