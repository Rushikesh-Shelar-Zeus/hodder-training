using ContosoPizzaNoSQl.Models;
using ContosoPizzaNoSQl.Repositories.Interfaces;
using ContosoPizzaNoSQl.Services.Interfaces;

namespace ContosoPizzaNoSQl.Services;

public class OrderService : IOrderService
{
    private readonly ICustomerService _customerService;
    private readonly IPizzaService _pizzaService;
    private readonly IOrderRepository _orderRepository;

    public OrderService(
        ICustomerService customerService,
        IPizzaService pizzaService,
        IOrderRepository orderRepository)
    {
        _customerService = customerService;
        _pizzaService = pizzaService;
        _orderRepository = orderRepository;
    }

    public async Task CreateOrderAsync(string customerId, List<OrderItem> orderItems)
    {
        try
        {
            var customer = await _customerService.GetCustomerByIdAsync(customerId);
            if (customer == null)
            {
                throw new ArgumentException("Customer not found");
            }

            decimal totalAmount = 0;
            foreach (var item in orderItems)
            {
                var pizza = await _pizzaService.GetPizzaByIdAsync(item.PizzaId);
                if (pizza == null)
                {
                    throw new ArgumentException($"Pizza with ID {item.PizzaId} not found");
                }
                item.PizzaName = pizza.Name;
                item.UnitPrice = pizza.Price;
                item.IsGlutenFree = pizza.IsGlutenFree;
                totalAmount += pizza.Price * item.Quantity;
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error creating order", ex);
        }
    }
    public Task DeleteOrderAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<Order?> GetOrderByIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Order>> GetOrdersAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<Order>> GetOrdersByCustomerIdAsync(string customerId)
    {
        throw new NotImplementedException();
    }
}
