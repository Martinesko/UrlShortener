using Microsoft.AspNetCore.Mvc;
using UrlShortener.Web.ViewModels;
using UrlShortener.Services.Data.Contracts;
using UrlShortener.Web.Utilities;

namespace UrlShortener.Web.Controllers;

public class UrlController : Controller
{
    private readonly IUrlService urlService;
    private readonly IUrlOpenService urlOpenService;

    public UrlController(IUrlService urlService, IUrlOpenService urlOpenService)
    {
        this.urlService = urlService;
        this.urlOpenService = urlOpenService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new UrlViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Index(UrlViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.ShortenedUrl = UrlUtility.GenerateShortCode();
            model.StatsUrl = UrlUtility.GenerateStatsCode();

            await urlService.CreateAsync(model);

            model.ShortenedUrl = UrlUtility.GetShortLink(HttpContext, model.ShortenedUrl);
            model.StatsUrl = UrlUtility.GetStatsLink(HttpContext, model.StatsUrl);

            return View(model);
        }
        catch (Exception)
        {
            return RedirectToAction("Index");
        }
    }

    [Route("{shortCode}")]
    public async Task<IActionResult> RedirectTOriginal(string shortCode)
    {
        try
        {
            var forwardedFor = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();

            var ip = !string.IsNullOrWhiteSpace(forwardedFor)
                ? forwardedFor.Split(',')[0].Trim()
                : HttpContext.Connection.RemoteIpAddress!.ToString();

            var urlId = await urlService.GetIdAsync(shortCode);

            await urlOpenService.CreateAsync(urlId, ip);
            var url = await urlService.GetUrlAsync(urlId);

            return Redirect(url);
        }
        catch (Exception)
        {
            return RedirectToAction("Index");
        }
    }

    [Route("stats/{secretCode}")]
    public async Task<IActionResult> Stats(string secretCode)
    {
        try
        {
            var model = await urlOpenService.GetStatsAsync(secretCode);
            return View(model);
        }
        catch (Exception)
        {
            return RedirectToAction("Index");
        }
    }
}