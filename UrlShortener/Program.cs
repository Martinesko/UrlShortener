using UrlShortener.Data;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Services;
namespace UrlShortener
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // Pass connection string

            builder.Services.AddScoped<IUrlService, UrlService>();


            builder.Services.AddHttpContextAccessor();
            var app = builder.Build();


            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

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
