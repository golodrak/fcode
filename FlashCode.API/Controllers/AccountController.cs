using FlashCode.API.Models;
using FlashCode.API.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;

namespace FlashCode.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    expires: DateTime.UtcNow.AddHours(1),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
                        SecurityAlgorithms.HmacSha256)
                );

                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
            }
            return Unauthorized();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            var user = new ApplicationUser { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("mobile/register")]
        public async Task<IActionResult> RegisterMobile([FromBody] MobileRegisterDto model)
        {
            ApplicationUser? user = null;
            if (!string.IsNullOrWhiteSpace(model.Token))
            {
                user = _userManager.Users.FirstOrDefault(u => u.MobileToken == model.Token);
                if (user == null)
                    return BadRequest("Invalid token");
            }
            else
            {
                user = _userManager.Users.FirstOrDefault(u => u.Email == model.Email);
                if (user != null)
                    return BadRequest("User already exists");
                user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    MobileToken = Guid.NewGuid().ToString()
                };
                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                    return BadRequest("Unable to create user");
            }

            user.FirstName = model.FirstName;
            user.Company = model.Company;
            user.AcceptContact = model.AcceptContact;
            user.Email = model.Email;
            await _userManager.UpdateAsync(user);

            return Ok(new TokenResult { Token = user.MobileToken });
        }

        [HttpGet("mobile/profile")]
        public IActionResult GetMobileProfile([FromQuery] string token)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.MobileToken == token);
            if (user == null)
                return NotFound();

            return Ok(new MobileProfileDto
            {
                FirstName = user.FirstName ?? string.Empty,
                Company = user.Company ?? string.Empty,
                Email = user.Email ?? string.Empty,
                AcceptContact = user.AcceptContact
            });
        }
    }
}
