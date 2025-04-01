namespace EasyHousingSolutionProjectAPI.Models
{
    public class UserVM
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }  // "Buyer", "Seller"

        // Buyer-specific properties
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string PhoneNo { get; set; }
        public string EmailId { get; set; }

        // Seller-specific properties
        public string Address { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
    }
}
