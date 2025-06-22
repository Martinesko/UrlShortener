using UrlShortener.Data.Models;

namespace UrlShortener.Models
{
    public class StatsViewModel
    {
        public Url Url { get; set; }
        public List<object> UniqueVisitsPerDay { get; set; }
        public List<object> TopIps { get; set; }
    }
}
