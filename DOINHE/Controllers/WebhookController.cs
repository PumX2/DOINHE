using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using System.Text.Json;
using DOINHE.Entitys;
using DOINHE.Db; // Đảm bảo bạn đã thêm namespace này
using Microsoft.EntityFrameworkCore;

namespace DOINHE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebhookController : ControllerBase
    {
        private readonly MyDbContext _db; // Khai báo DbContext

        public WebhookController(MyDbContext db)
        {
            _db = db; // Khởi tạo DbContext
        }

        [HttpPost]
        public async Task<IActionResult> ReceiveWebhook([FromBody] WebhookType webhookType)
        {
            // Kiểm tra dữ liệu webhook từ PayOS
            if (webhookType == null || string.IsNullOrEmpty(webhookType.code))
            {
                return BadRequest("Invalid webhook data.");
            }

            // Xử lý dựa trên trạng thái thanh toán
            if (webhookType.code == "successful") // Kiểm tra theo mã code tương ứng với thành công
            {
                // Lấy orderId từ webhookType.data nếu có
                var orderId = webhookType.data!.description; // Thay đổi nếu tên thuộc tính khác
                int OrderID = int.Parse(orderId);
                var order = await _db.Orders.FindAsync(OrderID); // Tìm đơn hàng trong CSDL

                if (order != null)
                {
                    order.Status = true; // Cập nhật trạng thái đơn hàng là đã thanh toán
                    await _db.SaveChangesAsync();
                }

                // Ở đây có thể gửi thông báo hay thực hiện hành động khác nếu cần
            }

            return Ok(); // Trả về OK để xác nhận đã nhận webhook
        }
    }
}
