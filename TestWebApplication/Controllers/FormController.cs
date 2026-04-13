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
        public async Task<IActionResult> CreateInvestor([FromBody] InvestorRequestModel request)
        {
            if (request == null || request.Investor == null)
                return Json(new { success = false, message = "Invalid data" });

            if (string.IsNullOrWhiteSpace(request.FormId))
                return Json(new { success = false, message = "FormId is required" });

            var obj = request.Investor;
            obj.IsActive = true;

            // Insert investor and get generated InvestorId
            var investorId = await _investorRepo.InsertInvestor(obj);

            if (investorId <= 0)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Failed to register investor"
                });
            }

            // Prepare FormDetails for Workflow API
            var formDetails = new FormDetails
            {
                ReferenceKey = investorId.ToString(),
                FormId = request.FormId, // Dynamic FormId from popup
                UsedTypes = null,
                Context = new Dictionary<string, object?>()
            };

            var url = _externalApiSettings.WorkflowCallApi;

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(formDetails),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync(url, jsonContent);

            object? workflowResponse = null;
            string? errorMessage = null;

            if (response.IsSuccessStatusCode)
            {
                var resultContent = await response.Content.ReadAsStringAsync();
                workflowResponse = JsonSerializer.Deserialize<object>(
                    resultContent,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            else
            {
                errorMessage = await response.Content.ReadAsStringAsync();
            }

            return Json(new
            {
                success = true,
                message = "Investor registered successfully",
                investorId = investorId,
                workflowTriggered = response.IsSuccessStatusCode,
                workflowResponse = workflowResponse,
                workflowError = errorMessage
            });
        }


        
    }
}
