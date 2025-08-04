using ContosoPizza.Models;
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;

namespace ContosoPizza.Services;

public class PizzaService : IPizzaService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<PizzaService> _logger;
    public PizzaService(IConfiguration configuration, ILogger<PizzaService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    private IDbConnection Connection =>
        new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

    public async Task<IEnumerable<Pizza>> GetAllPizzasAsync()
    {
        try
        {
            using var db = Connection;
            string sql = "SELECT * FROM Pizzas";
            return await db.QueryAsync<Pizza>(sql);
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "[AllPizzasAsync] Error retrieving pizzas from database.");
            throw;
        }
    }

    public async Task<Pizza?> GetPizzaByIdAsync(int id)
    {
        try
        {
            using var db = Connection;
            string sql = "SELECT * FROM Pizzas WHERE Id = @Id";
            return await db.QueryFirstOrDefaultAsync<Pizza>(sql, new { Id = id });
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "[PizzaByIdAsync] Error retrieving pizza with Id {id} from database.", id);
            throw;
        }
    }
    
    public async Task<int?> AddPizzaAsync(Pizza pizza)
    {
        try
        {
            using var db = Connection;
            string sql = "INSERT INTO Pizzas (Name, IsGlutenFree) OUTPUT INSERTED.Id VALUES (@Name, @IsGlutenFree)";
            var id = await db.ExecuteScalarAsync<int>(sql, pizza);
            pizza.Id = id; 
            return id;
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, $"[AddPizzaAsync] Error Adding pizza to Database.");
            throw;
        }
    }

    public async Task UpdatePizzaAsync(int id, Pizza pizza)
    {
        try
        {
            using var db = Connection;
            string sql = "UPDATE Pizzas SET Name = @Name, IsGlutenFree = @IsGlutenFree WHERE Id = @Id";
            var parameters = new { pizza.Name, pizza.IsGlutenFree, Id = id };
            await db.ExecuteAsync(sql, parameters);
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "[UpdatePizzaAsync] Error Updating pizza with Id {id} in Database.", id);
            throw;
        }
    }

    public async Task DeletePizzaAsync(int id)
    {
        try
        {
            using var db = Connection;
            string sql = "DELETE FROM Pizzas WHERE Id = @Id";
            await db.ExecuteAsync(sql, new { Id = id });
        }
        catch(SqlException ex)
        {
            _logger.LogError(ex, "[DeletePizzaAsync] Error Deleting pizza with Id {id} in Database.", id);
            throw;
        }
    }
}