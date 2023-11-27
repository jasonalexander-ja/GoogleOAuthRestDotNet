using GoogleOAuthExample.Models;

namespace GoogleOAuthExample.Services;

public class InMemoryGoogleTokenStore: IGoogleTokenStore
{
    public Dictionary<string, GoogleOAuthTokenUnit> Store { get; set; } = new Dictionary<string, GoogleOAuthTokenUnit>();
    public TimeSpan Lifetime { get; set; } = new TimeSpan(0, 30, 0);
    public async Task<GoogleOAuthTokenUnit?> GetTokenAsync(string tokenId)
    {
        Store = Store.Where(k => k.Value.Issued + Lifetime > DateTime.Now)
            .ToDictionary(k => k.Key, k => k.Value);
        return Store.GetValueOrDefault(tokenId, null);
    }
    public GoogleOAuthTokenUnit? GetToken(string tokenId)
    {
        Store = Store.Where(k => k.Value.Issued + Lifetime > DateTime.Now)
            .ToDictionary(k => k.Key, k => k.Value);
        return Store.GetValueOrDefault(tokenId, null);
    }
    public async Task<GoogleOAuthTokenUnit> SetTokenAsync(GoogleOAuthToken token)
    {
        Store = Store.Where(k => k.Value.Issued + Lifetime > DateTime.Now)
            .ToDictionary(k => k.Key, k => k.Value);
        var tokenUnit = new GoogleOAuthTokenUnit
        {
            Id = Guid.NewGuid().ToString(),
            Token = token,
            Issued = DateTime.Now
        };
        Store.Add(tokenUnit.Id, tokenUnit);
        return tokenUnit;
    }
    public GoogleOAuthTokenUnit SetToken(GoogleOAuthToken token)
    {
        Store = Store.Where(k => k.Value.Issued + Lifetime > DateTime.Now)
            .ToDictionary(k => k.Key, k => k.Value);
        var tokenUnit = new GoogleOAuthTokenUnit
        {
            Id = Guid.NewGuid().ToString(),
            Token = token,
            Issued = DateTime.Now
        };
        Store.Add(tokenUnit.Id, tokenUnit);
        return tokenUnit;
    }
}
