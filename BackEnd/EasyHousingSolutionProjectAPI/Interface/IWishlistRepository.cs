using EasyHousingSolutionProjectAPI.Models;

namespace EasyHousingSolutionProjectAPI.Interface
{
    public interface IWishlistRepository
    {
        Task<bool> AddToWishlist(int buyerId,int propertyId);
        Task<List<Property>> GetPropertiesInWishListsByBuyerId(int buyerId);

        Task<bool> RemoveFromWishlist(int buyerId,int PropertyId);

    }
}
