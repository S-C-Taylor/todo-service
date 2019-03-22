using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Todo.API.Models;
using Todo.API.Repository;

namespace Todo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        [ProducesResponseType(200, Type = typeof(JwtSecurityTokenHandler))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Login([FromBody] UserDto userParam) {
            var user = _userRepository.Login(userParam.Username, userParam.Password);
    
            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });
    
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySecretKey010203"));
            
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                    var tokenOptions = new JwtSecurityToken(
                claims: new List<Claim> {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, "user")
                },
                expires: DateTime.Now.AddDays(2),
                signingCredentials: signinCredentials
            );
    
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return Ok(new { Token = tokenString });
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] UserDto userParam) {
            var user = _userRepository.Create(userParam);

            if (user == null) {
                return Conflict($"Username {userParam.Username} is already in use.");
            }

            return await Login(userParam);
        }
    }
}
