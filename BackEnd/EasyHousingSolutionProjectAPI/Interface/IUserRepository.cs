using EasyHousingSolutionProjectAPI.Models;

namespace EasyHousingSolutionProjectAPI.Interface
{
    public interface IUserRepository
    {
        Task<User> GetByUsernameAsync(string username);
        Task<bool> CreateUserAsync(User user);
    }
}
