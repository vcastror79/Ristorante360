using Microsoft.EntityFrameworkCore;
using Ristorante360Admin.Models;

namespace Ristorante360Admin.Services.Contract
{
    public interface IUserService
    {
        Task<User> GetUser(string email, string password);
        Task<User> SaveUser(User Model);
    }
}
