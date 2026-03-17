using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TestWebApplication.Models;
using TestWebApplication.Repository.Investor;

namespace TestWebApplication.Controllers
{
    public class FormController : Controller
    {
        private readonly IInvestorRepository _investorRepo;

        public FormController(IInvestorRepository investorRepo)
        {
            _investorRepo = investorRepo;
        }

        public async Task<IActionResult> InvesterForm()
        {
            var investors = await _investorRepo.GetAllInvestors();
            return View(investors);
        }

        [HttpPost]
        public async Task<IActionResult> CreateInvestor([FromBody] InvestorModel obj)
        {
            if (obj == null)
                return BadRequest(new { success = false, message = "Invalid data" });

            obj.IsActive = true;
            var result = await _investorRepo.InsertInvestor(obj);

            if (result == 1)
            {
                return Ok(new
                {
                    success = true,
                    message = "Investor registered successfully"
                });
            }

            return StatusCode(500, new
            {
                success = false,
                message = "Failed to register investor"
            });
        }
    }
}
