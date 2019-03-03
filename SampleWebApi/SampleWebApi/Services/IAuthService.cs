using System.Threading.Tasks;

namespace SampleWebApi
{
    public interface IAuthService
    {
        Task<bool> ValidateCredentials(UserModel userModel, out ApplicationUser user);
        Task<AuthenticatedUser> SignInUser(ApplicationUser user);
        Task SignOutUser();
    }

    public class AuthenticatedUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
    }

    public class UserModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}