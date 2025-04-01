using EasyHousingSolutionProjectAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyHousingSolutionProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<PaymentController> _logger;
        private readonly IConfiguration _configuration;

        public PaymentController(IHttpClientFactory httpClientFactory,IConfiguration configuration,ILogger<PaymentController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentRequestById(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var paymentApiUrl = _configuration["ServiceUrls:PaymentAPI"];

            if (string.IsNullOrEmpty(paymentApiUrl))
            {
                _logger.LogError("Payment API URL is missing in configuration.");
                return StatusCode(500, "Payment API URL is not configured.");
            }

            var apiUrl = $"{paymentApiUrl}{id}";
            _logger.LogInformation("Calling Payment API: {Url}", apiUrl);

            try
            {
                var response = await client.GetAsync(apiUrl);

                _logger.LogInformation("Response Status Code: {StatusCode}", response.StatusCode);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return Ok(content);
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("API Error: {StatusCode}, Response: {Error}", response.StatusCode, errorContent);

                return StatusCode((int)response.StatusCode, errorContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while calling Payment API.");
                return StatusCode(500, "An internal server error occurred.");
            }
        }


    }
}
