using UrlShortener.Data.Models;

namespace UrlShortener.Models
{
    public class StatsViewModel
    {
        public required Url Url { get; set; } 
        public List<object> UniqueVisitsPerDay { get; set; } = new List<object>();
        public List<object> TopIps { get; set; } = new List<object>();
    }
}
