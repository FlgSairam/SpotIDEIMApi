using DapperAuthApi.Models;
using DapperAuthApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DapperAuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUserController : ControllerBase
    {
        private readonly AppUserRepository _userRepository; // Correct type used here

        public AppUserController(AppUserRepository userRepository) // Correct type used here
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userRepository.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AppUser user) // Correct type used here
        {
            user.Created_By = "system"; // or get from token context
            var result = await _userRepository.CreateAsync(user);
            return result > 0 ? Ok("User created successfully") : BadRequest("Creation failed");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, AppUser user) // Correct type used here
        {
            user.Pid = id;
            user.Updated_By = "system"; // or get from token context
            var result = await _userRepository.UpdateAsync(user);
            return result > 0 ? Ok("User updated successfully") : NotFound("User not found");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _userRepository.DeleteAsync(id);
            return result > 0 ? Ok("User deleted (soft) successfully") : NotFound("User not found");
        }
    }
}
