using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;

using ContosoPizza.Repositories.Interfaces;
using ContosoPizza.Models;
using Microsoft.VisualBasic;

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

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        try
        {
            using var db = Connection;
            string storedProcedure = "Order_GetAll";
            return await db.QueryAsync<Order>(storedProcedure, commandType: CommandType.StoredProcedure);
        }
        catch (SqlException ex)
        {
            throw new Exception("Error retrieving all orders", ex);
        }
    }

    public async Task<IEnumerable<Order>> GetPagedAsync(int pageNumber, int pageSize, string? sortBy = null, string sortDirection = "asc")
    {
        try
        {
            using var db = Connection;
            var parameters = new DynamicParameters();
            parameters.Add("@PageNumber", pageNumber);
            parameters.Add("@PageSize", pageSize);
            parameters.Add("@SortBy", sortBy);
            parameters.Add("@SortDirection", sortDirection);

            var orders = await db.QueryAsync<Order>(
                "Order_GetPaged",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return orders;
        }
        catch (SqlException ex)
        {
            throw new Exception("Error retrieving paged orders", ex);
        }
    }

    public Task<IEnumerable<OrderItem>> GetItemsByOrderIdAsync(int orderId)
    {
        throw new NotImplementedException("Method not implemented yet.");
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        try
        {
            using var db = Connection;

            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            string storedProcedure = "Order_GetById";
            using var multi = await db.QueryMultipleAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

            var order = await multi.ReadFirstOrDefaultAsync<Order>();
            if (order == null)
            {
                return null;
            }

            var items = await multi.ReadAsync<OrderItem>();
            order.OrderItems = items.ToList();

            return order;
        }
        catch (SqlException ex)
        {
            throw new Exception($"Error retrieving order with ID {id}", ex);
        }
    }

    public async Task<int> CreateWithItemsAsync(Order order)
    {
        using var db = Connection;
        using var transaction = db.BeginTransaction();
        try
        {

            var orderParameters = new DynamicParameters();
            orderParameters.Add("@CustomerId", order.CustomerId);
            orderParameters.Add("@OrderDate", order.OrderDate);
            orderParameters.Add("@TotalAmount", order.TotalAmount);
            orderParameters.Add("@NewOrderId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            string orderStoredProcedure = "Order_Create";
            await db.ExecuteAsync(orderStoredProcedure, orderParameters, transaction, commandType: CommandType.StoredProcedure);
            int orderId = orderParameters.Get<int>("@NewOrderId");

            if (order.OrderItems != null)
            {
                foreach (var item in order.OrderItems)
                {
                    var itemParameters = new DynamicParameters();
                    itemParameters.Add("@OrderId", orderId);
                    itemParameters.Add("@PizzaId", item.PizzaId);
                    itemParameters.Add("@Quantity", item.Quantity);
                    itemParameters.Add("@UnitPrice", item.UnitPrice);
                    itemParameters.Add("@NewItemId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    string itemStoredProcedure = "OrderItem_Create";
                    await db.ExecuteAsync(itemStoredProcedure, itemParameters, transaction, commandType: CommandType.StoredProcedure);
                }
            }

            transaction.Commit();
            return orderId;
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Error creating order: {ex.Message}");
            transaction.Rollback();
            return -1;
        }
    }
    
    public async Task<bool> DeleteWithItemsAsync(int id)
    {
        using var db = Connection;
        using var transaction = db.BeginTransaction();
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);  

            string deleteOrderProcedure = "Order_Delete";
            await db.ExecuteAsync(deleteOrderProcedure, parameters, transaction, commandType: CommandType.StoredProcedure);

            transaction.Commit();
            return true;
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Error deleting order with ID {id}: {ex.Message}");
            transaction.Rollback();
            return false;
        }
    }

}