using GoogleOAuthExample.Models;
using GoogleOAuthExample.Services;
using Microsoft.Extensions.Primitives;
using System.Web;

namespace GoogleOAuthExample.Extensions;

public static class WebApplicationExtensions
{
    public static IEndpointConventionBuilder MapGoogleAuth(this WebApplication app)
    {
        string endPoint = "";
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider.GetRequiredService<GoogleAuthConfig>();
            endPoint = services.RedirectUri;

        }

        return app.MapGet(endPoint, async (HttpContext context, GoogleAuthConfig config, IGoogleTokenStore tokenStore) =>
        {
            var redirectUrl = $"{config.RedirectScheme ?? context.Request.Scheme}://{config.RedirectHost ?? context.Request.Host.Value}{config.RedirectUri}";
            StringValues codes;
            if (!context.Request.Query.TryGetValue("code", out codes))
            {
                string authUri = $"https://accounts.google.com/o/oauth2/v2/auth?response_type=code"
                    + $"&client_id={config.ClientId}&redirect_uri={HttpUtility.UrlEncode(redirectUrl)}&scope={config.Scope}";
                context.Response.Redirect(authUri);
                return "";
            }

            string code = codes.First()!;

            HttpClient client = new HttpClient();

            HttpResponseMessage result = await client.PostAsJsonAsync(
                config.TokenEndpoint, new
                {
                    code,
                    client_id = config.ClientId,
                    client_secret = config.ClientSecret,
                    redirect_uri = redirectUrl,
                    grant_type = "authorization_code",
                });

            if (!result.IsSuccessStatusCode)
            {
                context.Response.Redirect("Error");
                return "";
            }

            var res = await result.Content.ReadFromJsonAsync<GoogleOAuthToken>();

            if (res is null)
            {
                context.Response.Redirect("Error");
                return "";
            }

            var tokenUnit = await tokenStore.SetTokenAsync(res);
            context.Response.Cookies.Append("GoogleApiTokenId", tokenUnit.Id);

            context.Response.Redirect("/");
            return "";
        });
    }
}
