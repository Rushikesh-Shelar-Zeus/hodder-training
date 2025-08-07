using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;

using ContosoPizza.Repositories.Interfaces;
using ContosoPizza.Models;
using ContosoPizza.Dtos.Pagination;
using ContosoPizza.Dtos.Orders;

namespace ContosoPizza.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly IConfiguration _configuration;

    public OrderRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private IDbConnection Connection
        => new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

    public async Task<int> CreateWithItemsAsync(Order order)
    {
        try
        {
            using var db = Connection;
            string storedProcedure = "Order_Create";
            var parameters = new DynamicParameters();
            parameters.Add("@CustomerId", order.CustomerId);
            parameters.Add("@OrderDate", order.OrderDate);
            parameters.Add("@TotalAmount", order.TotalAmount);

            parameters.Add("@NewOrderId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await db.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

            int orderId = parameters.Get<int>("@NewOrderId");

            if (orderId <= 0)
            {
                throw new Exception("Failed to create order in Database.");
            }

            if (order.OrderItems != null && order.OrderItems.Count != 0)
            {
                foreach (OrderItem item in order.OrderItems)
                {
                    string itemStoredProcedure = "OrderItem_Create";
                    var itmeParammeters = new DynamicParameters();
                    itmeParammeters.Add("@OrderId", orderId);
                    itmeParammeters.Add("@PizzaId", item.PizzaId);
                    itmeParammeters.Add("@Quantity", item.Quantity);
                    itmeParammeters.Add("@UnitPrice", item.UnitPrice);
                    itmeParammeters.Add("@NewItemId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    await db.ExecuteAsync(itemStoredProcedure, itmeParammeters, commandType: CommandType.StoredProcedure);
                    int itemId = itmeParammeters.Get<int>("@NewItemId");
                    if (itemId <= 0)
                    {
                        throw new Exception("Failed to create order item.");
                    }
                }
            }
            return orderId;
        }
        catch
        {
            throw new Exception("Failed to create order with items.");
        }
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        try
        {
            using var db = Connection;
            string storedProcedure = "Order_GetById";
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            return await db.QueryFirstOrDefaultAsync<Order>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        }
        catch
        {
            throw new Exception($"Failed to retrieve order with ID {id}.");
        }
    }

    public async Task<IEnumerable<OrderItem>> GetItemsByOrderIdAsync(int orderId)
    {
        try
        {
            using var db = Connection;
            string storedProcedure = "OrderItem_GetByOrderId";
            var parameters = new DynamicParameters();
            parameters.Add("@OrderId", orderId);

            return await db.QueryAsync<OrderItem>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        }
        catch
        {
            throw new Exception($"Failed to retrieve items for order with ID {orderId}.");
        }
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        try
        {
            using var db = Connection;
            string storedProcedure = "Order_GetAll";

            return await db.QueryAsync<Order>(storedProcedure, commandType: CommandType.StoredProcedure);
        }
        catch
        {
            throw new Exception("Failed to retrieve all orders.");
        }
    }

    public async Task<PagedResult<OrderDto>> GetPagedResultAsync(PagedQueryParams queryParams)
    {
        try
        {
            using var db = Connection;

            var parameters = new DynamicParameters();
            parameters.Add("@PageNumber", queryParams.PageNumber, DbType.Int32);
            parameters.Add("@PageSize", queryParams.PageSize, DbType.Int32);
            parameters.Add("@SortBy", queryParams.SortBy, DbType.String);
            parameters.Add("@SortDirection", queryParams.SortDirection, DbType.String);

            string storedProcedure = "Order_GetPagedSorted";
            using var multi = await db.QueryMultipleAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

            var items = (await multi.ReadAsync<OrderDto>()).ToList();
            var totalCount = await multi.ReadFirstAsync<int>();

            return new PagedResult<OrderDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = queryParams.PageNumber,
                PageSize = queryParams.PageSize
            };
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw new Exception("Failed to Fetch Paged Orders");
        }
    }
}