using UrlShortener.Web.ViewModels;

namespace UrlShortener.Services.Data.Contracts
{
    public interface IUrlOpenService
    {
        Task CreateAsync(int urlId, string ipAddress);
        Task<StatsViewModel?> GetSecret(string statusCode);
    }
}
