using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DOINHE.Db;
using DOINHE.Entitys;

namespace DOINHE.Pages.Report
{
    public class CreateModel : PageModel
    {
        private readonly DOINHE.Db.MyDbContext _context;

        public CreateModel(DOINHE.Db.MyDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int id)
        {
        ViewData["OrderId"] = id;
            
            return Page();
        }

        [BindProperty]
        public DTO.ReportDTO Report { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Reports == null || Report == null)
            {
                return Page();
            }
            Entitys.Report report = new Entitys.Report()
            {
                OrderId = Report.OrderId,
                ImgReport = Static.ConvertToByte.ConvertIFormFileToByte(Report.ImgReport!),
                Description = Report.Description
            };
            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
