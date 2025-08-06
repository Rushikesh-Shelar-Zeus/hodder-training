using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;

using ContosoPizza.Models;
using ContosoPizza.Repositories.Interfaces;

namespace ContosoPizza.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<CustomerRepository> _logger;

    public CustomerRepository(IConfiguration configuration, ILogger<CustomerRepository> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    private IDbConnection Connection =>
        new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        try
        {
            using var db = Connection;
            string storedProcedure = "Customer_GetAll";

            return await db.QueryAsync<Customer>(storedProcedure, commandType: CommandType.StoredProcedure);
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "[GetALlCustomersAsync] Error retrieving pizzas from database.");
            throw;
        }
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        try
        {
            using var db = Connection;
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id, DbType.Int32);

            string storedProcedure = "Customer_GetById";
            return await db.QueryFirstOrDefaultAsync<Customer>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "[CustomerByIdAsync] Error retrieving pizza with Id {id} from database.", id);
            return null;
        }
    }

    public async Task<int> CreateAsync(Customer customer)
    {
        try
        {
            using var db = Connection;
            var parameters = new DynamicParameters();
            parameters.Add("@Name", customer.Name, DbType.String);
            parameters.Add("@Email", customer.Email, DbType.String);
            parameters.Add("@PhoneNumber", customer.PhoneNumber, DbType.String);
            parameters.Add("@Address", customer.Address, DbType.String);

            parameters.Add("@NewId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            string storedProcedure = "Customer_Create";
            await db.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("@NewId");
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, $"[CreatePizzaAsync] Error Adding pizza to Database.");
            throw;
        }
    }

    public async Task<bool> UpdateAsync(int id, Customer customer)
    {
        try
        {
            using var db = Connection;
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id, DbType.Int32);
            parameters.Add("@Name", customer.Name, DbType.String);
            parameters.Add("@Email", customer.Email, DbType.String);
            parameters.Add("@PhoneNumber", customer.PhoneNumber, DbType.String);
            parameters.Add("@Address", customer.Address, DbType.String);

            parameters.Add("@RowsAffected", dbType: DbType.Int32, direction: ParameterDirection.Output);

            string storedProcedure = "Customer_Update";
            await db.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("@RowsAffected") > 0;
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "[UpdateCustomerAsync] Error updating customer with Id {id} in database.", id);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            using var db = Connection;
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id, DbType.Int32);
            parameters.Add("@RowsAffected", dbType: DbType.Int32, direction: ParameterDirection.Output);

            string storedProcedure = "Customer_Delete";
            await db.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("@RowsAffected") > 0;
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "[DeleteCustomerAsync] Error deleting customer with Id {id} from database.", id);
            throw;
        }
    }
}