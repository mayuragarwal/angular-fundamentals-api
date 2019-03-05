using BCrypt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SampleWebApi
{
    public class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Dictionary<string, ApplicationUser> _applicationUsers;
        private readonly string salt;

        public AuthService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            salt = BCryptHelper.GenerateSalt();

            var applicationUser = new ApplicationUser()
            {
                Id = 1,
                Email = "a@b.com",
                UserName = "mayur",
                FirstName = "mayur",
                LastName = "agarwal",
                PasswordHash = BCryptHelper.HashPassword("password", salt)
            };

            var testApplicationUser = new ApplicationUser()
            {
                Id = 2,
                Email = "test@b.com",
                UserName = "test",
                FirstName = "test",
                LastName = "test",
                PasswordHash = BCryptHelper.HashPassword("test", salt)
            };

            _applicationUsers = new Dictionary<string, ApplicationUser>()
            {
                { "mayur", applicationUser },
                { "test", testApplicationUser }
            };
        }

        public async Task<AuthenticatedUser> SignInUser(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await _httpContextAccessor.HttpContext.SignInAsync(principal);

            return new AuthenticatedUser
            {
                FirstName = user.FirstName,
                Id = user.Id,
                LastName = user.LastName,
                UserName = user.UserName
            };
        }

        public async Task SignOutUser()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public Task<bool> ValidateCredentials(UserModel userModel, out ApplicationUser user)
        {
            user = null;

            if (_applicationUsers.TryGetValue(userModel.UserName, out var applicationUser)
                && applicationUser.PasswordHash == BCryptHelper.HashPassword(userModel.Password, salt))
            {
                user = applicationUser;
            }

            return Task.FromResult(user != null);
        }
    }
}
