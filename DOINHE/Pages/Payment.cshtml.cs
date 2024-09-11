using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Net.payOS;
using Net.payOS.Types;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DOINHE.Pages
{
    public class PaymentModel : PageModel
    {
        private readonly PayOS _payOS;

        public PaymentModel(PayOS payOS)
        {
            _payOS = payOS;
        }

        public string PaymentUrl { get; private set; }

        public async Task<IActionResult> OnGetAsync(int amount)
        {
            try
            {
                // Tạo mã đơn hàng (orderCode)
                long orderCode = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                // Tạo danh sách sản phẩm
                List<ItemData> items = new List<ItemData>
        {
            new ItemData("Product Name", 1, amount) // Thay "Product Name" bằng tên dịch vụ của bạn
        };

                // Tạo dữ liệu thanh toán
                PaymentData paymentData = new PaymentData(
                    orderCode: orderCode,
                    amount: amount,
                    description: "Thanh toán cho dịch vụ",
                    items: items,
                    cancelUrl: "https://example.com/cancel",
                    returnUrl: "https://example.com/success"
                );

                // Gọi API tạo liên kết thanh toán
                CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);

                // Kiểm tra kết quả
                if (createPayment != null && !string.IsNullOrEmpty(createPayment.checkoutUrl))
                {
                    PaymentUrl = createPayment.checkoutUrl;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to create payment link. No URL returned.");
                }

                return Page();
            }
            catch (Exception ex)
            {
                // Ghi lại thông tin lỗi chi tiết
                ModelState.AddModelError(string.Empty, "Error creating payment link: " + ex.Message);
                return Page();
            }
        }

    }
}
