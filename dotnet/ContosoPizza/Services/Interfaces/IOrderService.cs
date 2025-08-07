using ContosoPizza.Dtos.Orders;
using ContosoPizza.Dtos.Pagination;

namespace ContosoPizza.Services.Interfaces;

public interface IOrderService
{
    Task<int> CreateOrderAsync(CreateOrderDto dto);
    Task<OrderDto?> GetOrderByIdAsync(int id);
    Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
    Task<PagedResult<OrderDto>> GetPagedOrdersAsync(PagedQueryParams queryParams);

}
