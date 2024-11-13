using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DOINHE.Entitys;
using System.Diagnostics.Metrics;
using System.Text.Json;
using DOINHE.Db;
using Azure;
using Newtonsoft.Json;

namespace DOINHE.Pages
{
    public class portfolio_detailsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public portfolio_detailsModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public DOINHE_BusinessObject.Product products { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri("https://localhost:7023/api/");

                    var product = await client.GetFromJsonAsync<DOINHE_BusinessObject.Product>($"Product/{id}");
                    if (product == null)
                    {
                        return NotFound();
                    }

                    products = product;
                }
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, $"Error connecting to API: {ex.Message}");
            }
            return Page();
        }
    }
}
