using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TestWebApplication.Models;
using TestWebApplication.Repository.Investor;

namespace TestWebApplication.Controllers
{
    public class FormController : Controller
    {
        private readonly IInvestorRepository _investorRepo;
        private readonly HttpClient _httpClient;
        private readonly ExternalApiSettings _externalApiSettings;
        public FormController(IInvestorRepository investorRepo, IHttpClientFactory httpClientFactory,
    IOptions<ExternalApiSettings> externalApiOptions)
        {
            _investorRepo = investorRepo;
            _httpClient = httpClientFactory.CreateClient();
            _externalApiSettings = externalApiOptions.Value;

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



        [HttpPost]
        public async Task<IActionResult> GetEvalueteRules([FromBody] FormDetails obj )
        {
            if (obj == null)
                return BadRequest("Invalid request data");
            // Optional validation (example)
            if (string.IsNullOrEmpty(obj.FormId))
                return BadRequest("FormId is required");

            var url = _externalApiSettings.WorkflowCallApi;
            obj.Context ??= new Dictionary<string, object?>();

            // Send the same object to external API
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(obj),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync(url, jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode,
                    "Error calling Workflow API");
            }
            var result = await response.Content.ReadAsStringAsync();

            // Convert string JSON to actual JSON object
            var jsonObject = JsonSerializer.Deserialize<object>(result);

            return Ok(jsonObject);
        }
    }
}
