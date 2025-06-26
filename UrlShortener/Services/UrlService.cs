using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Data.Models;
using UrlShortener.Models;
using Microsoft.Extensions.Logging;

namespace UrlShortener.Services
{
    public class UrlService : IUrlService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<UrlService> _logger;

        public UrlService(AppDbContext context, IHttpContextAccessor httpContextAccessor, ILogger<UrlService> logger)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        public async Task<UrlViewModel> ShortenUrl(UrlViewModel viewModel)
        {
            try
            {
                var originalUrl = viewModel.Url;
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

                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null)
                    throw new InvalidOperationException("No HTTP context available.");

                var baseUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}";

                return new UrlViewModel
                {
                    ShortenedUrl = $"{baseUrl}/{shortCode}",
                    StatsUrl = $"{baseUrl}/stats/{secretCode}",
                    Url = originalUrl
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ShortenUrl");
                throw;
            }
        }

        private static string GenerateShortCode()
        {
            return Guid.NewGuid().ToString("N")[..6];
        }

        private static string GenerateSecretCode()
        {
            return Guid.NewGuid().ToString("N");
        }

        public async Task<IActionResult> RedirectToOr(string shortCode)
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null)
                    return new StatusCodeResult(500);

                var url = await _context.Urls.FirstOrDefaultAsync(e => e.ShortenedUrl == shortCode);

                if (url == null)
                {
                    return new NotFoundObjectResult("Short Url not Found!");
                }

                var forwardedFor = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                var ip = !string.IsNullOrWhiteSpace(forwardedFor)
                    ? forwardedFor.Split(',')[0].Trim()
                    : httpContext.Connection.RemoteIpAddress?.ToString();
                var open = new UrlOpen
                {
                    OpendAt = DateTime.UtcNow,
                    urlId = url.Id,
                };
                if (ip != null)
                {
                    open.IpAddress = ip;
                }
                _context.UrlOpens.Add(open);
                await _context.SaveChangesAsync();

                return new RedirectResult(url.OriginalUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in RedirectToOr");
                return new StatusCodeResult(500);
            }
        }

        public async Task<StatsViewModel?> GetSecret(string secretCode)
        {
            try
            {
                var url = await _context.Urls.FirstOrDefaultAsync(u => u.SecretCode == secretCode);
                if (url == null)
                    return null;

                var opens = _context.UrlOpens.Where(e => e.urlId == url.Id);

                var uniqueVisitsPerDay = await opens
                    .GroupBy(o => o.OpendAt.Date)
                    .Select(g => new { Date = g.Key, Count = g.Select(x => x.IpAddress).Distinct().Count() })
                    .OrderBy(x => x.Date)
                    .ToListAsync();

                var topIps = await opens
                    .GroupBy(o => o.IpAddress)
                    .Select(g => new { Ip = g.Key, Count = g.Count() })
                    .OrderByDescending(x => x.Count)
                    .Take(10)
                    .Cast<object>()
                    .ToListAsync();

                return new StatsViewModel
                {
                    Url = url,
                    UniqueVisitsPerDay = uniqueVisitsPerDay.Cast<object>().ToList(),
                    TopIps = topIps
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetSecret");
                return null;
            }
        }
    }
}
