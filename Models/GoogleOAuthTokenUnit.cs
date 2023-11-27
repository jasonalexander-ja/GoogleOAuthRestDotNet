namespace GoogleOAuthExample.Models;

public class GoogleOAuthTokenUnit
{
    public string Id { get; set; } = "";
    public DateTime Issued { get; set; }
    public GoogleOAuthToken Token { get; set; } = new GoogleOAuthToken();
}
