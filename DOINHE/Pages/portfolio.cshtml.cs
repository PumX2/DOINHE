using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DOINHE.Entitys;
//using DOINHE_BusinessObject;
using System.Linq;
using DOINHE.Db;
using Newtonsoft.Json;
using System.Net.Http;

namespace DOINHE.Pages
{
    public class portfolioModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public portfolioModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        [BindProperty]
        public List<Entitys.Product> products { get; set; } = new List<Entitys.Product>();

        [BindProperty]
        public List<Category> categories { get; set; } = new List<Category>();

        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? categoryId { get; set; }
        public async Task<IActionResult> OnGetAsync(int? pageNumber, string? searchTerm, DateTime? startDate, DateTime? endDate, double? minPrice, double? maxPrice)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7023/api/Product");
            List<Entitys.Product>? productss= new();
            if (response.IsSuccessStatusCode)
            {
                productss = JsonConvert.DeserializeObject<List<Entitys.Product>>(await response.Content.ReadAsStringAsync());

            }


            int pageSize = 12;
            CurrentPage = pageNumber ?? 1;

            // Lấy sản phẩm đã được duyệt và chưa mua
            var totalProducts = productss.Where(p => p.StatusIsApprove == true && p.StatusIsBuy == false);

            // Lọc theo từ khóa nếu có
            if (!string.IsNullOrEmpty(searchTerm))
            {
                totalProducts = totalProducts.Where(p => p.ProductName.Contains(searchTerm) || p.Description.Contains(searchTerm));
            }

            // Lọc theo khoảng giá nếu có
            if (minPrice.HasValue)
            {
                totalProducts = totalProducts.Where(p => (double?)p.Price >= minPrice);
            }
            if (maxPrice.HasValue)
            {
                totalProducts = totalProducts.Where(p => (double?)p.Price <= maxPrice);
            }
            // Lọc theo category nếu có
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                totalProducts = totalProducts.Where(p => p.CategoryId == categoryId.Value);
            }
            // Tính số trang
            TotalPages = (int)Math.Ceiling(totalProducts.Count() / (double)pageSize);

            // Lấy sản phẩm cho trang hiện tại
            products = totalProducts
                .Skip((CurrentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            var response1 = await client.GetAsync("https://localhost:7023/api/Category");
            var categories = JsonConvert.DeserializeObject<List<Entitys.Category>>(await response1.Content.ReadAsStringAsync());


            return Page();
        }


    }
}
