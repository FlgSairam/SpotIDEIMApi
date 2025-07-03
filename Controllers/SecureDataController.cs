using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DapperAuthApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SecureDataController : ControllerBase
{
    [HttpGet]
    public IActionResult GetSecureData()
    {
        return Ok("You have accessed protected data!");
    }
}
