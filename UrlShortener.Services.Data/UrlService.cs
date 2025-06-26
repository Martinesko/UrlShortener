using Microsoft.EntityFrameworkCore;
using UrlShortener.Data.Models;
using UrlShortener.Web.ViewModels;
using UrlShortener.Services.Data.Contracts;
using UrlShortener.Data;

namespace UrlShortener.Services.Data
{
    public class UrlService : IUrlService
    {
        private readonly UrlShortenerDbContext context;

        public UrlService(UrlShortenerDbContext context)
        {
            this.context = context;
        }

        public async Task CreateAsync(UrlViewModel viewModel)
        {
            var url = new Url
            {
                OriginalUrl = viewModel.Url,
                ShortenedUrl = viewModel.ShortenedUrl,
                StatsUrl = viewModel.StatsUrl
            };

            context.Urls.Add(url);
            await context.SaveChangesAsync();
        }

        public async Task<int> GetIdAsync(string shortCode)
        {
            return (await context.Urls.FirstAsync(u => u.ShortenedUrl == shortCode)).Id;
        }

        public async Task<string> GetUrlAsync(int id)
        {
            return (await context.Urls.FindAsync(id))!.OriginalUrl;
        }
    }
}
