using EasyHousingSolutionProjectAPI.Interface;
using EasyHousingSolutionProjectAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyHousingSolutionProjectAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        EasyHousingSolutionProjectDbContext _context;
        public UserRepository(EasyHousingSolutionProjectDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            return await _context.SaveChangesAsync()>0;
        }

        public Task<User> GetByUsernameAsync(string username)
        {
            return _context.Users.FirstOrDefaultAsync(x=>x.UserName == username);

        }
    }
}
