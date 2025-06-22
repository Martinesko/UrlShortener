using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using UrlShortener.Models;
using UrlShortener.Services;

namespace UrlShortener.Controllers;

public class UrlController : Controller
{
    private readonly IUrlService _urlService;

    public UrlController(IUrlService urlService)
    {
        _urlService = urlService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new UrlViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Index(UrlViewModel model)
    {
        var modell = await _urlService.ShortenUrl(model);
        return View(modell);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [Route("{shortCode}")]
    public async Task<IActionResult> RedirectTOriginal(string shortCode)
    {
        return await _urlService.RedirectToOr(shortCode);
    }

    [Route("stats/{secretCode}")]
    public async Task<IActionResult> Stats(string secretCode)
    {
        var model = await _urlService.GetSecret(secretCode);
        return View(model);
    }
}


