using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Data.Models;
using UrlShortener.Services.Data.Contracts;
using UrlShortener.Web.ViewModels;

namespace UrlShortener.Services.Data
{
    public class UrlOpenService : IUrlOpenService
    {
        private readonly UrlShortenerDbContext context;

        public UrlOpenService(UrlShortenerDbContext context)
        {
            this.context = context;
        }

        public async Task CreateAsync(int urlId, string ipAddress)
        {
            var urlOpen = new UrlOpen
            {
                UrlId = urlId,
                IpAddress = ipAddress
            };

            context.UrlOpens.Add(urlOpen);
            await context.SaveChangesAsync();
        }
        public async Task<StatsViewModel> GetStatsAsync(string statsCode)
        {
            var opens = context.UrlOpens.Include(uo => uo.Url).Where(e => e.Url.StatsUrl == statsCode);

            var uniqueVisitsPerDay = await opens
                .GroupBy(o => o.OpendAt.Date)
                .Select(g => new UniqueVisitsPerDayViewModel { Date = g.Key, Count = g.Select(x => x.IpAddress).Distinct().Count() })
                .OrderBy(x => x.Date)
                .ToListAsync();

            var topIps = await opens
                .GroupBy(o => o.IpAddress)
                .Select(g => new TopIpViewModel { Ip = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(10)
                .ToListAsync();

            var url = context.Urls.First(u => u.StatsUrl == statsCode);

            return new StatsViewModel
            {
                OriginalUrl = url.OriginalUrl,
                ShortUrl = url.ShortenedUrl,
                UniqueVisitsPerDay = uniqueVisitsPerDay.ToList(),
                TopIps = topIps
            };
        }
    }
}
