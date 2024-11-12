using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DOINHE_BusinessObject;

namespace DOINHE.Pages.Admin
{
    public class OrderAdminModel : PageModel
    {
        public List<Order> Orders { get; set; } = new List<Order>();

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("admin") == null)
            {
                return RedirectToPage("/Index");
            }
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri("https://localhost:7023/api/");

                    Orders = await client.GetFromJsonAsync<List<Order>>("Order");
                }
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, $"Lỗi khi kết nối API: {ex.Message}");
            }
            return Page();
        }
    }
}
