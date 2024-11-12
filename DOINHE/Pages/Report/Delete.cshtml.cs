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
    public class DeleteModel : PageModel
    {
        private readonly DOINHE.Db.MyDbContext _context;

        public DeleteModel(DOINHE.Db.MyDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Reports == null)
            {
                return NotFound();
            }
            var report = await _context.Reports.FindAsync(id);

            if (report != null)
            {
                Report = report;
                _context.Reports.Remove(Report);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
