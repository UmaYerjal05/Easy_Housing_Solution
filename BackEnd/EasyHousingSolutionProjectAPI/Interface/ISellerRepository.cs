using EasyHousingSolutionProjectAPI.Models;

namespace EasyHousingSolutionProjectAPI.Interface
{
    public interface ISellerRepository
    {
        Task<bool> AddProperty(Property property);

        Task<bool> AddSeller(Seller seller);

        Task<List<Property>> GetPropertiesBySellerId(int sellerId);

        Task<Seller> GetSellerByName(string sellerName);

        //Task<int> GetSellerByUsername(string username);

        Task<Property> GetPropertyDetails(int propertyId);
        Task<bool> EditProperty(int propertyId,Property property);
        Task<IEnumerable<Property>> GetVerifiedPropertiesByOwner(int sellerId);

        Task<IEnumerable<Property>> GetDeactivatedPropertiesByOwner(int sellerId);

        // 
        Task<bool> DeleteProperty(Property property);

        Task<bool> UploadPropertyImages(Image image);
        public Task<IEnumerable<Image>> GetImagesByPropertyId(int propertyId);

        Task<IEnumerable<Property>> GetAllProperties();

        Task<IEnumerable<Seller>> GetAllSellersAsync();
    }
}
