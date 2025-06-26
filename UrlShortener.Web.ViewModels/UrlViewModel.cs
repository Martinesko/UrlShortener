using System.ComponentModel.DataAnnotations;
using static UrlShortener.Common.ValidationConstants.Url;

namespace UrlShortener.Web.ViewModels
{
    public class UrlViewModel
    {
        [Required(ErrorMessage = RequiredMessage)]
        [Url(ErrorMessage = InvalidMessage)]
        public string Url { get; set; } = string.Empty;
        
        public string ShortenedUrl { get; set; } = string.Empty;

        public string StatsUrl { get; set; } = string.Empty;
    }
}
