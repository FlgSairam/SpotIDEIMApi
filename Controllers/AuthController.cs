using Microsoft.AspNetCore.Mvc;
using DapperAuthApi.Models;
using DapperAuthApi.Repositories;
using DapperAuthApi.Services;

namespace DapperAuthApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserRepository _userRepository;
    private readonly TokenService _tokenService;

    public AuthController(UserRepository userRepository, TokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    [HttpPost("FluentgridLogin")]
    public async Task<IActionResult> Login([FromBody] User login)
    {
        var user = await _userRepository.GetUserAsync(login.Username, login.Password);
        if (user == null)
            return Unauthorized("Invalid credentials");

        var token = "Bearer " + _tokenService.CreateToken(user.Username);
        return Ok(new { token });
    }
}
