using GoogleOAuthExample.Models;

namespace GoogleOAuthExample.Services;

public interface IGoogleTokenStore
{
    public Task<GoogleOAuthTokenUnit?> GetTokenAsync(string tokenId);
    public GoogleOAuthTokenUnit? GetToken(string tokenId);
    public Task<GoogleOAuthTokenUnit> SetTokenAsync(GoogleOAuthToken token);
    public GoogleOAuthTokenUnit SetToken(GoogleOAuthToken token);
}
