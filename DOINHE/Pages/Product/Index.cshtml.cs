using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DOINHE.Db;
using DOINHE.Entitys;
using System.Text.Json;

namespace DOINHE.Pages.Product
{
    public class IndexModel : PageModel
    {
        private readonly DOINHE.Db.MyDbContext _context;

        public IndexModel(DOINHE.Db.MyDbContext context)
        {
            _context = context;
        }

        public IList<Entitys.Product> Product { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            var id = int.Parse(HttpContext.Session.GetString("UserId"));
            
            if (_context.Products != null)
            {
                Product = await _context.Products
                    .Where(p => p.UserId == id)
                .Include(p => p.Categories)
                .Include(p => p.Users).ToListAsync();
            }
            return Page();
        }
    }
}
