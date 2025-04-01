using System.Linq;
using EasyHousingSolutionProjectAPI.Interface;
using EasyHousingSolutionProjectAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace EasyHousingSolutionProjectAPI.Repository
{
    public class BuyerRepository : IBuyerRepository
    {

        public EasyHousingSolutionProjectDbContext _context;
        IPropertyRepository _propertyRepository;

        IWishlistRepository _wishlistRepository;
        public BuyerRepository(EasyHousingSolutionProjectDbContext context, IPropertyRepository propertyRepository, IWishlistRepository wishlistRepository)
        {
            _context = context;
            _propertyRepository = propertyRepository;
            _wishlistRepository = wishlistRepository;
        }
        public async Task<bool> Add(Buyer buyer)
        {
            var data = await _context.Buyers.FindAsync(buyer.BuyerId);
            if (data != null)
            {
                return false;
            }

            _context.Buyers.Add(buyer);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddToWishList(int buyerId, int PropertyId)
        {
            return await _wishlistRepository.AddToWishlist(buyerId, PropertyId);
        }

        public bool Delete(int buyerId)
        {
            var data = _context.Buyers.Find(buyerId);
            if (data != null)
            {
                return false;
            }
            _context.Buyers.Remove(data);
            _context.SaveChanges();
            return true;
        }

        public async Task<bool> DeletePropertyFromWishList(int buyerId, int PropertyId)
        {
            return await _wishlistRepository.RemoveFromWishlist(buyerId, PropertyId);
        }

        public Buyer Get(int id)
        {
            return _context.Buyers.Find(id);
        }

        public List<Buyer> GetAll()
        {
            return _context.Buyers.ToList();
        }

        public async Task<int> GetBuyerByName(string username)
        {
            var data = await _context.Buyers
        .Include(b => b.User) // Ensures the related User entity is included
        .FirstOrDefaultAsync(b => b.User.UserName == username);

            if (data != null)
            {
                return data.BuyerId;
            }
            else
            {
                return 0;
            }
        }

        public Task<Property> GetPropertyById(int propertyId)
        {
            return _propertyRepository.GetPropety(propertyId);
        }

        public  async Task<SellerContactDetails> GetSellerContactDetails(int propertyId)
        {
            try
            {
                var result = await (from p in _context.Properties
                                    join s in _context.Sellers
                                    on p.SellerId equals s.SellerId
                                    where p.PropertyId == propertyId
                                    select new SellerContactDetails
                                    {
                                        SellerName= s.FirstName +" "+s.LastName,
                                        ContactNumber = s.PhoneNo,
                                        Email = s.EmailId,
                                        PropertyName = p.PropertyName
                                    }).FirstOrDefaultAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching contact details: {ex.Message}");
            }
        }
        public async Task<List<Property>> GetWishLists(int buyerId)
        {
            return await _wishlistRepository.GetPropertiesInWishListsByBuyerId(buyerId);
        }
        public Task<IEnumerable<Property>> SearchPropertyByRegion(string Region)
        {
            return _propertyRepository.GetPropertyByRegion(Region);
        }
        public async Task<IEnumerable<Property>> SearchPropertyByType(string Type)
        {
            return await _propertyRepository.GetAllVerifiedPropertyByType(Type);
        }

        public async Task<IEnumerable<Property>> SortPropertyByPrice(decimal price)
        {
            return await _propertyRepository
                .SortPropertyByPrice(price);
        }

        public bool Update(Buyer entity)
        {
            var data = _context.Buyers.Find(entity.BuyerId);
            if (data != null)
            {
                
                data.DateOfBirth = entity.DateOfBirth;
                data.PhoneNo = entity.PhoneNo;
                data.EmailId = entity.EmailId;
                data.FirstName = entity.FirstName;
                data.LastName = entity.LastName;
                _context.SaveChanges();
                return true;
            }
            return false;

        }
    }
}
