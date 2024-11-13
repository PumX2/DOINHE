using DOINHE_BusinessObject;
using DOINHE_Repository;
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using Net.payOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOINHE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly PayOS _payOS;

        public PaymentController(IProductRepository productRepository, IUserRepository userRepository, IOrderRepository orderRepository, PayOS payOS)
        {
            _productRepository = productRepository;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _payOS = payOS;
        }

        [HttpGet("product/{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = _productRepository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = _userRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost("createPaymentLink")]
        public async Task<IActionResult> CreatePaymentLink(int availableCredit, int productId, int userId)
        {
            var product = _productRepository.GetProductById(productId);
            var user = _userRepository.GetUserById(userId);

            if (product == null || user == null)
            {
                return NotFound("Product or User not found.");
            }

            if (availableCredit >= product.Price)
            {
                product.quantityInStock -= 1;
                if (product.quantityInStock <= 0) product.StatusIsBuy = true;

                user.Money -= product.Price;
                var newOrder = new Order
                {
                    ProductId = product.Id,
                    UserId = user.Id,
                    OrderDate = DateTime.Now,
                    Price = product.Price,
                    Status = false
                };
                _orderRepository.SaveOrder(newOrder);

                _productRepository.UpdateProduct(product);
                _userRepository.UpdateUser(user);

                return Ok(new { message = "Order created without additional payment." });
            }

            int totalPayment = (int)(product.Price - availableCredit);
            long orderCode = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            var items = new List<ItemData> { new ItemData(product.ProductName, 1, totalPayment) };

            var paymentData = new PaymentData(
                orderCode: orderCode,
                amount: totalPayment,
                description: "Thanh toán cho dịch vụ",
                items: items,
                cancelUrl: "http://doinhe.runasp.net/Index",
                returnUrl: "http://doinhe.runasp.net/PaymentSuccess"
            );

            var createPayment = await _payOS.createPaymentLink(paymentData);

            if (createPayment != null && !string.IsNullOrEmpty(createPayment.checkoutUrl))
            {
                return Ok(new { paymentUrl = createPayment.checkoutUrl });
            }

            return BadRequest("Failed to create payment link.");
        }

        [HttpPost("paymentSuccess")]
        public async Task<IActionResult> PaymentSuccess(int orderId, int productId, int userId)
        {
            var product = _productRepository.GetProductById(productId);
            var user = _userRepository.GetUserById(userId);

            if (product == null || user == null)
            {
                return NotFound("Product or User not found.");
            }

            product.quantityInStock -= 1;
            if (product.quantityInStock <= 0) product.StatusIsBuy = true;

            var newOrder = new Order
            {
                ProductId = product.Id,
                UserId = user.Id,
                OrderDate = DateTime.Now,
                Price = product.Price,
                Status = true // Đã thanh toán
            };

            _orderRepository.SaveOrder(newOrder);
            _productRepository.UpdateProduct(product);
            _userRepository.UpdateUser(user);

            return Ok(new { message = "Payment successful, order created." });
        }
    }
}
