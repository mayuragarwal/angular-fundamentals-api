using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace SampleWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("token")]
        public ActionResult GetToken()
        {
            // secret security key
            var securityKey = "this is our super long security key for token validation https://www.youtube.com/watch?v=7tgLuJ__ZKU";

            // symmetric security key
            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

            // signing credentials
            var signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature);

            // create token
            var token = new JwtSecurityToken(
                issuer: "eventsApi",
                audience: "participants",
                expires: DateTime.Now.AddHours(1),
                signingCredentials: signingCredentials
                );

            //return token
            var strToken = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(strToken);
        }

        [HttpPost("signin")]
        public async Task<ActionResult<AuthenticatedUser>> SignIn([FromBody] UserModel userModel)
        {
            if (await _authService.ValidateCredentials(userModel, out ApplicationUser user))
            {
                return await _authService.SignInUser(user);
            }

            return new UnauthorizedResult();
        }

        [HttpPost("signout")]
        public async Task SignOut()
        {
            await _authService.SignOutUser();
        }
    }
}