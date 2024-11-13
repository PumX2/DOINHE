using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DOINHE.Pages.Admin
{
    public class DashboardModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("admin") == null)
            {
                return RedirectToPage("/Index");
            }
            return Page();
        }
    }
}
