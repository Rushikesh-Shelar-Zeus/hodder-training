using Microsoft.AspNetCore.Mvc;

using ContosoPizza.Dtos.Orders;
using ContosoPizza.Services.Interfaces;

namespace ContosoPizza.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    //GET: /api/order
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllOrders()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        return Ok(orders);
    }

    //GET: /api/order/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDto>> GetOrderById(int id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        if (order is null)
        {
            return NotFound(new { Message = $"Order with ID {id} not found" });
        }
        return Ok(order);
    }

    //POST: /api/order
    [HttpPost]
    public async Task<IActionResult> CreateOrderAsync([FromBody] CreateOrderDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        int newOrderId = await _orderService.CreateOrderAsync(dto);
        if (newOrderId < 0)
        {
            return StatusCode(500, new { Message = "Failed to create order" });
        }

        var createdOrder = await _orderService.GetOrderByIdAsync(newOrderId);
        return CreatedAtAction(nameof(GetOrderById), new { id = newOrderId }, createdOrder);
    }

    //GET: /api/order/paged
    [HttpGet("paged")]
    public async Task<IActionResult> GetPagedOrders(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string sortBy = "Date",
        [FromQuery] string sortDirection = "asc")
    {
        try
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest("Page number and page size must be greater than 0.");
            }

            var orders = await _orderService.GetPagedOrdersAsync(pageNumber, pageSize, sortBy, sortDirection);

            if (!orders.Any())
            {
                return NotFound("No orders found for the given page.");
            }

            return Ok(orders);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while retrieving orders.");
        }
    }
}