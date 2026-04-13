using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TestApplication.API.Models;
using TestApplication.API.Repository.UserMaster;

namespace TestApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserMasterController : ControllerBase
    {
        private readonly IUserMaster _repo;

        public UserMasterController(IUserMaster repo)
        {
            _repo = repo;
        }

        [HttpPost("LoginUser")]
        public async Task<IActionResult> LoginUser([FromBody] Login obj)
        {
            if (obj == null)
                return BadRequest(new { success = false, message = "Invalid data" });

            var user = await _repo.Login(obj);
            if (user != null)
            {
                return Ok(new { success = true, message = "Login successful", user });
            }
            
            return Unauthorized(new { success = false, message = "Invalid username or password" });
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserMasterEntity obj)
        {
            if (obj == null)
                return BadRequest(new { success = false, message = "Invalid data" });
            
            obj.IsActive = true;
            var result = await _repo.InsertUser(obj);

            if (result == 1)
            {
                return Ok(new
                {
                    success = true,
                    message = "User created successfully"
                });
            }

            return StatusCode(500, new
            {
                success = false,
                message = "Failed to create user"
            });
        }
    }
}
