using EasyHousingSolutionProjectAPI.Interface;
using EasyHousingSolutionProjectAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyHousingSolutionProjectAPI.Repository
{
    public class StateRepository : IStateRepository
    {
        EasyHousingSolutionProjectDbContext EasyHousingSolutionProjectDbContext;
        public StateRepository(EasyHousingSolutionProjectDbContext easyHousingSolutionProjectDbContext)
        {
            EasyHousingSolutionProjectDbContext = easyHousingSolutionProjectDbContext;
        }
        public async Task<List<State>> GetStates()
        {
            return await EasyHousingSolutionProjectDbContext.States.ToListAsync();
        }
    }
}
