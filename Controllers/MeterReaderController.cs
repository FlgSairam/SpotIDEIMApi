using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DapperAuthApi.Repositories;
using DapperAuthApi.Models;
using DapperAuthApi.Services;

namespace DapperAuthApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MeterReaderController : ControllerBase
{
    private readonly UserRepository _repository;

    private readonly TokenService _tokenService;


    public MeterReaderController(UserRepository repository, TokenService tokenService)
    {
        _repository = repository;
        _tokenService = tokenService;
    }

    [HttpGet("FluentgridInfo/{accessId}")]
    [Authorize]
    public async Task<IActionResult> GetMeterreaderInfo(string accessId)
    {
        var employee = await _repository.GetEmployeeInfoAsync(accessId);
        if (employee == null)
            return NotFound("Employee not found");

        return Ok(employee);
    } 
    
    
}
