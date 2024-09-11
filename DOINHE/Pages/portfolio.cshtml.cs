using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DOINHE.Entitys;
using System.Diagnostics.Metrics;
using System.Text.Json;
using DOINHE.Db;
using Azure;

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
        public List<Product> products { get; set; } = new List<Product>();

        [BindProperty]
        public List<Category> categories { get; set; } = new List<Category>();

        public async Task<IActionResult> OnGetAsync()
        {
            int pageSize = 8;

            products = _db.Products.Where(p => p.StatusIsApprove == true).ToList();
            categories = _db.Categories.ToList();    
            return Page();
        }
    }
}
