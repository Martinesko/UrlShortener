using Microsoft.EntityFrameworkCore;
using UrlShortener.Data.Models;

namespace UrlShortener.Data
{
    public class UrlShortenerDbContext : DbContext
    {
        public UrlShortenerDbContext(DbContextOptions<UrlShortenerDbContext> options) : base(options) { }
        public DbSet<Url> Urls { get; set; }
        public DbSet<UrlOpen> UrlOpens { get; set; }
    }
}
