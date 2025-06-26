using UrlShortener.Data.Models;

namespace UrlShortener.Web.ViewModels
{
    public class StatsViewModel
    {
        public string OriginalUrl { get; set; } = string.Empty;
        public string ShortUrl { get; set; } = string.Empty;

        public List<UniqueVisitsPerDayViewModel> UniqueVisitsPerDay { get; set; } = new List<UniqueVisitsPerDayViewModel>();
        public List<TopIpViewModel> TopIps { get; set; } = new List<TopIpViewModel>();
    }
}
