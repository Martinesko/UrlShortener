using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using UrlShortener.Models;
using UrlShortener.Services;

namespace UrlShortener.Controllers;

public class UrlController : Controller
{
    private readonly IUrlService _urlService;
    private readonly ILogger<UrlController> _logger;


    public UrlController(IUrlService urlService , ILogger<UrlController> logger)
    {
        _urlService = urlService;
        _logger = logger;

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
            var modell = await _urlService.ShortenUrl(model);
            return View(modell);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred.");
            TempData["ErrorMessage"] = "An unexpected error occurred. Please try again.";
            return RedirectToPage("/Error");
        }

    }

    [Route("{shortCode}")]
    public async Task<IActionResult> RedirectTOriginal(string shortCode)
    {
        try
        {
            return await _urlService.RedirectToOr(shortCode);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred.");
            TempData["ErrorMessage"] = "An unexpected error occurred. Please try again.";
            return RedirectToPage("/Error");
        }
    }

    [Route("stats/{secretCode}")]
    public async Task<IActionResult> Stats(string secretCode)
    {
        try
        {
            var model = await _urlService.GetSecret(secretCode);
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred.");
            TempData["ErrorMessage"] = "An unexpected error occurred. Please try again.";
            return RedirectToPage("/Error");
        }
    }
}


