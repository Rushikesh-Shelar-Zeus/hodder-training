using ContosoPizza.Models;
using ContosoPizza.Dtos.Orders;
using ContosoPizza.Services.Interfaces;
using ContosoPizza.Repositories.Interfaces;

namespace ContosoPizza.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepo;
    private readonly IPizzaRepository _pizzaRepo;
    private readonly ICustomerRepository _customerRepo;
    private readonly ILogger<OrderService> _logger;

    public OrderService(
        IOrderRepository orderRepo,
        IPizzaRepository pizzaRepo,
        ICustomerRepository customerRepo,
        ILogger<OrderService> logger)
    {
        _orderRepo = orderRepo;
        _pizzaRepo = pizzaRepo;
        _customerRepo = customerRepo;
        _logger = logger;
    }

    public async Task<int> CreateOrderAsync(CreateOrderDto dto)
    {
        try
        {
            //1. Validate Customer
            var customerExists = await _customerRepo.GetByIdAsync(dto.CustomerId);
            if (customerExists is null)
            {
                _logger.LogError("Customer with ID {CustomerId} not found", dto.CustomerId);
                throw new Exception($"Customer with ID {dto.CustomerId} not found");
            }

            //2. Prepare OrderItems and Total
            decimal totalAmount = 0;

            var orderItems = new List<OrderItem>();

            foreach (var item in dto.OrderItems)
            {
                var pizza = await _pizzaRepo.GetByIdAsync(item.PizzaId);
                if (pizza is null)
                {
                    _logger.LogError("Pizza with ID {PizzaId} not found", item.PizzaId);
                    throw new Exception($"Pizza with ID {item.PizzaId} not found");
                }

                var subTotal = pizza.Price * item.Quantity;
                totalAmount += subTotal;

                orderItems.Add(new OrderItem
                {
                    PizzaId = pizza.Id,
                    Quantity = item.Quantity,
                    UnitPrice = pizza.Price
                });
            }

            //3. Create the Order Model
            var order = new Order
            {
                CustomerId = dto.CustomerId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalAmount,
                OrderItems = orderItems
            };

            var newOrderId = await _orderRepo.CreateWithItemsAsync(order);
            Console.WriteLine($"New Order ID: {newOrderId}");

            if (newOrderId <= 0)
            {
                _logger.LogError("Failed to create order for customer {CustomerId}", dto.CustomerId);
                throw new Exception("Failed to create order");
            }

            return newOrderId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating order for customer {CustomerId}", dto.CustomerId);
            return -1;
        }
    }

    public async Task<OrderDto?> GetOrderByIdAsync(int id)
    {
        try
        {
            var order = await _orderRepo.GetByIdAsync(id);
            if (order is null)
            {
                _logger.LogWarning("Order with ID {OrderId} not found", id);
                return null;
            }

            var orderItems = await _orderRepo.GetItemsByOrderIdAsync(id);

            var customer = await _customerRepo.GetByIdAsync(order.CustomerId);
            if (customer is null)
            {
                _logger.LogWarning("Customer with ID {CustomerId} not found for order {OrderId}", order.CustomerId, id);
                return null;
            }

            return new OrderDto
            {
                Id = order.Id,
                CustomerName = customer.Name,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                OrderItems = orderItems.Select(oi => new OrderItemDto
                {
                    PizzaId = oi.PizzaId,
                    PizzaName = oi.Pizza?.Name!,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                }).ToList()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving order with ID {OrderId}", id);
            return null;
        }
    }

    public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
    {
        try
        {
            var orders = await _orderRepo.GetAllAsync();
            var orderDtos = new List<OrderDto>();

            foreach (var order in orders)
            {
                var orderItems = await _orderRepo.GetItemsByOrderIdAsync(order.Id);
                var customer = await _customerRepo.GetByIdAsync(order.CustomerId);

                if (customer is null)
                {
                    _logger.LogWarning("Customer with ID {CustomerId} not found for order {OrderId}", order.CustomerId, order.Id);
                    throw new Exception($"Customer with ID {order.CustomerId} not found for order {order.Id}");
                }

                orderDtos.Add(new OrderDto
                {
                    Id = order.Id,
                    CustomerName = customer.Name,
                    OrderDate = order.OrderDate,
                    TotalAmount = order.TotalAmount,
                    OrderItems = orderItems.Select(oi => new OrderItemDto
                    {
                        PizzaId = oi.PizzaId,
                        PizzaName = oi.Pizza?.Name!,
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice,
                    }).ToList()
                });
            }

            return orderDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all orders");
            return Enumerable.Empty<OrderDto>();
        }
    }

    public async Task<IEnumerable<OrderDto>> GetPagedOrdersAsync(int pageNumber, int pageSize, string? sortBy = null, string sortDirection = "asc")
    {
        try
        {
            var orders = await _orderRepo.GetPagedAsync(pageNumber, pageSize, sortBy, sortDirection);
            var orderDtos = new List<OrderDto>();

            foreach (var order in orders)
            {
                var orderItems = await _orderRepo.GetItemsByOrderIdAsync(order.Id);
                var customer = await _customerRepo.GetByIdAsync(order.CustomerId);

                if (customer is null)
                {
                    _logger.LogWarning("Customer with ID {CustomerId} not found for order {OrderId}", order.CustomerId, order.Id);
                    throw new Exception($"Customer with ID {order.CustomerId} not found for order {order.Id}");
                }

                orderDtos.Add(new OrderDto
                {
                    Id = order.Id,
                    CustomerName = customer.Name,
                    OrderDate = order.OrderDate,
                    TotalAmount = order.TotalAmount,
                    OrderItems = orderItems.Select(oi => new OrderItemDto
                    {
                        PizzaId = oi.PizzaId,
                        PizzaName = oi.Pizza?.Name!,
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice,
                    }).ToList()
                });
            }

            return orderDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving paged orders");
            return [];
        }
    }
}