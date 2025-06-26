using UrlShortener.Web.ViewModels;

namespace UrlShortener.Services.Data.Contracts
{
    public interface IUrlService
    {
         Task CreateAsync(UrlViewModel viewModel);
         Task<int> GetIdAsync(string shortCode);
        Task<string> GetUrlAsync(int id);
    }
}
