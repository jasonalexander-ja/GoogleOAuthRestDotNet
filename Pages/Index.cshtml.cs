using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GoogleOAuthExample;
using GoogleOAuthExample.Services;
using System.Net;

namespace GoogleOAuthExample.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> Logger;
        private HttpClient HttpClient;
        private IGoogleTokenStore GoogleTokenStore;

        public string Data { get; set; } = "";

        public IndexModel(ILogger<IndexModel> logger, IGoogleTokenStore tokenStore)
        {
            Logger = logger;
            HttpClient = new HttpClient();
            GoogleTokenStore = tokenStore;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            string tokenId;
            if (!Request.Cookies.TryGetValue("GoogleApiTokenId", out tokenId))
                return Redirect("oauth2callback");
            var token = await GoogleTokenStore.GetTokenAsync(tokenId);

            if (token == null || token.Token.ExpiresIn <= 0)
                return Redirect("oauth2callback");

            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.Token.AccessToken}");

            HttpResponseMessage result = await HttpClient.GetAsync("https://www.googleapis.com/drive/v2/files");

            if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Redirect("oauth2callback");
            }
            Data = await result.Content.ReadAsStringAsync();

            return Page();
        }

        public void OnPost() 
        {

        }
    }
}
