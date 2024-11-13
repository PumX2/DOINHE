using DOINHE.Db;
//using DOINHE.Entitys;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Azure;
using System.Text;
using DOINHE_BusinessObject;
using Microsoft.Identity.Client;

namespace DOINHE.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ProfileModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }
        public User users { get; set; }
        public DOINHE_BusinessObject.Product product { get; set; }
        public List<OrderProductDto> OrderProducts { get; set; } = new List<OrderProductDto>();
        public List<Order> Orders { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {

            try
            {
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new System.Uri("https://localhost:7023/api/");

                var user = await client.GetFromJsonAsync<User>($"User/{id}");
                if (user == null)
                {
                    return NotFound();
                }

                users = user;
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, $"Error connecting to API: {ex.Message}");
            }

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri("https://localhost:7023/api/");

                    Orders = await client.GetFromJsonAsync<List<Order>>("Order");
                }
                List<OrderProductDto> hh = new List<OrderProductDto>();
                foreach (var item in Orders.Where(o => o.UserId == id))
                {
                    try
                    {
                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new System.Uri("https://localhost:7023/api/");

                            var products = await client.GetFromJsonAsync<DOINHE_BusinessObject.Product>($"Product/{item.ProductId}");
                            if (products == null)
                            {
                                return NotFound();
                            }

                            product = products;
                            var o = new OrderProductDto
                            {
                                ImgDescription = product.ImgDescription,
                                OrderDate = item.OrderDate,
                                Status = item.Status,
                                OrderId = item.Id,
                                ProductId = product.Id,
                                OrderPrice = product.Price,
                                ProductName = products.ProductName
                            };
                            hh.Add(o);
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        ModelState.AddModelError(string.Empty, $"Error connecting to API: {ex.Message}");
                    }
                }
                OrderProducts.AddRange(hh);


            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, $"Lỗi khi kết nối API: {ex.Message}");
            }
            return Page();
        }


        // Phương thức này xử lý việc chỉnh sửa thông tin user
        public async Task<IActionResult> OnPostEditProfileAsync(string name, string phone)
        {
            var accountJson = HttpContext.Session.GetString("Account");
            var user = System.Text.Json.JsonSerializer.Deserialize<User>(accountJson);
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7023/api/User");
            List<Entitys.User>? a = new();
            if (response.IsSuccessStatusCode)
            {
                a = JsonConvert.DeserializeObject<List<Entitys.User>>(await response.Content.ReadAsStringAsync());

            }
            // Cập nhật thông tin user
            var userToUpdate = a.Find(a => a.Id == user.Id);
            if (userToUpdate != null)
            {
                userToUpdate.Name = name;
                userToUpdate.Phone = phone;

                // Prepare the JSON content for the API request
                var jsonContent = new StringContent(JsonConvert.SerializeObject(userToUpdate), Encoding.UTF8, "application/json");

                // Send the update request (PUT or PATCH depending on API)
                var updateResponse = await client.PutAsync($"https://localhost:7023/api/User/{userToUpdate.Id}", jsonContent);

                if (updateResponse.IsSuccessStatusCode)
                {
                    // Update the session after successful API update
                    HttpContext.Session.SetString("Account", System.Text.Json.JsonSerializer.Serialize(userToUpdate));
                }
                else
                {
                    // Handle unsuccessful update (optional)
                    Console.WriteLine($"Failed to update user. Status Code: {updateResponse.StatusCode}");
                }
            }

            // Trả về trang hiện tại sau khi lưu
            return RedirectToPage();
        }

        public class OrderProductDto
        {
            public int? OrderId { get; set; }
            public int? ProductId { get; set; }
            public string ProductName { get; set; }
            public double? OrderPrice { get; set; }
            public bool? Status { get; set; }
            public DateTime? OrderDate { get; set; }
            public byte[]? ImgDescription { get; set; }
        }
    }
}
