using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DapperAuthApi.Repositories;
using DapperAuthApi.Models;
using DapperAuthApi.Services;

namespace DapperAuthApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly EmployeeRepository _repository;

    private readonly TokenService _tokenService;


    public EmployeeController(EmployeeRepository repository, TokenService tokenService)
    {
        _repository = repository;
        _tokenService = tokenService;
    }

    [HttpGet("EmployeeLoginInfo/{mobileNo}")]
    [Authorize]
    public async Task<IActionResult> GetEmployeeLoginInfo(string mobileNo)
    {
        var employee = await _repository.GetEmployeeLoginInfoAsync(mobileNo);
        if (employee == null)
            return NotFound("Employee details not found");

        return Ok(employee);
    } 
}
