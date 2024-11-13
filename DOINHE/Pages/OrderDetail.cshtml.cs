using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DOINHE_BusinessObject;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;

namespace DOINHE.Pages
{
    public class OrderDetailModel : PageModel
    {
        private readonly HttpClient _httpClient;

        // Inject IHttpClientFactory vào để tạo HttpClient
        public OrderDetailModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        // Các thuộc tính của model sẽ được hiển thị trong view
        public DOINHE_BusinessObject.Product Product { get; set; }
        public Order Order { get; set; }

        // Phương thức OnGetAsync để lấy thông tin sản phẩm và đơn hàng
        public async Task<IActionResult> OnGetAsync(int id)
        {

            // Gọi API để lấy thông tin sản phẩm và đơn hàng


            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new System.Uri("https://localhost:7023/api/");

                    var orderResponse = await client.GetFromJsonAsync<DOINHE_BusinessObject.Order>($"Order/{id}");
                    if (orderResponse == null)
                    {
                        return NotFound();
                    }

                    Order = orderResponse;
                }
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

                    var productResponse = await client.GetFromJsonAsync<DOINHE_BusinessObject.Product>($"Product/{Order.ProductId}");
                    if (productResponse == null)
                    {
                        return NotFound();
                    }

                    Product = productResponse;
                }
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, $"Error connecting to API: {ex.Message}");
            }
            // Cập nhật thông tin cho model




            return Page(); // Trả về trang với dữ liệu đã được cập nhật
        }
    }
}
