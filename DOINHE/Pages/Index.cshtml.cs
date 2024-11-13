using DOINHE.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace DOINHE.Pages
{
    public class IndexModel : PageModel
    {
        private readonly MyDbContext _db;
        private readonly IConfiguration _configuration;

        public IndexModel(MyDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        [BindProperty]
        public List<DOINHE_BusinessObject.Product> products { get; set; } = new List<DOINHE_BusinessObject.Product>();
        public List<DOINHE_BusinessObject.Product> productss { get; set; } = new List<DOINHE_BusinessObject.Product>();
        public async Task<IActionResult> OnGetAsync(string searchTerm)
        {
            string UrlAPI = _configuration["UrlAPI"];
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri("https://localhost:7023/api/");

                    productss = await client.GetFromJsonAsync<List<DOINHE_BusinessObject.Product>>("Product");
                    products = productss.Where(p => p.StatusIsApprove == true).ToList();
                }
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, $"Lỗi khi kết nối API: {ex.Message}");
            }


            // Nếu có searchTerm thì lọc sản phẩm trước khi hiển thị
            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p => p.ProductName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return Page();
        }

    }
}