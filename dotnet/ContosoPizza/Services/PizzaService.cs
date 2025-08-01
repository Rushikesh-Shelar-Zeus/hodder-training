using ContosoPizza.Models;
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;

namespace ContosoPizza.Services;

public class PizzaService : IPizzaService
{
    private readonly IConfiguration _configuration;

    public PizzaService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private IDbConnection Connection =>
        new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

    public async Task<IEnumerable<Pizza>> GetAllPizzasAsync()
    {
        using var db = Connection;
        string sql = "SELECT * FROM Pizzas";
        return await db.QueryAsync<Pizza>(sql);
    }

    public async Task<Pizza?> GetPizzaByIdAsync(int id)
    {
        using var db = Connection;
        string sql = "SELECT * FROM Pizzas WHERE Id = @Id";
        return await db.QueryFirstOrDefaultAsync<Pizza>(sql, new { Id = id });
    }

    public async Task AddPizzaAsync(Pizza pizza)
    {
        using var db = Connection;
        string sql = "INSERT INTO Pizzas (Name, IsGlutenFree) VALUES (@Name, @IsGlutenFree)";
        await db.ExecuteAsync(sql, pizza);
    }

    public async Task UpdatePizzaAsync(int id, Pizza pizza)
    {
        using var db = Connection;
        string sql = "UPDATE Pizzas SET Name = @Name, IsGlutenFree = @IsGlutenFree WHERE Id = @Id";
        var parameters = new { pizza.Name, pizza.IsGlutenFree, Id = id };  
        await db.ExecuteAsync(sql, parameters);
    }

    public async Task DeletePizzaAsync(int id)
    {
        using var db = Connection;
        string sql = "DELETE FROM Pizzas WHERE Id = @Id";
        await db.ExecuteAsync(sql, new { Id = id });
    }
}