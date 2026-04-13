using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using TestApplication.API.Models;
using TestApplication.API.Repository.Investor;
using System.Net.Http;

namespace TestApplication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormController : ControllerBase
    {
        private readonly IInvestorRepository _investorRepo;
        private readonly HttpClient _httpClient;
        private readonly ExternalApiSettings _externalApiSettings;

        public FormController(IInvestorRepository investorRepo, IHttpClientFactory httpClientFactory, IOptions<ExternalApiSettings> externalApiOptions)
        {
            _investorRepo = investorRepo;
            _httpClient = httpClientFactory.CreateClient();
            _externalApiSettings = externalApiOptions.Value;
        }

        [HttpGet("GetAllInvestors")]
        public async Task<IActionResult> GetAllInvestors()
        {
            var investors = await _investorRepo.GetAllInvestors();
            return Ok(investors);
        }

        [HttpPost("CreateInvestor")]
        public async Task<IActionResult> CreateInvestor([FromBody] InvestorModel obj)
        {
            if (obj == null)
                return BadRequest(new { success = false, message = "Invalid data" });

            obj.IsActive = true;
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
                ReferenceKey = investorId.ToString(), // From SCOPE_IDENTITY()
                FormId = "FORM202604783", // Static value
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

            if (response.IsSuccessStatusCode)
            {
                var resultContent = await response.Content.ReadAsStringAsync();
                workflowResponse = JsonSerializer.Deserialize<object>(resultContent);
            }
            // Return combined response
            return Ok(new
            {
                success = true,
                message = "Investor registered successfully",
                investorId = investorId,
                workflowTriggered = response.IsSuccessStatusCode,
                workflowResponse = workflowResponse
            });
        }

        //[HttpPost("GetEvalueteRules")]
        //public async Task<IActionResult> GetEvalueteRules([FromBody] FormDetails obj )
        //{
        //    if (obj == null)
        //        return BadRequest("Invalid request data");
            
        //    if (string.IsNullOrEmpty(obj.FormId))
        //        return BadRequest("FormId is required");

        //    var url = _externalApiSettings.WorkflowCallApi;
        //    obj.Context ??= new Dictionary<string, object?>();

        //    var jsonContent = new StringContent(
        //        JsonSerializer.Serialize(obj),
        //        Encoding.UTF8,
        //        "application/json");

        //    var response = await _httpClient.PostAsync(url, jsonContent);

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        return StatusCode((int)response.StatusCode,
        //            "Error calling Workflow API");
        //    }
        //    var result = await response.Content.ReadAsStringAsync();
        //    var jsonObject = JsonSerializer.Deserialize<object>(result);

        //    return Ok(jsonObject);
        //}
    }
}
