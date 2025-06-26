using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public async Task<StatsViewModel?> GetSecret(string statusCode)
        {
            var opens = context.UrlOpens.Where(e => e.Url.StatsUrl == statusCode);

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

            return new StatsViewModel
            {
                Url = opens.Select(o => o.Url).First(),
                UniqueVisitsPerDay = uniqueVisitsPerDay.ToList(),
                TopIps = topIps
            };
        }
    }
}
