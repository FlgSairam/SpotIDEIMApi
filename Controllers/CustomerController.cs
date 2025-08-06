using DapperAuthApi.Models;
using DapperAuthApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DapperAuthApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly CustomerRepository _repository;

    public CustomerController(CustomerRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _repository.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var customer = await _repository.GetByIdAsync(id);
        if (customer == null) return NotFound();
        return Ok(customer);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Customer customer)
    {
        await _repository.CreateAsync(customer);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, Customer customer)
    {
        await _repository.UpdateAsync(id, customer);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _repository.DeleteAsync(id);
        return Ok();
    }
}
