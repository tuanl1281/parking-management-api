using Microsoft.AspNetCore.Http;

namespace Parking.Management.Service.Utilities;

public static class UrlUtilities
{
    public static string GetBaseUrl(HttpContext context, bool isHttps = false)
    {
        var request = context.Request;
        var host = request.Host.ToUriComponent();
        var pathBase = request.PathBase.ToUriComponent();

        return isHttps ? $"https://{host}{pathBase}" : $"http://{host}{pathBase}";
    }
}