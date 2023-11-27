using Microsoft.AspNetCore.Builder;
using GoogleOAuthExample.Models;
using GoogleOAuthExample.Services;

namespace GoogleOAuthExample.Extensions;

public static class IServiceCollectionExtensions
{
    public static void AddGoogleAuth(this IServiceCollection services, IConfigurationSection config)
    {
        GoogleAuthConfig auth = config.Get<GoogleAuthConfig>() ?? new GoogleAuthConfig();

        if (!auth.RedirectUri.StartsWith("/")) 
        {
            throw new ArgumentException($"RedirectUri must start with a '/'. ");
        }

        services.AddSingleton<GoogleAuthConfig>(auth);
        services.AddSingleton<IGoogleTokenStore>(new InMemoryGoogleTokenStore());
    }
}
