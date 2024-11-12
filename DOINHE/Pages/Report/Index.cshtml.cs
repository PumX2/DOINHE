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
    public class IndexModel : PageModel
    {
        private readonly DOINHE.Db.MyDbContext _context;

        public IndexModel(DOINHE.Db.MyDbContext context)
        {
            _context = context;
        }

        public IList<Entitys.Report> Report { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Reports != null)
            {
                Report = await _context.Reports
                .Include(r => r.Order).ToListAsync();
            }
        }
    }
}
