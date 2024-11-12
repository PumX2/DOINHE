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
    public class portfolio_detailsModel : PageModel
    {
        private readonly MyDbContext _db;

        public portfolio_detailsModel(MyDbContext db)
        {
            _db = db;
        }

        public Entitys.Product products { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (_db.Products == null)
            {
                return NotFound();
            }

            var product = await _db.Products.FirstOrDefaultAsync(a => a.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            products = product;
            return Page();
        }
    }
}
