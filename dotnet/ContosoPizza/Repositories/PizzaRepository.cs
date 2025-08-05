using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

using ContosoPizza.Models;

namespace ContosoPizza.Repositories;

public class PizzaRepository : IPizzaRepository
{
    private readonly IConfiguration _configuration;

    private readonly ILogger<PizzaRepository> _logger;

    public PizzaRepository(IConfiguration configuration, ILogger<PizzaRepository> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    private IDbConnection Connection =>
        new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

    public async Task<IEnumerable<Pizza>> GetAllAsync()
    {
        try
        {
            using var db = Connection;
            string storedProcedure = "Pizza_GetAll";

            return await db.QueryAsync<Pizza>(storedProcedure, commandType: CommandType.StoredProcedure);
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "[AllPizzasAsync] Error retrieving pizzas from database.");
            throw;
        }
    }

    public async Task<Pizza?> GetByIdAsync(int id)
    {
        try
        {
            using var db = Connection;
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id, DbType.Int32);
            string storedProcedure = "Pizza_GetById";

            return await db.QueryFirstOrDefaultAsync<Pizza>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "[PizzaByIdAsync] Error retrieving pizza with Id {id} from database.", id);
            throw;
        }
    }

    public async Task<int> CreateAsync(Pizza pizza)
    {
        try
        {
            using var db = Connection;
            var parameters = new DynamicParameters();
            parameters.Add("@Name", pizza.Name, DbType.String);
            parameters.Add("@IsGlutenFree", pizza.IsGlutenFree, DbType.Boolean);
            parameters.Add("@Price", pizza.Price, DbType.Decimal);

            parameters.Add("@NewId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            
            string storedProcedure = "Pizza_Create";
            await db.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("@NewId");
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, $"[CreatePizzaAsync] Error Adding pizza to Database.");
            throw;
        }
    }

    public async Task<bool> UpdateAsync(int id, Pizza pizza)
    {
        try
        {
            using var db = Connection;
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id, DbType.Int32);
            parameters.Add("@Name", pizza.Name, DbType.String);
            parameters.Add("@IsGlutenFree", pizza.IsGlutenFree, DbType.Boolean);
            parameters.Add("@Price", pizza.Price, DbType.Decimal);
            parameters.Add("@RowsAffected", dbType: DbType.Int32, direction: ParameterDirection.Output);

            string storedProcedure = "Pizza_Update";

            await db.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            
            return parameters.Get<int>("@RowsAffected") > 0;
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "[UpdatePizzaAsync] Error Updating pizza with Id {id} in Database.", id);
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

            string storedProcedure = "Pizza_Delete";
            await db.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("@RowsAffected") > 0;
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "[DeletePizzaAsync] Error Deleting pizza with Id {id} in Database.", id);
            throw;
        }
    }
}