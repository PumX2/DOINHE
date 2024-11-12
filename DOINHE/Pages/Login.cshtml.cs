using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DOINHE.Entitys;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace DOINHE.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public LoginModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [BindProperty]
        public User User { get; set; }

        public IActionResult OnGet()
        {
            User = new User(); // Đảm bảo model được khởi tạo

            if (HttpContext.Session.GetString("admin") != null || HttpContext.Session.GetString("customer") != null)
            {
                return RedirectToPage("");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Cấu hình client API
            var client = _httpClientFactory.CreateClient();

            var loginUser = new
            {
                Email = User.Email,
                Password = User.Password
            };

            var content = new StringContent(JsonSerializer.Serialize(loginUser), Encoding.UTF8, "application/json");

            // Gửi yêu cầu đăng nhập đến API
            var response = await client.PostAsync("https://localhost:7023/api/user/login", content);
            if (response.IsSuccessStatusCode)
            {
                var responseData = JsonSerializer.Deserialize<JsonElement>(await response.Content.ReadAsStringAsync());
                var role = responseData.GetProperty("role").GetString();
                var user = responseData.GetProperty("user");
                var userId = responseData.GetProperty("userId");
                var name = responseData.GetProperty("name");

                // Lưu thông tin người dùng vào Session
                HttpContext.Session.SetString("Account", user.ToString());

                if (role == "admin")
                {
                    HttpContext.Session.SetString("admin", user.ToString());
                    return RedirectToPage("/Admin/Dashboard");
                }
                if (role == "user")
                {
                    HttpContext.Session.SetString("customer", user.ToString());
                    HttpContext.Session.SetString("UserId", userId.ToString());
                    HttpContext.Session.SetString("name", name.ToString());
                    return RedirectToPage("/Index");
                }
            }

            // Nếu đăng nhập không thành công, thông báo lỗi ngay lập tức
            TempData["msg"] = "Invalid email or password.";
            return Page();
        }

    }
}
