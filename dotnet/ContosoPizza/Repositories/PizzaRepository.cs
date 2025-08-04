using System.Data;
using ContosoPizza.Models;
using Dapper;
using Microsoft.Data.SqlClient;

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
            string storedProcedure = "GetAllPizzas";
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
            string storedProcedure = "GetPizzaById";
            return await db.QueryFirstOrDefaultAsync<Pizza>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "[PizzaByIdAsync] Error retrieving pizza with Id {id} from database.", id);
            throw;
        }
    }

    public async Task<int> AddAsync(Pizza pizza)
    {
        try
        {
            using var db = Connection;
            var parameters = new DynamicParameters();
            parameters.Add("@Name", pizza.Name, DbType.String);
            parameters.Add("@IsGlutenFree", pizza.IsGlutenFree, DbType.Boolean);

            parameters.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            string storedProcedure = "AddPizza";
            await db.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

            int newPizzaId = parameters.Get<int>("@Id");
            pizza.Id = newPizzaId;
            return newPizzaId;
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, $"[AddPizzaAsync] Error Adding pizza to Database.");
            throw;
        }
    }

    public async Task UpdateAsync(int id, Pizza pizza)
    {
        try
        {
            using var db = Connection;
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id, DbType.Int32);
            parameters.Add("@Name", pizza.Name, DbType.String);
            parameters.Add("@IsGlutenFree", pizza.IsGlutenFree, DbType.Boolean);
            string storedProcedure = "UpdatePizza";

            await db.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "[UpdatePizzaAsync] Error Updating pizza with Id {id} in Database.", id);
            throw;
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            using var db = Connection;
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id, DbType.Int32);
            string storedProcedure = "DeletePizza";
            await db.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "[DeletePizzaAsync] Error Deleting pizza with Id {id} in Database.", id);
            throw;
        }
    }
}