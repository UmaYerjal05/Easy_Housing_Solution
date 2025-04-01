using EasyHousingSolutionProjectAPI.Models;

namespace EasyHousingSolutionProjectAPI.Interface
{
    public interface IStateRepository
    {
        public Task<List<State>> GetStates();
    }
}
