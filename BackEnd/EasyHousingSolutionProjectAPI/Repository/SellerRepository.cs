using EasyHousingSolutionProjectAPI.Interface;
using EasyHousingSolutionProjectAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyHousingSolutionProjectAPI.Repository
{
    public class SellerRepository : ISellerRepository
    {
        EasyHousingSolutionProjectDbContext _context;
        IPropertyRepository propertyRepository;
        IImageRepository imageRepository;
        public SellerRepository(EasyHousingSolutionProjectDbContext context, IPropertyRepository propertyRepository, IImageRepository imageRepository)
        {
            _context = context;
            this.propertyRepository = propertyRepository;
            this.imageRepository = imageRepository;
        }
        public async Task<bool> AddProperty(Property property)
        {
            return await  propertyRepository.AddProperty(property);
        }

        public async Task<bool> DeleteProperty(Property property)
        {
            return await propertyRepository.DeleteProperty(property.PropertyId);
        }

        public async Task<bool> EditProperty(int propertyId, Property property)
        {
            return await propertyRepository.UpdateProperty(propertyId,property);
        }

        public async Task<IEnumerable<Property>> GetDeactivatedPropertiesByOwner(int sellerId)
        {
            return await propertyRepository.GetUnverifiedPropertiesBySeller(sellerId);
        }

        public async Task<Property> GetPropertyDetails(int propertyId)
        {
            return await propertyRepository.GetPropety(propertyId);
        }

        public async Task<IEnumerable<Property>> GetVerifiedPropertiesByOwner(int sellerId)
        {
            return await propertyRepository.GetVerifiedPropertiesBySeller(sellerId);
        }

        public Task<bool> UploadPropertyImages(Image image)
        {
            return imageRepository.AddImage(image);
        }
        public async Task<IEnumerable<Image>> GetImagesByPropertyId(int propertyId)
        {
            return await _context.Images.Where(x => x.PropertyId == propertyId).ToListAsync();
        }

        public async Task<bool> AddSeller(Seller seller)
        {
            _context.Sellers.Add(seller);
            return await _context.SaveChangesAsync()>0;
        }

        public async Task<IEnumerable<Property>> GetAllProperties()
        {
           return await propertyRepository.GetAllProperty();
        }

        public async Task<List<Property>> GetPropertiesBySellerId(int sellerId)
        {
            return await _context.Properties.Where(x => x.SellerId == sellerId).ToListAsync();
        }

        public async Task<Seller> GetSellerByName(string sellerName)
        {
            return await _context.Sellers
                                 .FirstOrDefaultAsync(s => s.UserName == sellerName);
        }

        public async Task<IEnumerable<Seller>> GetAllSellersAsync()
        {
            return await _context.Sellers
                             .Select(s => new Seller
                             {
                                 SellerId = s.SellerId,
                                 UserName = s.UserName,
                                 FirstName = s.FirstName,
                                 LastName = s.LastName,
                                 EmailId = s.EmailId,
                                 PhoneNo = s.PhoneNo
                             })
                             .ToListAsync();
        }

    }
}
