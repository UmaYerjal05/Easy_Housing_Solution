using EasyHousingSolutionProjectAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentGatewayMicroservice.Repository;

namespace PaymentGatewayMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentRequestController : ControllerBase
    {
        private readonly IPaymentRequest _paymentRequestRepository;

        // Inject the repository via constructor
        public PaymentRequestController(IPaymentRequest paymentRequestRepository)
        {
            _paymentRequestRepository = paymentRequestRepository;
        }

        // POST api/paymentrequests
        [HttpPost]
        public async Task<ActionResult<PaymentRequest>> CreatePaymentRequest([FromBody] PaymentRequest paymentRequest)
        {
            if (paymentRequest == null)
            {
                return BadRequest("Invalid payment request data.");
            }

            var createdRequest = await _paymentRequestRepository.CreateAsync(paymentRequest);
            return CreatedAtAction(nameof(GetPaymentRequestById), new { id = createdRequest.Id }, createdRequest);
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentRequest>> GetPaymentRequestById(int id)
        {
            var paymentRequest = await _paymentRequestRepository.GetByIdAsync(id);

            if (paymentRequest == null)
            {
                return NotFound(new { message = "Payment request not found." });
            }

            return Ok(paymentRequest);
        }


        // GET api/paymentrequests
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<PaymentRequest>>> GetAllPaymentRequests()
        {
            var paymentRequests = await _paymentRequestRepository.GetAllAsync();
            return Ok(paymentRequests);
        }

        // PUT api/paymentrequests/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<PaymentRequest>> UpdatePaymentRequest(int id, [FromBody] PaymentRequest paymentRequest)
        {
            var updatedRequest = await _paymentRequestRepository.UpdateAsync(id, paymentRequest);
            if (updatedRequest == null)
            {
                return NotFound("Payment request not found.");
            }
            return Ok(updatedRequest);
        }

        // DELETE api/paymentrequests/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePaymentRequest(int id)
        {
            var success = await _paymentRequestRepository.DeleteAsync(id);
            if (!success)
            {
                return NotFound("Payment request not found.");
            }
            return NoContent();
        }
    }
}
