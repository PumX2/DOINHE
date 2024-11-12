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
        public OrderDetailModel OrderDetail { get; set; }
        public DOINHE_BusinessObject.Product Product { get; set; }
        public Order Order { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            using (var client = new HttpClient())
            {
                // Sử dụng https://localhost:7023 cho API
                client.BaseAddress = new System.Uri("https://localhost:7023/api/");

                try
                {
                    // Lấy thông tin sản phẩm và đơn hàng
                    var productResponse = await client.GetFromJsonAsync<DOINHE_BusinessObject.Product>($"Product/{id}");
                    var orderResponse = await client.GetFromJsonAsync<Order>($"Order/{id}");

                    if (productResponse == null || orderResponse == null)
                    {
                        return NotFound();
                    }

                    // Cập nhật model
                    OrderDetail = new OrderDetailModel
                    {
                        Product = productResponse,
                        Order = orderResponse
                    };
                }
                catch (HttpRequestException ex)
                {
                    // Xử lý lỗi khi có lỗi mạng hoặc phản hồi không thành công
                    ModelState.AddModelError(string.Empty, $"Request failed: {ex.Message}");
                    return Page();
                }

                return Page();
            }
        }

    }
}
