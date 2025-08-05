using Microsoft.AspNetCore.Mvc;

using ContosoPizza.Services.Interfaces;
using ContosoPizza.Dtos;

namespace ContosoPizza.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PizzaController : ControllerBase
{
    private readonly IPizzaService _pizzaService;
    public PizzaController(IPizzaService pizzaService)
    {
        _pizzaService = pizzaService;

    }

    //GET: /api/pizza
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PizzaDto>>> GetAllPizzas()
    {
        var pizzas = await _pizzaService.GetAllPizzasAsync();
        return Ok(pizzas);
    }

    //GET: /api/pizza/5
    [HttpGet("{id}")]
    public async Task<ActionResult<PizzaDto>> GetById(int id)
    {
        var pizza = await _pizzaService.GetPizzaByIdAsync(id);
        if (pizza is null)
        {
            return NotFound(new { Message = $"Pizza with ID {id} not found." });
        }

        return Ok(pizza);
    }

    // POST: /api/pizza
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreatePizzaDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var newId = await _pizzaService.CreatePizzaAsync(dto);
        if (newId < 0)
        {
            return StatusCode(500, new { Message = "Failed to Create Pizza" });
        }

        var createdPizza = await _pizzaService.GetPizzaByIdAsync(newId);
        if (createdPizza == null)
            return StatusCode(500, new { Message = "Pizza was created but could not be fetched." });

        return CreatedAtAction(nameof(GetById), new { Id = newId }, createdPizza);
    }

    //PUT: /api/pizza/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePizzaDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var success = await _pizzaService.UpdatePizzaAsync(id, dto);
        if (!success)
        {
            return NotFound(new { Message = $"Pizza with ID {id} not found or update failed." });
        }
        return NoContent();
    }

    //DELETE: /api/pizza/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _pizzaService.DeletePizzaAsync(id);
        if (!success)
        {
            return NotFound(new { Message = $"Pizza with ID {id} not found or deletion failed." });
        }

        return NoContent();
    }
}