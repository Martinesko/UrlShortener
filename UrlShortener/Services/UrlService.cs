using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Data.Models;
using UrlShortener.Models;

namespace UrlShortener.Services
{
    public class UrlService : IUrlService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UrlService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public async Task<UrlViewModel> ShortenUrl(UrlViewModel viewModel)
        {
            string originalUrl = viewModel.Url;
            if (string.IsNullOrWhiteSpace(originalUrl) || !Uri.IsWellFormedUriString(originalUrl, UriKind.Absolute))
                throw new ArgumentException("Invalid URL");


            var shortCode = GenerateShortCode();
            var secretCode = GenerateSecretCode();

            var url = new Url
            {
                OriginalUrl = originalUrl,
                ShortenedUrl = shortCode,
                SecretCode = secretCode,
                CreatedAt = DateTime.UtcNow
            };



            _context.Urls.Add(url);
            await _context.SaveChangesAsync();

            var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";

            var response = new
            {
                ShortenedUrl = $"{baseUrl}/{shortCode}",
                StatsUrl = $"{baseUrl}/stats/{secretCode}"
            };
            UrlViewModel model = new UrlViewModel
            {
                ShortenedUrl = $"{baseUrl}/{shortCode}",
                StatsUrl = $"{baseUrl}/stats/{secretCode}",
                Url = originalUrl
            };
            return model;

        }

        private string GenerateShortCode()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 6);
        }

        private string GenerateSecretCode()
        {
            return Guid.NewGuid().ToString("N");
        }

        public async Task<IActionResult> RedirectToOr(string shortCode)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var url = await _context.Urls.FirstOrDefaultAsync(e => e.ShortenedUrl == shortCode);
           
            var forwardedFor = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            string ip;


            if (url == null)
            {
                return new NotFoundObjectResult("Short Url not Found!");
            }

            if (forwardedFor != null) 
                ip = forwardedFor.Split(',')[0].Trim();
            else 
                ip = httpContext.Connection.RemoteIpAddress?.ToString();

            UrlOpen open = new UrlOpen
            {
                OpendAt = DateTime.UtcNow,
                urlId = url.Id,
                IpAddress = ip
            };

            _context.UrlOpens.Add(open);
            await _context.SaveChangesAsync();
         
            return new RedirectResult(url.OriginalUrl);
        }

        public async Task<StatsViewModel> GetSecret(string secretCode)
        {

            var url = await _context.Urls.FirstOrDefaultAsync(u => u.SecretCode == secretCode);
            var opens = _context.UrlOpens.Where(e => e.urlId == url.Id);

            if (url == null)
                return null;

            var uniqueVisitsPerDay = opens
                .GroupBy(o => o.OpendAt.Date)
                .Select(g => new { Date = g.Key, Count = g.Select(x => x.IpAddress).Distinct().Count() })
                .OrderBy(x => x.Date)
                .ToList();

            var topIps = opens
            .GroupBy(o => o.IpAddress)
            .Select(g => new { Ip = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(10)
            .Cast<object>()
            .ToList();

            return new StatsViewModel
            {
                Url = url,
                UniqueVisitsPerDay = uniqueVisitsPerDay.Cast<object>().ToList(),
                TopIps = topIps
            };

        }
    }
}
