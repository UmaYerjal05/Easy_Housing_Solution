using EasyHousingSolutionProjectAPI.Models;

namespace EasyHousingSolutionProjectAPI.Interface
{
    public interface IPropertyRepository
    {

        // Get the verified and unverified properties of the seller
        // admin Functionality
        Task<IEnumerable<Property>>GetPropertiesBySeller(int sellerId);

        // admin home page
        //admin Functionality
        public Task<IEnumerable<Property>> GetAllProperty();


        //get the properties by region for buyer and admin 
        // admin functionality
        Task<IEnumerable<Property>> GetPropertyByRegion(string region);

        // Admin deactivate the property of Seller with reason
        //admin functionality
        Task<bool> DeactivatePropertyByAdmin(int propertyId, string reason);

        // Admin Activate the property of seller
        //admin functionality
        Task<bool> VerifyPropertyByAdmin(int propertyId);



        // Seller Properties 
        // Add,Edit Property - generic
        public Task<bool> AddProperty(Property entity);
        public Task<bool> UpdateProperty(int propertyId,Property entity);

        //public Property Get(int id);
        Task<Property> GetPropety(int propertyId);
        Task<bool> DeleteProperty(int propertyId);

        // Get only verified Properties by the seller
        //seller functionality
        Task<IEnumerable<Property>> GetVerifiedPropertiesBySeller(int sellerId);
        // Get only unverified Properties by the seller
        // seller functionality
        Task<IEnumerable<Property>> GetUnverifiedPropertiesBySeller(int sellerId);



        // Buyer Properties 
        // View Verified Property by buyer
        //Get the all verified property by resign 
        // buyer functionality
        Task<IEnumerable<Property>> GetAllVerifiedPropertyByRegion(string region);

        //Get the all the verified property by type
        // buyer functionality
        Task<IEnumerable<Property>> GetAllVerifiedPropertyByType(string type);

        // Buyer 
        Task<IEnumerable<Property>> SortPropertyByPrice(decimal price);


        

        
    }
}
