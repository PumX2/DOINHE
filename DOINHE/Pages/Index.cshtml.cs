using DOINHE.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DOINHE.Pages
{
    public class IndexModel : PageModel
    {
        private readonly MyDbContext _db;

        public IndexModel(MyDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public List<Entitys.Product> products { get; set; } = new List<Entitys.Product>();
        public async Task<IActionResult> OnGetAsync(string searchTerm)
        {
            products = _db.Products
                .Where(p => p.StatusIsApprove == true)
                .ToList();

            // Nếu có searchTerm thì lọc sản phẩm trước khi hiển thị
            if (!string.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p => p.ProductName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return Page();
        }

    }
}