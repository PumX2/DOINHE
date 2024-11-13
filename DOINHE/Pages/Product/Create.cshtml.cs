using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DOINHE.Db;
using DOINHE.Entitys;
using System.Numerics;
using Microsoft.AspNetCore.Authorization;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Newtonsoft.Json;

namespace DOINHE.Pages.Product
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public CreateModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public SelectList CategoryList { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var accountJson = HttpContext.Session.GetString("Account");
            var member = JsonConvert.DeserializeObject<User>(accountJson);

            int userIDs = (int)member.Id;

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7023/api/category");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                var categories = JsonConvert.DeserializeObject<List<Category>>(json);
                CategoryList = new SelectList(categories, "Id", "CategoryName");
                // Do something with the categories
                return Page(); // Return categories to your view
            }
            // Sử dụng CategoryList để truyền danh sách các Category


            return BadRequest();
        }


        [BindProperty]
        public DTO.ProductDTO Product { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            List<Entitys.Product>? products = new();
            var client = _httpClientFactory.CreateClient();

            var response = await client.GetAsync("https://localhost:7023/api/product");
            if (response.IsSuccessStatusCode)
            {
                products = JsonConvert.DeserializeObject<List<Entitys.Product>>(await response.Content.ReadAsStringAsync());
            }


            if (!ModelState.IsValid || products == null || Product == null)
            {
                var response1 = await client.GetAsync("https://localhost:7023/api/Category");
                var response2 = await client.GetAsync("https://localhost:7023/api/User");
                if (!response1.IsSuccessStatusCode)
                {
                    return BadRequest(); // Return categories to your view
                }
                if (!response2.IsSuccessStatusCode)
                {
                    return BadRequest(); // Return categories to your view
                }
                var categories = JsonConvert.DeserializeObject<List<Entitys.Category>>(await response1.Content.ReadAsStringAsync());
                var users = JsonConvert.DeserializeObject<List<Entitys.User>>(await response2.Content.ReadAsStringAsync());
                ViewData["CategoryName"] = new SelectList(categories, "Id", "CategoryName");
                ViewData["UserId"] = new SelectList(users, "Id", "Email");
                return Page();
            }


            Entitys.Product product = new Entitys.Product()
            {

                ProductName = Product.ProductName,
                CategoryId = Product.CategoryId,
                UserId = int.Parse(HttpContext.Session.GetString("UserId")),
                DateTimeStart = Product.DateTimeStart,
                DateTimeEnd = Product.DateTimeEnd,
                CreateDate = DateTime.Now,
                Price = Product.Price,
                Address = Product.Address,
                Description = Product.Description,
                StatusIsBuy = false,
                StatusIsApprove = false,
                ImgDescription = Static.ConvertToByte.ConvertIFormFileToByte(Product.ImgDescription!),
                quantityInStock = Product.quantityInStock,
                Key = Product.Key,
                ImgKey = Static.ConvertToByte.ConvertIFormFileToByte(Product.ImgKey!)
            };

            var content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
            var response3 = await client.PostAsync("https://localhost:7023/api/Product", content);
            if (response.IsSuccessStatusCode)
            { return RedirectToPage("./Index"); }
            return BadRequest();


        }
    }
}
