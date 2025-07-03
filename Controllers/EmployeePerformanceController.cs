using DapperAuthApi.Models;
using DapperAuthApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DapperAuthApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeePerformanceController : ControllerBase
{
    private readonly EmployeePerformanceRepository _repository;

    public EmployeePerformanceController(EmployeePerformanceRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _repository.GetAllAsync();
        return result == null ? NotFound() : Ok(result);
    }

    [HttpGet("{employee_Id}/{qry_month}")]
    public async Task<IActionResult> GetById(long employee_Id, int qry_month)
    {
        var result = await _repository.GetByIdAsync(employee_Id, qry_month);
        return result == null ? NotFound() : Ok(result);
    }

    
    [HttpPost]
    public async Task<EmployeePerformanceResponse> Create(EmployeePerformance entity)
    {
        EmployeePerformanceResponse EmployeePerformance = new EmployeePerformanceResponse();
        EmployeePerformance = await _repository.CreateAsync(entity);
        return EmployeePerformance;
    }

    [HttpPut("{id}")]
    public async Task<EmployeePerformanceResponse> Update(long id, EmployeePerformance entity)
    {
        EmployeePerformanceResponse EmployeePerformance = new EmployeePerformanceResponse();
        entity.Pid = id;
        EmployeePerformance = await _repository.UpdateAsync(entity);
        return EmployeePerformance;
    }

    [HttpDelete("{id}")]
    public async Task<EmployeePerformanceResponse> Delete(long id)
    {
        EmployeePerformanceResponse EmployeePerformance = new EmployeePerformanceResponse();
        EmployeePerformance = await _repository.DeleteAsync(id);
        return EmployeePerformance;
    }
}
