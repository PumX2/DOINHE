﻿using DOINHE.Db;
using DOINHE.Entitys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Net.payOS;
using Net.payOS.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Text.Json;
using System.Threading.Tasks;
//using DOINHE_BusinessObject;

namespace DOINHE.Pages
{
    public class PaymentModel : PageModel
    {
        private readonly PayOS _payOS;
        private readonly MyDbContext _db;

        public PaymentModel(PayOS payOS, MyDbContext db)
        {
            _db = db;
            _payOS = payOS;
        }

        public string PaymentUrl { get; private set; }
        public Entitys.Product products { get; set; }
        public User users { get; set; }
        public int UserIds { get; set; }

        // Tải sản phẩm từ CSDL và hiển thị trang Payment
        public async Task<IActionResult> OnGetAsync(int id, int UserId)
        {
            // Lấy session từ HttpContext
            var accountJson = HttpContext.Session.GetString("customer");

            if (!string.IsNullOrEmpty(accountJson))
            {
                // Giải mã đối tượng từ JSON
                var member = JsonConvert.DeserializeObject<User>(accountJson);

                if (member != null)
                {
                    UserIds = (int)member.Id;  // Lấy UserId
                }
            }
            var product = await _db.Products.FirstOrDefaultAsync(a => a.Id == id);
            var user = await _db.Users.FirstOrDefaultAsync(a => a.Id == UserIds);

            if (product == null)
            {
                return NotFound();
            }
            if (user == null)
            {
                return NotFound();
            }

            products = product;
            users = user;
            HttpContext.Session.SetString("Product", JsonConvert.SerializeObject(product));
            return Page(); // Trả về trang Payment mà không chuyển hướng
        }

        public async Task<IActionResult> OnPostCreatePaymentLinkAsync(int availableCredit, int id)
        {
            var product = await _db.Products.FirstOrDefaultAsync(a => a.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            var accountJson = HttpContext.Session.GetString("Account");
            if (string.IsNullOrEmpty(accountJson))
            {
                return RedirectToPage("/Login"); // Chuyển hướng tới trang đăng nhập nếu người dùng chưa đăng nhập
            }

            var user = JsonConvert.DeserializeObject<User>(accountJson);
            if (user == null)
            {
                return NotFound();
            }

            if (availableCredit >= product.Price)
            {
                product.quantityInStock -= 1;
                await _db.SaveChangesAsync();
                if (product.quantityInStock <= 0) { product.StatusIsBuy = true; }

                await _db.SaveChangesAsync();
                user.Money -= product.Price;
                var userdb = _db.Users.FirstOrDefault(x => x.Id == user.Id);

                userdb.Money -= product.Price;
                //_db.Users.Update(user);
                //_db.Entry(user).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                var newOrder = new Order
                {
                    ProductId = product.Id,
                    UserId = user.Id,
                    OrderDate = DateTime.Now,
                    Price = product.Price,
                    Status = false // Đã thanh toán
                };

                _db.Orders.Add(newOrder);
                await _db.SaveChangesAsync();

                TempData["Message"] = "Mua hàng thành công mà không cần thanh toán thêm.";
                return RedirectToPage("/Profile");
            }

            int totalPayment = (int)(product.Price - availableCredit);

            if (totalPayment < 0)
            {
                ModelState.AddModelError(string.Empty, "Số tiền thanh toán không hợp lệ.");
                return Page();
            }

            products = product;
            //string returnURL = "https://localhost:7040/PaymentSuccess?id=" + product.Id;
            //string returnURL2 = "https://localhost:7040/portfolio-details?id=" + product.Id;
            try
            {
                long orderCode = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                List<ItemData> items = new List<ItemData> { new ItemData(product.ProductName, 1, totalPayment) };

                PaymentData paymentData = new PaymentData(
                    orderCode: orderCode,
                    amount: totalPayment,
                    description: "Thanh toán cho dịch vụ",
                    items: items,
                    cancelUrl: "https://localhost:7040/Index",
                    returnUrl: "https://localhost:7040/PaymentSuccess"
                //cancelUrl: "https://localhost:7040/Index",
                //returnUrl: "https://localhost:7040/PaymentSuccess"
                );

                CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);

                if (createPayment != null && !string.IsNullOrEmpty(createPayment.checkoutUrl))
                {

                    PaymentUrl = createPayment.checkoutUrl;
                    return Page();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to create payment link. No URL returned.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error creating payment link: " + ex.Message);
            }

            return Page();
        }



    }
}


//using Microsoft.AspNetCore.Mvc.RazorPages;
//using System.Net.Http;
//using System.Net.Http.Json;
//using System.Threading.Tasks;

//namespace DOINHE.Pages
//{
//    public class PaymentModel : PageModel
//    {
//        public ProductDTO Product { get; set; }
//        public UserDTO User { get; set; }
//        public string PaymentUrl { get; set; }

//        private static readonly HttpClient _httpClient = new HttpClient
//        {
//            BaseAddress = new System.Uri("http://localhost:7023")
//        };

//        // Phương thức xử lý dữ liệu truyền vào và gọi API
//        public async Task OnGetAsync(int id, int userId)
//        {
//            Product = await _httpClient.GetFromJsonAsync<ProductDTO>($"api/Payment/product/{id}");
//            User = await _httpClient.GetFromJsonAsync<UserDTO>($"api/Payment/user/{userId}");
//        }

//        public async Task OnPostCreatePaymentLinkAsync(int availableCredit, int id, int userId)
//        {
//            var response = await _httpClient.PostAsJsonAsync("api/Payment/createPaymentLink", new
//            {
//                availableCredit,
//                productId = id,
//                userId
//            });

//            var paymentResponse = await response.Content.ReadFromJsonAsync<PaymentResponse>();
//            PaymentUrl = paymentResponse?.PaymentUrl;

//            if (!string.IsNullOrEmpty(PaymentUrl))
//            {
//                Response.Redirect(PaymentUrl);
//            }
//            else
//            {
//                await _httpClient.PostAsJsonAsync("api/Payment/paymentSuccess", new { productId = id, userId });
//            }
//        }

//        public class ProductDTO
//        {
//            public string ProductName { get; set; }
//            public decimal Price { get; set; }
//        }

//        public class UserDTO
//        {
//            public decimal Money { get; set; }
//        }

//        public class PaymentResponse
//        {
//            public string PaymentUrl { get; set; }
//        }
//    }
//}
