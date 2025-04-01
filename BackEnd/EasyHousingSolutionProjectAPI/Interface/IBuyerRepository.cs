using EasyHousingSolutionProjectAPI.Models;

namespace EasyHousingSolutionProjectAPI.Interface
{
    public interface IBuyerRepository : IRepo<Buyer>
    {
        Task<IEnumerable<Property>> SearchPropertyByRegion(string Region);
        Task<List<Property>> GetWishLists(int buyerId);
        Task<IEnumerable<Property>> SortPropertyByPrice(decimal price);
        Task<bool> AddToWishList(int buyerId, int PropertyId);
        public Task<SellerContactDetails> GetSellerContactDetails(int propertyId);

        //List<WishList> GetAllWishList();
        Task<IEnumerable<Property>> SearchPropertyByType(string Type);

        Task<bool> DeletePropertyFromWishList(int buyerId, int PropertyId);

        Task<Property> GetPropertyById(int propertyId);

        Task<int> GetBuyerByName(string username);

    }
}
