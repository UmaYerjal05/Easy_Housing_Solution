namespace EasyHousingSolutionProjectAPI.Models
{
    public class PaymentRequest
    {
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string CVV { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
        public string TransactionId { get; set; }
    }
}
