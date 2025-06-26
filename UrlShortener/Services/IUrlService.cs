using Microsoft.AspNetCore.Mvc;
using UrlShortener.Models;

namespace UrlShortener.Services
{
    public interface IUrlService
    {
        public Task<UrlViewModel> ShortenUrl(UrlViewModel viewModel);
        public Task<IActionResult> RedirectToOr(string shortCode);
        public Task<StatsViewModel?> GetSecret(string secretCode);
       }
}
