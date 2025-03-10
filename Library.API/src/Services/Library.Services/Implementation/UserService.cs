using Library.Data.Models.Security;
using Library.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Library.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly UserManager<UserSet> _userManager;
        private readonly RoleManager<RoleSet> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public UserService(UserManager<UserSet> userManager,
                        RoleManager<RoleSet> roleManager,
                        IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserSet> FindByEmailAsync(string email) => await _userManager.FindByEmailAsync(email);

        public async Task AddUserAsync(UserSet user, string password) => await _userManager.CreateAsync(user, password);

        public async Task<bool> CheckPasswordAsync(UserSet user, string password) => await _userManager.CheckPasswordAsync(user, password);

        public async Task AddUserToRoleAsync(UserSet user, string role)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new RoleSet { Name = role });
            }
            await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<int> GetUserId()
        {
            var claimsUser = _httpContextAccessor.HttpContext?.User;
            if (claimsUser == null)
                return 0;

            var user = await _userManager.GetUserAsync(claimsUser);

            return user!.Id;
        }
    }
}
