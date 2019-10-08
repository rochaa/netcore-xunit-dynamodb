using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using gidu.Domain.Core.Users;
using gidu.Domain.Helpers;
using AutoMapper;

namespace gidu.WebAPI.Controllers
{
    [Route("v1/logon")]
    [ApiController]
    public class LogonController : ControllerBase
    {
        private readonly UserAuthentication _userAuthentication;
        private readonly IUserRepository _userRepository;

        public LogonController(UserAuthentication userAuthentication, IUserRepository userRepository)
        {
            _userAuthentication = userAuthentication;
            _userRepository = userRepository;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            var user = await _userAuthentication.AuthenticateAsync(userLoginDto);
            var userLogged = new UserLoggedDto
            {
                Token = GenerateJWT(user),
                Refresh_Token = user.RefreshToken,
                User = Mapper.Map<UserDto>(user)
            };

            return Ok(userLogged);
        }

        private string GenerateJWT(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII
                .GetBytes(Environment.GetEnvironmentVariable("Key_Token")));

            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var rights = _userAuthentication.CreateRights(user);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(rights),
                Expires = DateTime.Now.ToUniversalTime().AddMinutes(GlobalParameters.TokenValidityInMinutes),
                SigningCredentials = credential
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
