using EasyHousingSolutionProjectAPI.Models;

namespace PaymentGatewayMicroservice.Repository
{
    public interface IPaymentRequest
    {
        Task<IEnumerable<PaymentRequest>> GetAllAsync();
        Task<PaymentRequest> GetByIdAsync(int id);
        Task<PaymentRequest> CreateAsync(PaymentRequest paymentRequest);
        Task<PaymentRequest> UpdateAsync(int id, PaymentRequest paymentRequest);
        Task<bool> DeleteAsync(int id);
    }
}
