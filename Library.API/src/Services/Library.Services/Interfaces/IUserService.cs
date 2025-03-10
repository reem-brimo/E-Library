using Library.Data.Models.Security;

namespace Library.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserSet> FindByEmailAsync(string email);
        Task AddUserAsync(UserSet user, string password);
        Task<bool> CheckPasswordAsync(UserSet user, string password);
        Task AddUserToRoleAsync(UserSet user, string role);
        Task<int> GetUserId();
    }
}