using Microsoft.AspNetCore.Mvc;

using ContosoPizza.Models;
using ContosoPizza.Services;

namespace ContosoPizza.Controllers;

[ApiController]
[Route("[controller]")]
public class PizzaController : ControllerBase
{
    private readonly IPizzaService _pizzaService;
    public PizzaController(IPizzaService pizzaService)
    {
        _pizzaService = pizzaService;
    }

    //GET all Action
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Pizza>>> GetAll()
    {
        var pizzas = await _pizzaService.GetAllPizzasAsync();
        return Ok(pizzas);
    }

    //GET by Id Action
    [HttpGet("{id}")]
    public async Task<ActionResult<Pizza>> Get(int id)
    {
        var pizza = await _pizzaService.GetPizzaByIdAsync(id);
        if (pizza is null)
        {
            return NotFound();
        }
        return Ok(pizza);
    }

    // POST action
    [HttpPost]
    public async Task<IActionResult> Create(Pizza pizza)
    {
        await _pizzaService.AddPizzaAsync(pizza);
        return CreatedAtAction(nameof(Get), new { id = pizza.Id }, pizza);
    }

    //Put Action
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Pizza pizza)
    {
        await _pizzaService.UpdatePizzaAsync(id, pizza);
        return NoContent();
    }

    //DELETE action
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _pizzaService.DeletePizzaAsync(id);
        return NoContent();
    }
}