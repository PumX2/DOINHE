using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DOINHE_BusinessObject;

namespace DOINHE.Pages.Admin
{
    public class ProductAdminModel : PageModel
    {
        public List<DOINHE_BusinessObject.Product> Products { get; set; } = new List<DOINHE_BusinessObject.Product>();

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

                    Products = await client.GetFromJsonAsync<List<DOINHE_BusinessObject.Product>>("Product");
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
