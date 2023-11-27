namespace GoogleOAuthExample.Models;

public class GoogleAuthConfig
{
    public string ClientId { get; set; } = "";
    public string ClientSecret { get; set; } = "";
    public string Scope { get; set; } = "";
    public string RedirectUri { get; set; } = "";
    public string? RedirectHost { get; set; }
    public string? RedirectScheme { get; set; }
    public string TokenEndpoint { get; set; } = "https://oauth2.googleapis.com/token";
}
