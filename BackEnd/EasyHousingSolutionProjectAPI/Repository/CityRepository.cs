using EasyHousingSolutionProjectAPI.Interface;
using EasyHousingSolutionProjectAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyHousingSolutionProjectAPI.Repository
{
    public class CityRepository : ICityRepository
    {
        EasyHousingSolutionProjectDbContext context;

        public CityRepository(EasyHousingSolutionProjectDbContext context)
        {
            this.context = context;
            
        }
        public async Task<List<City>> GetCities(int stateId)
        {
            return await context.Cities.Where(x => x.StateId == stateId).ToListAsync();
        }
    }
}
