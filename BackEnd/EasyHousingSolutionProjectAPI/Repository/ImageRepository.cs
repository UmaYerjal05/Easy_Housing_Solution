using EasyHousingSolutionProjectAPI.Interface;
using EasyHousingSolutionProjectAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;

namespace EasyHousingSolutionProjectAPI.Repository
{
    public class ImageRepository : IImageRepository
    {
        
        private readonly EasyHousingSolutionProjectDbContext _context;
        IFormFileCollection files;

        public ImageRepository(EasyHousingSolutionProjectDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddImage(Image image)
        {
            try
            {
                _context.Images.Add(image);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


        public async Task<bool> DeleteImage(int imageId)
        {
            var data = await _context.Images.FirstOrDefaultAsync(x => x.ImageId == imageId);
            if (data != null)
            {
                _context.Images.Remove(data);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Image>> GetImagesByPropertyId(int propertyId)
        {
            return await _context.Images.Where(x => x.PropertyId == propertyId).ToListAsync();
        }

        
    }
}
