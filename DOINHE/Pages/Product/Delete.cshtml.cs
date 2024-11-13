using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DOINHE.Db;
using DOINHE.Entitys;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace DOINHE.Pages.Product
{
    public class DeleteModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public DeleteModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [BindProperty]
        public Entitys.Product Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7023/api/Product");
            List<Entitys.Product>? products = new();
            if (response.IsSuccessStatusCode)
            {
                products = JsonConvert.DeserializeObject<List<Entitys.Product>>(await response.Content.ReadAsStringAsync());

            }
            if (id == null || products == null)
            {
                return NotFound();
            }

            var product = products.FirstOrDefault(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            else
            {
                Product = product;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7023/api/Product");
            List<Entitys.Product>? products = new();
            if (response.IsSuccessStatusCode)
            {
                products = JsonConvert.DeserializeObject<List<Entitys.Product>>(await response.Content.ReadAsStringAsync());

            }
            if (id == null || products == null)
            {
                return NotFound();
            }
            var product = products.Find(p => p.Id == id);

            if (product != null)
            {
                var response2 = await client.DeleteAsync("https://localhost:7023/api/Product/{id}");
                if (response.IsSuccessStatusCode)
                { return RedirectToPage("./Index"); }

            }
            return BadRequest();

        }
    }
}
