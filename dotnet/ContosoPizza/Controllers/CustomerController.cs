using Microsoft.AspNetCore.Mvc;

using ContosoPizza.Services.Interfaces;
using ContosoPizza.Dtos.Customers;

namespace ContosoPizza.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    //GET: /api/customer
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAll()
    {
        var customers = await _customerService.GetAllAsync();
        return Ok(customers);
    }

    //GET: /api/customer/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerDto>> GetById(int id)
    {
        var customer = await _customerService.GetByIdAsync(id);
        if (customer is null)
        {
            return NotFound(new { Message = $"Customer with Id {id} not found" });
        }

        return Ok(customer);
    }

    //POST: /api/customer
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateCustomerDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var newId = await _customerService.CreateAsync(dto);

        if (newId < 0)
        {
            return StatusCode(500, new { Message = "Failed to Create Customer" });
        }

        var createdCustomer = await _customerService.GetByIdAsync(newId);
        if (createdCustomer is null)
        {
            return StatusCode(500, new { Message = "Customer was created but could not be fetched." });
        }

        return CreatedAtAction(nameof(GetById), new { Id = newId }, createdCustomer);
    }

    //PUT: /api/customer/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCustomerDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var success = await _customerService.UpdateAsync(id, dto);
        if (!success)
        {
            return NotFound(new { Message = $"Customer with ID {id} not found or update failed." });
        }
        return NoContent();
    }

    //DELETE: /api/customer/5
    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _customerService.DeleteAsync(id);
        if (!success)
        {
            return NotFound(new { Message = $"Customer with ID {id} not found" });
        }
        return NoContent();
    }
}