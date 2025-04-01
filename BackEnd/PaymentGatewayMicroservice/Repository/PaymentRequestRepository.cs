using EasyHousingSolutionProjectAPI.Models;
using Microsoft.EntityFrameworkCore;
using PaymentGatewayMicroservice.Models;

namespace PaymentGatewayMicroservice.Repository
{
    public class PaymentRequestRepository:IPaymentRequest
    {
        private readonly PaymentRequestDbContext _context;

        public PaymentRequestRepository(PaymentRequestDbContext context)
        {
            _context=context;
        }

        public async Task<IEnumerable<PaymentRequest>> GetAllAsync()
        {
            return await _context.PaymentRequests.ToListAsync();
        }

        public async Task<PaymentRequest> GetByIdAsync(int id)
        {
            return await _context.PaymentRequests
                .FirstOrDefaultAsync(pr => pr.Id == id);
        }

        public async Task<PaymentRequest> CreateAsync(PaymentRequest paymentRequest)
        {
            paymentRequest.CreatedAt = System.DateTime.Now;
            paymentRequest.Status = "Pending"; // default status
            paymentRequest.TransactionId = System.Guid.NewGuid().ToString();

            // Add to DbContext and save changes
            _context.PaymentRequests.Add(paymentRequest);
            await _context.SaveChangesAsync();

            return paymentRequest;
        }

        public async Task<PaymentRequest> UpdateAsync(int id, PaymentRequest paymentRequest)
        {
            var existingRequest = await _context.PaymentRequests
                .FirstOrDefaultAsync(pr => pr.Id == id);
            if (existingRequest != null)
            {
                existingRequest.CardNumber = paymentRequest.CardNumber;
                existingRequest.ExpiryDate = paymentRequest.ExpiryDate;
                existingRequest.CVV = paymentRequest.CVV;
                existingRequest.Amount = paymentRequest.Amount;
                existingRequest.Status = paymentRequest.Status;
                existingRequest.TransactionId = paymentRequest.TransactionId;

                await _context.SaveChangesAsync();
                return existingRequest;
            }

            return null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var paymentRequest = await _context.PaymentRequests
                .FirstOrDefaultAsync(pr => pr.Id == id);
            if (paymentRequest != null)
            {
                _context.PaymentRequests.Remove(paymentRequest);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
