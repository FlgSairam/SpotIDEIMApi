// Controller
using Microsoft.AspNetCore.Mvc;
using DapperAuthApi.Models;
using DapperAuthApi.Repositories;

namespace DapperAuthApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserCustomerMappingController : ControllerBase
    {
        private readonly UserCustomerMappingRepository _repository;

        public UserCustomerMappingController(UserCustomerMappingRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetMappedCustomers(long userId)
        {
            var customers = await _repository.GetMappedCustomersAsync(userId);
            return Ok(customers);
        }

        [HttpPost]
        public async Task<IActionResult> SaveMapping([FromBody] UserCustomerMapRequest request)
        {
            await _repository.SaveUserCustomerMappingAsync(request.UserId, request.CustomerIds, request.ModifiedBy);
            return Ok(new { message = "Mappings saved successfully" });
        }
    }
}
