using UrlShortener.Data;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Services.Data;
using UrlShortener.Services.Data.Contracts;
namespace UrlShortener.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<UrlShortenerDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IUrlService, UrlService>();
            builder.Services.AddScoped<IUrlOpenService, UrlOpenService>();

            builder.Services.AddHttpContextAccessor();

            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Url}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.MapControllerRoute(
                name: "short",
               pattern: "{shortCode}",
                defaults: new { controller = "Url", action = "RedirectToOriginal" });

            app.Run();
        }
    }
}
