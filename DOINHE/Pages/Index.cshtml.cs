using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text.Json;
using DOINHE_BusinessObject;

namespace DOINHE.Pages
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [BindProperty]
        public List<DOINHE_BusinessObject.Product> products { get; set; } = new List<DOINHE_BusinessObject.Product>();

        public async Task<IActionResult> OnGetAsync(string searchTerm)
        {
            // Gọi API để lấy danh sách sản phẩm
            var response = await _httpClient.GetAsync("http://localhost:7023/api/Product");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                products = JsonSerializer.Deserialize<List<DOINHE_BusinessObject.Product>>(json) ?? new List<DOINHE_BusinessObject.Product>();

                // Nếu có searchTerm thì lọc sản phẩm trước khi hiển thị
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    products = products
                        .Where(p => p.ProductName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }
            }
            else
            {
                // Xử lý lỗi nếu không lấy được dữ liệu từ API
                ModelState.AddModelError(string.Empty, "Không thể tải danh sách sản phẩm.");
            }

            return Page();
        }
    }
}
