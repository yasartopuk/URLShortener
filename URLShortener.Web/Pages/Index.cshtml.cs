using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.Json.Nodes;

namespace URLShortener.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public string OriginalUrl { get; set; }

        public async Task OnPostAsync()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("URLShortener");
                var response = await httpClient.PostAsJsonAsync("shorten", new { OriginalUrl });

                if (response.IsSuccessStatusCode)
                {
                    var value = await response.Content.ReadFromJsonAsync<JsonObject>();
                    ViewData["ShortUrl"] = value["shortUrl"];
                    ViewData["ShortCode"] = value["shortCode"];
                }
                else
                {
                    ViewData["ErrorMessage"] = await response.Content.ReadAsStringAsync();
                }
            }
            catch (HttpRequestException ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
        }
    }
}