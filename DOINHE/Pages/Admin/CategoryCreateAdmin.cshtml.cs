using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DOINHE_BusinessObject;

namespace DOINHE.Pages.Admin
{
    public class CategoryAddAdminModel : PageModel
    {
        [BindProperty]
        public Category Category { get; set; } = new Category();

        private readonly IHttpClientFactory _httpClientFactory;

        public CategoryAddAdminModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("admin") == null)
            {
                return RedirectToPage("/Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new System.Uri("http://doinheexe.runasp.net/api/");

                var response = await client.PostAsJsonAsync("Category", Category);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("CategoryAdmin");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to create the category.");
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
