using Microsoft.AspNetCore.Mvc;
using TestWebApplication.Models;
using TestWebApplication.Repository.UserMaster;

namespace TestWebApplication.Controllers
{
    public class UserMasterController : Controller
    {
        private readonly IUserMaster _repo;
        public UserMasterController(IUserMaster repo)
        {
            _repo = repo;
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginUser([FromBody] Login obj)
        {
            if (obj == null)
                return BadRequest(new { success = false, message = "Invalid data" });

            var user = await _repo.Login(obj);
            if (user != null)
            {
                // In a real scenario, you'd set a session or token here.
                return Ok(new { success = true, message = "Login successful", redirectUrl = "/Form/InvesterForm" });
            }
            
            return Unauthorized(new { success = false, message = "Invalid username or password" });
        }



        //Login
        public IActionResult UserRegistration()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserMasterEntity obj)
        {
            if (obj == null)
                return BadRequest(new { success = false, message = "Invalid data" });

            
            obj.IsActive=true;
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
