using Microsoft.EntityFrameworkCore;
using Ristorante360Admin.Models;
using Ristorante360Admin.Services.Contract;


namespace Ristorante360Admin.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly RistoranteContext _dbcontext;

        public UserService(RistoranteContext dbContext)
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
