using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DOINHE.Entitys;
using System.Linq;
using DOINHE.Db;

namespace DOINHE.Pages
{
    public class portfolioModel : PageModel
    {
        private readonly MyDbContext _db;

        public portfolioModel(MyDbContext db)
        {
            _db = db;
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
            int pageSize = 12;
            CurrentPage = pageNumber ?? 1;

            // Lấy sản phẩm đã được duyệt và chưa mua
            var totalProducts = _db.Products.Where(p => p.StatusIsApprove == true && p.StatusIsBuy == false);

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
            products = await totalProducts
                .Skip((CurrentPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            categories = await _db.Categories.ToListAsync();

            return Page();
        }


    }
}
