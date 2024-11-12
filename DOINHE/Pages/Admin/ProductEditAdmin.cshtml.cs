using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DOINHE_BusinessObject;

namespace DOINHE.Pages.Admin
{
    public class ProductEditAdminModel : PageModel
    {
        [BindProperty]
        public DOINHE_BusinessObject.Product Product { get; set; } = new DOINHE_BusinessObject.Product();

        public async Task<IActionResult> OnGetAsync(int id)
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

                    var product = await client.GetFromJsonAsync<DOINHE_BusinessObject.Product>($"Product/{id}");
                    if (product == null)
                    {
                        return NotFound();
                    }

                    Product = product;
                }
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, $"Error connecting to API: {ex.Message}");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri("https://localhost:7023/api/");
                    var response = await client.PutAsJsonAsync($"Product/{Product.Id}", Product);

                    if (!response.IsSuccessStatusCode)
                    {
                        ModelState.AddModelError(string.Empty, "Failed to update the product.");
                        return Page();
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, $"Error connecting to API: {ex.Message}");
                return Page();
            }

            return RedirectToPage("ProductAdmin");
        }
    }
}
