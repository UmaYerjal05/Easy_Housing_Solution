using EasyHousingSolutionProjectAPI.Models;

namespace EasyHousingSolutionProjectAPI.Interface
{
    public interface ICityRepository
    {
        public Task<List<City>> GetCities(int stateId);
    }
}
