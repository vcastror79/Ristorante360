using Microsoft.EntityFrameworkCore;
using ElChanteAdmin.Models;
using ElChanteAdmin.Services.Contract;

namespace ElChanteAdmin.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly ElChanteContext _dbcontext;

        public UserService(ElChanteContext dbContext)
        {
            _dbcontext = dbContext;
        }
        public async Task<User> GetUser(string email, string password)
        {
            User user_found = await _dbcontext.Users.Where(u => u.Email == email && u.Password == password).FirstOrDefaultAsync();
            return user_found;
        }

        public async Task<User> SaveUser(User Model)
        {
            _dbcontext.Users.Add(Model);
            await _dbcontext.SaveChangesAsync();

            return Model;
        }
    }
}
