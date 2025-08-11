using ContosoPizzaNoSQl.Models;
using ContosoPizzaNoSQl.Repositories.Interfaces;
using ContosoPizzaNoSQl.Services.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

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

    public async Task<Order?> CreateOrderAsync(string customerId, List<OrderItem> orderItems)
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
            var order = new Order
            {
                CustomerId = customerId,
                OrderItems = orderItems,
                TotalAmount = totalAmount,
                CreatedAt = DateTime.UtcNow,
                CustomerName = customer.Name
            };
            await _orderRepository.CreateAsync(order);
            return order;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error creating order", ex);
        }
    }
    public async Task DeleteOrderAsync(string id)
    {
        try
        {
            await _orderRepository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error deleting order", ex);
        }
    }

    public async Task<Order?> GetOrderByIdAsync(string id)
    {
        try
        {

            if (!ObjectId.TryParse(id, out _))
            {
                throw new GraphQLException(
                    ErrorBuilder.New()
                        .SetMessage("Invalid order ID format")
                        .SetCode("INVALID_ID")
                        .Build()
                );
            }
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                throw new GraphQLException(
                ErrorBuilder.New()
                    .SetMessage("Order not found")
                    .SetCode("ORDER_NOT_FOUND")
                    .Build()
                );
            }
            return order;
        }
        catch (GraphQLException)
        {
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving order: {ex.Message}");
            throw new GraphQLException(
                ErrorBuilder.New()
                    .SetMessage("An unexpected error occurred while retrieving the order")
                    .SetCode("INTERNAL_ERROR")
                    .Build()
            );

        }
    }

    public async Task<List<Order>> GetAllOrdersAsync()
    {
        try
        {
            return await _orderRepository.GetAllAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error getting orders", ex);
        }
    }

    public async Task<List<Order>> GetOrdersByCustomerIdAsync(string customerId)
    {
        try
        {
            return await _orderRepository.GetByCustomerIdAsync(customerId);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error getting orders for customer with ID {customerId}", ex);
        }
    }

    public Task<List<Order>> GetOrdersAsync(int pageNumber, int pageSize, string sortBy, string order)
    {
        try
        {
            bool ascending = order.Equals("ASC", StringComparison.OrdinalIgnoreCase);
            var sortDefinition = sortBy switch
            {
                "totalamount" => ascending ? Builders<Order>.Sort.Ascending(o => o.TotalAmount) : Builders<Order>.Sort.Descending(o => o.TotalAmount),
                "createdat" => ascending ? Builders<Order>.Sort.Ascending(o => o.CreatedAt) : Builders<Order>.Sort.Descending(o => o.CreatedAt),
                _ => ascending ? Builders<Order>.Sort.Ascending(o => o.CustomerName) : Builders<Order>.Sort.Descending(o => o.CustomerName)
            };

            return _orderRepository.GetAsync(pageNumber, pageSize, sortDefinition);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error getting orders", ex);
        }
    }
}
