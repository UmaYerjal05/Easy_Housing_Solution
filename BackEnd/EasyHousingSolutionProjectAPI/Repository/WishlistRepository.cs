using System.ComponentModel;
using EasyHousingSolutionProjectAPI.Interface;
using EasyHousingSolutionProjectAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyHousingSolutionProjectAPI.Repository
{
    public class WishlistRepository : IWishlistRepository
    {
        public EasyHousingSolutionProjectDbContext _context;
        public WishlistRepository(EasyHousingSolutionProjectDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddToWishlist(int buyerId, int propertyId)
        {
            var existingWishListItem=await _context.WishLists.FirstOrDefaultAsync(c=> c.BuyerId==buyerId && c.PropertyId==propertyId);
            if (existingWishListItem != null)
            {
                throw new Exception("Property is already in the wishlist");
            }
            var wishlist = new WishList
            {
                BuyerId = buyerId,
                PropertyId = propertyId
            };
            await _context.WishLists.AddAsync(wishlist);
            _context.SaveChanges();
            return true;
        }

        public async Task<List<Property>> GetPropertiesInWishListsByBuyerId(int buyerId)
        {
            var propertiesInWishlist = await _context.WishLists
           .Where(w => w.BuyerId == buyerId)
           .Join(_context.Properties,
               wishList => wishList.PropertyId,
               property => property.PropertyId,
               (wishList, property) => property)
           .ToListAsync();

            return propertiesInWishlist;

        }

        public async Task<bool> RemoveFromWishlist(int buyerId, int PropertyId)
        {
            var data=await _context.WishLists.FirstOrDefaultAsync(c=>c.BuyerId==buyerId && c.PropertyId== PropertyId);
            if(data != null)
            {
                _context.WishLists.Remove(data);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
