using EasyHousingSolutionProjectAPI.Models;

namespace EasyHousingSolutionProjectAPI.Interface
{
    public interface IImageRepository
    {
        public Task<bool> AddImage(Image image);
        //public Task<string> AddImage(Image image);
        public Task<bool> DeleteImage(int imageId);
        public Task<IEnumerable<Image>> GetImagesByPropertyId(int propertyId);
        //Task SaveAsync();
    }
}
