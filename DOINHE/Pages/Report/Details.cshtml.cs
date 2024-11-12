using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DOINHE.Db;
using DOINHE.Entitys;

namespace DOINHE.Pages.Report
{
    public class DetailsModel : PageModel
    {
        private readonly DOINHE.Db.MyDbContext _context;

        public DetailsModel(DOINHE.Db.MyDbContext context)
        {
            _context = context;
        }

      public Entitys.Report Report { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Reports == null)
            {
                return NotFound();
            }

            var report = await _context.Reports.FirstOrDefaultAsync(m => m.Id == id);
            if (report == null)
            {
                return NotFound();
            }
            else 
            {
                Report = report;
            }
            return Page();
        }
    }
}
