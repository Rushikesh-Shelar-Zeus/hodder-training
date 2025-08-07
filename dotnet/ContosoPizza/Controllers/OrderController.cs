using Microsoft.AspNetCore.Mvc;

using ContosoPizza.Dtos.Orders;
using ContosoPizza.Services.Interfaces;
using ContosoPizza.Dtos.Pagination;

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

    //GET: /api/all
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllOrders()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        return Ok(orders);
    }

    //GET: /api/order
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllOrders([FromQuery] PagedQueryParams queryParams)
     {
        if (queryParams.PageNumber < 1 || queryParams.PageSize < 1)
        {
            return BadRequest(new { Message = "Page number and page size must be greater than zero." });
        }
        var pizzas = await _orderService.GetPagedOrdersAsync(queryParams);
        return Ok(pizzas);
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
}