using DapperAuthApi.Models;
using DapperAuthApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DapperAuthApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeAttendanceController : ControllerBase
{
    private readonly EmployeeAttendanceRepository _repository;

    public EmployeeAttendanceController(EmployeeAttendanceRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var result = await _repository.GetAllAsync();
        return result == null ? NotFound() : Ok(result);
    }

    [HttpGet("{employee_Id}/{qry_month}")]
    [Authorize]
    public async Task<IActionResult> GetById(long employee_Id, int qry_month)
    {
        var result = await _repository.GetByIdAsync(employee_Id, qry_month);
        return result == null ? NotFound() : Ok(result);
    }

    
    [HttpPost]
    [Authorize]
    public async Task<EmployeeAttendanceResponse> Create(EmployeeAttendance entity)
    {
        EmployeeAttendanceResponse employeeAttendance = new EmployeeAttendanceResponse();
        employeeAttendance = await _repository.CreateAsync(entity);
        return employeeAttendance;
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<EmployeeAttendanceResponse> Update(long id, EmployeeAttendance entity)
    {
        EmployeeAttendanceResponse employeeAttendance = new EmployeeAttendanceResponse();
        entity.Pid = id;
        employeeAttendance = await _repository.UpdateAsync(entity);
        return employeeAttendance;
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<EmployeeAttendanceResponse> Delete(long id)
    {
        EmployeeAttendanceResponse employeeAttendance = new EmployeeAttendanceResponse();
        employeeAttendance = await _repository.DeleteAsync(id);
        return employeeAttendance;
    }
}
