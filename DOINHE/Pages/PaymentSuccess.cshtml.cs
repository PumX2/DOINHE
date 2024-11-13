using DOINHE.Db;
using DOINHE.Entitys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DOINHE.Pages
{
    public class PaymentSuccessModel : PageModel
    {
        private readonly MyDbContext _db;
        public User Users { get; set; }
        public Entitys.Product Product { get; set; }
        public Order Order { get; set; }

        public PaymentSuccessModel(MyDbContext db)
        {
            _db = db;
        }

        public async Task OnGetAsync()
        {
            var accountJson = HttpContext.Session.GetString("Account");
            var member = JsonSerializer.Deserialize<User>(accountJson);
            int userIDs = (int)member.Id;
            var user = await _db.Users.FirstOrDefaultAsync(a => a.Id == userIDs);

            var productJson = HttpContext.Session.GetString("Product");
            var products = JsonSerializer.Deserialize<User>(productJson);
            int productIds = (int)products.Id;
            var product = await _db.Products.FirstOrDefaultAsync(b => b.Id == productIds);

            Product = product;

            product.quantityInStock -= 1;
            await _db.SaveChangesAsync();
            if (product.quantityInStock <= 0) { product.StatusIsBuy = true; }

            // Trừ tiền của người dùng bằng giá sản phẩm
            //user.Money -= product.Price;
            //user.Money -= product.Price;
            //_db.Users.Update(user);
            //_db.Entry(user).State = EntityState.Modified;
            //await _db.SaveChangesAsync();

            var newOrder = new Order
            {
                ProductId = product.Id,
                UserId = user.Id,
                OrderDate = DateTime.Now,
                Price = product.Price,
                Status = false 
            };
            Order = newOrder;
            _db.Orders.Add(newOrder);
            await _db.SaveChangesAsync();
        }
    }
}
