using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using DOINHE_BusinessObject;
using DOINHE_Repository;

namespace DOINHE1_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ODataController
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [EnableQuery(PageSize = 20)]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_orderRepository.GetAllOrders());
        }

        [EnableQuery]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var order = _orderRepository.GetAllOrders().FirstOrDefault(o => o.Id == id);
            if (order == null)
                return NotFound();
            return Ok(order);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Order order)
        {
            if (order == null)
                return BadRequest("Order cannot be null.");

            _orderRepository.SaveOrder(order);
            return CreatedAtAction(nameof(Get), new { id = order.Id }, order);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Order order)
        {
            if (order == null)
                return BadRequest("Order cannot be null.");

            var existingOrder = _orderRepository.GetAllOrders().FirstOrDefault(o => o.Id == id);
            if (existingOrder == null)
                return NotFound();

            _orderRepository.UpdateOrder(order);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var order = _orderRepository.GetAllOrders().FirstOrDefault(o => o.Id == id);
            if (order == null)
                return NotFound();

            _orderRepository.DeleteOrder(order);
            return NoContent();
        }
    }
}
