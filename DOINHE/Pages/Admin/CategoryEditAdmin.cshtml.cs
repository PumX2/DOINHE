using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DOINHE_BusinessObject;

namespace DOINHE.Pages.Admin
{
    public class CategoryEditAdminModel : PageModel
    {
        [BindProperty]
        public Category Category { get; set; } = new Category();

        private readonly IHttpClientFactory _httpClientFactory;

        public CategoryEditAdminModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (HttpContext.Session.GetString("admin") == null)
            {
                return RedirectToPage("/Index"); 
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new System.Uri("https://localhost:7023/api/");

                var category = await client.GetFromJsonAsync<Category>($"Category/{id}");
                if (category == null)
                {
                    return NotFound();
                }

                Category = category;
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, $"Error connecting to API: {ex.Message}");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string action)
        {
            // Kiểm tra nếu session không có giá trị "admin", chuyển hướng về trang Index
            if (HttpContext.Session.GetString("admin") == null)
            {
                return RedirectToPage("/Index"); // Chuyển hướng về trang Index
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (action == "delete")
            {
                // Gọi API để xóa danh mục
                try
                {
                    var client = _httpClientFactory.CreateClient();
                    client.BaseAddress = new System.Uri("https://localhost:7023/api/");
                    var response = await client.DeleteAsync($"Category/{Category.Id}");

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToPage("CategoryAdmin");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Failed to delete the category.");
                    }
                }
                catch (HttpRequestException ex)
                {
                    ModelState.AddModelError(string.Empty, $"Error connecting to API: {ex.Message}");
                }
            }
            else
            {
                // Cập nhật danh mục
                try
                {
                    var client = _httpClientFactory.CreateClient();
                    client.BaseAddress = new System.Uri("https://localhost:7023/api/");
                    var response = await client.PutAsJsonAsync($"Category/{Category.Id}", Category);

                    if (!response.IsSuccessStatusCode)
                    {
                        ModelState.AddModelError(string.Empty, "Failed to update the category.");
                        return Page();
                    }
                }
                catch (HttpRequestException ex)
                {
                    ModelState.AddModelError(string.Empty, $"Error connecting to API: {ex.Message}");
                    return Page();
                }
            }

            return RedirectToPage("CategoryAdmin");
        }
    }
}
