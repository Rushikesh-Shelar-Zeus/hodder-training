using ContosoPizza.Dtos.Orders;

namespace ContosoPizza.Services.Interfaces;

public interface IOrderService
{
    Task<int> CreateOrderAsync(CreateOrderDto dto);
    Task<OrderDto?> GetOrderByIdAsync(int id);
    Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
}
