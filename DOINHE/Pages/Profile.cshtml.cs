using DOINHE.Db;
using DOINHE.Entitys;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace DOINHE.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly MyDbContext _db;
        public User users { get; set; }
        public List<OrderProductDto> OrderProducts { get; set; }

        public ProfileModel(MyDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var accountJson = HttpContext.Session.GetString("Account");
            var user = JsonSerializer.Deserialize<User>(accountJson);
            var userss = await _db.Users.FirstOrDefaultAsync(a => a.Id == user.Id);

            // Lấy danh sách order và thông tin sản phẩm, bao gồm cả hình ảnh
            OrderProducts = await _db.Orders
                .Where(a => a.UserId == user.Id)
                .Select(order => new OrderProductDto
                {
                    OrderId = order.Id,
                    ProductId = order.ProductId,
                    ProductName = order.Product.ProductName,
                    OrderPrice = order.Price,
                    Status = order.Status,
                    OrderDate = order.OrderDate,
                    ImgDescription = order.Product.ImgDescription // Lấy hình ảnh sản phẩm
                })
                .ToListAsync();

            users = userss;
            return Page();
        }


        // Phương thức này xử lý việc chỉnh sửa thông tin user
        public async Task<IActionResult> OnPostEditProfileAsync(string name, string phone)
        {
            var accountJson = HttpContext.Session.GetString("Account");
            var user = JsonSerializer.Deserialize<User>(accountJson);

            // Cập nhật thông tin user
            var userToUpdate = await _db.Users.FindAsync(user.Id);
            if (userToUpdate != null)
            {
                userToUpdate.Name = name;
                userToUpdate.Phone = phone;

                // Lưu thay đổi vào cơ sở dữ liệu
                await _db.SaveChangesAsync();

                // Cập nhật lại session sau khi thay đổi thông tin
                HttpContext.Session.SetString("Account", JsonSerializer.Serialize(userToUpdate));
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
            public byte[]? ImgDescription { get; internal set; }
        }
    }
}
