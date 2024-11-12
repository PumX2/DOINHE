using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DOINHE_BusinessObject;

namespace DOINHE.Pages.Admin
{
    public class UserEditAdminModel : PageModel
    {
        [BindProperty]
        public User User { get; set; } = new User();

        private readonly IHttpClientFactory _httpClientFactory;

        public UserEditAdminModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (HttpContext.Session.GetString("admin") == null)
            {
                return RedirectToPage("/Index");
            }
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new System.Uri("https://localhost:7023/api/");

                var user = await client.GetFromJsonAsync<User>($"User/{id}");
                if (user == null)
                {
                    return NotFound();
                }

                User = user;
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, $"Error connecting to API: {ex.Message}");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string action)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (action == "delete")
            {
                // Gọi API để xóa người dùng
                try
                {
                    var client = _httpClientFactory.CreateClient();
                    client.BaseAddress = new System.Uri("https://localhost:7023/api/");
                    var response = await client.DeleteAsync($"User/{User.Id}");

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToPage("UserAdmin");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Failed to delete the user.");
                    }
                }
                catch (HttpRequestException ex)
                {
                    ModelState.AddModelError(string.Empty, $"Error connecting to API: {ex.Message}");
                }
            }
            else
            {
                // Cập nhật người dùng
                try
                {
                    var client = _httpClientFactory.CreateClient();
                    client.BaseAddress = new System.Uri("https://localhost:7023/api/");
                    var response = await client.PutAsJsonAsync($"User/{User.Id}", User);

                    if (!response.IsSuccessStatusCode)
                    {
                        ModelState.AddModelError(string.Empty, "Failed to update the user.");
                        return Page();
                    }
                }
                catch (HttpRequestException ex)
                {
                    ModelState.AddModelError(string.Empty, $"Error connecting to API: {ex.Message}");
                    return Page();
                }
            }

            return RedirectToPage("UserAdmin");
        }
    }
}
