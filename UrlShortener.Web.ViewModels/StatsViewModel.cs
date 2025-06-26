using UrlShortener.Data.Models;

namespace UrlShortener.Web.ViewModels
{
    public class StatsViewModel
    {
        public required Url Url { get; set; }
        public List<UniqueVisitsPerDayViewModel> UniqueVisitsPerDay { get; set; } = new List<UniqueVisitsPerDayViewModel>();
        public List<TopIpViewModel> TopIps { get; set; } = new List<TopIpViewModel>();
    }
}
