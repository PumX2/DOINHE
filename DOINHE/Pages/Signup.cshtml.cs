using DOINHE.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DOINHE.Pages
{
    public class SignupModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        [BindProperty]
        public UserDTO UserDTO { get; set; }

        public SignupModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var client = _httpClientFactory.CreateClient();
            var requestContent = new StringContent(JsonSerializer.Serialize(UserDTO), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:7023/api/user/signup", requestContent);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Signup successful!";
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                TempData["ErrorMessage"] = errorMessage;
            }
            return Page();
        }
    }
}
