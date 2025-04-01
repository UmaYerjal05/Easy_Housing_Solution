using EasyHousingSolutionProjectAPI.Interface;
using EasyHousingSolutionProjectAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyHousingSolutionProjectAPI.Repository
{
    public class PropertyRepository : IPropertyRepository
    {
        EasyHousingSolutionProjectDbContext _context;
        public PropertyRepository(EasyHousingSolutionProjectDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddProperty(Property entity)
        {
            
            if (entity != null)
            {
                _context.Properties.Add(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DeactivatePropertyByAdmin(int propertyId, string reason)
        {
            var data = await _context.Properties.FindAsync(propertyId);
            if (data != null && data.IsActive==false)
            {
                return false;
            }
            data.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProperty(int propertyId)
        {
            var data = await _context.Properties.FindAsync(propertyId);
            if(data != null)
            {
                _context.Properties.Remove(data);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Property>> GetAllProperty()
        {
            return await _context.Properties.ToListAsync();
        }

        public async Task<IEnumerable<Property>> GetAllVerifiedPropertyByRegion(string region)
        {
            return await _context.Properties.Where(p => p.Address.Contains(region) && p.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Property>> GetAllVerifiedPropertyByType(string type)
        {
            return await _context.Properties.Where(p => p.PropertyType.Equals(type) && p.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Property>> GetPropertiesBySeller(int sellerId)
        {
            return await _context.Properties.Where(p => p.SellerId == sellerId).ToListAsync();
        }

        //public async Task<Property> GetProperty(int propertyId) => await _context.Properties.FirstOrDefaultAsync(GetProperty => GetProperty.PropertyId == propertyId && GetProperty.IsActive);

        public async Task<IEnumerable<Property>> GetPropertyByRegion(string region)
        {
            return await _context.Properties.Where(p => p.Address.Contains(region)).ToListAsync();
        }

        public async Task<Property> GetPropety(int propertyId)
        {
            return await _context.Properties.FindAsync(propertyId);
        }

        public async Task<IEnumerable<Property>> GetUnverifiedPropertiesBySeller(int sellerId)
        {
            var data = await _context.Properties.Where(x => x.IsActive == false && x.SellerId == sellerId).ToListAsync();
            return data;
        }

        public async Task<IEnumerable<Property>> GetVerifiedPropertiesBySeller(int sellerId)
        {
            var data = await _context.Properties.Where(x => x.IsActive == true && x.SellerId == sellerId).ToListAsync();
                    return data;
        }

        public async Task<IEnumerable<Property>> SortPropertyByPrice(decimal price)
        {
            return await _context.Properties.OrderBy(p => p.PriceRange <= price && p.IsActive).ToListAsync();
        }

        public async Task<bool> UpdateProperty(int propertyId, Property entity)
        {
            var data = _context.Properties.Find(propertyId);
            if (data != null)
            {
                data.PropertyName = entity.PropertyName;
                data.PropertyType = entity.PropertyType;
                data.PropertyOption = entity.PropertyOption;
                data.Description = entity.Description;
                data.Address = entity.Address;
                data.PriceRange = entity.PriceRange;
                data.InitialDeposit = entity.InitialDeposit;
                data.Landmark = entity.Landmark;
                data.IsActive = entity.IsActive;
                data.SellerId = entity.SellerId;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> VerifyPropertyByAdmin(int propertyId)
        {
            var data = await _context.Properties.FindAsync(propertyId);
            if (data == null || data.IsActive == true)
            {
                return false;
            }
            data.IsActive = true;
            _context.SaveChanges();
            return true;
        }

    }
}
