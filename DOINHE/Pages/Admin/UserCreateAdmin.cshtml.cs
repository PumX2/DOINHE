using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DOINHE_BusinessObject;

namespace DOINHE.Pages.Admin
{
    public class UserAddAdminModel : PageModel
    {
        [BindProperty]
        public User User { get; set; } = new User();

        private readonly IHttpClientFactory _httpClientFactory;

        public UserAddAdminModel(IHttpClientFactory httpClientFactory)
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
                client.BaseAddress = new System.Uri("https://localhost:7023/api/");

                var response = await client.PostAsJsonAsync("User", User);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("UserAdmin");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to create the user.");
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
