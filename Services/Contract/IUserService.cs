using Microsoft.EntityFrameworkCore;
using Ristorante360Context.Models;

namespace Ristorante360Context.Services.Contract
{
    public interface IUserService
    {
        Task<User> GetUser(string email, string password);
        Task<User> SaveUser(User Model);
    }
}
