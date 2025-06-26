namespace UrlShortener.Web.Utilities
{
    public static class UrlUtility
    {
        public static string GenerateShortCode()
        {
            return Guid.NewGuid().ToString("N")[..6];
        }

        public static string GenerateStatsCode()
        {
            return Guid.NewGuid().ToString("N");
        }

        public static string GetShortLink(HttpContext httpContext, string shortCode)
        {
            return $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/{shortCode}";
        }
        public static string GetStatsLink(HttpContext httpContext, string statsCode)
        {
            return $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/stats/{statsCode}";
        }
    }
}
