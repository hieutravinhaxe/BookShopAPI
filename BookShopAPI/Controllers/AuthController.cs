using BookShopAPI.Const;
using BookShopAPI.Helpers;
using BookShopAPI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BookShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public IConfiguration _config;
        public ShopOnlineAPIContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthController(IConfiguration config,
            ShopOnlineAPIContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            this._config = config;
            this._context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        
        [HttpPost("login")]
        public IActionResult CheckLogin(LoginModel u)
        {
            Users user = _context.Users.Where(us => us.Email == u.Email).FirstOrDefault();
            if(user == null || !user.Password.Equals(Utilities.HashMD5(u.Password)))
            {
                return BadRequest("Invalid email or password");
            }
            else if (user.Active == false)
            {
                return NotFound("Account not exist or have been disable");
            }
            else
            {
                var roleUser = _context.Roles.Find(user.RoleId);
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub,_config["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim(ClaimTypes.Role, roleUser.RoleName),
                    new Claim(ClaimTypes.Name, user.UserId.ToString())
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], claims,
                    expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);
                return Ok(new JwtSecurityTokenHandler().WriteToken(token));
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<Users>> Post(Users u)
        {
            Users user = _context.Users.Where(us => us.Email == u.Email).FirstOrDefault();
            if (user != null)
            {
                return BadRequest("Email have been existed");
            }
            u.Password = Utilities.HashMD5(u.Password);
            u.RoleId = RolesConst.CustomerRole;
            u.Active = true;
            _context.Users.Add(u);
            await _context.SaveChangesAsync();

            return StatusCode(201, "register success");
        }

        [Authorize]
        [HttpPost("profile/{id}")]
        public async Task<ActionResult<Users>> PostProfile(int id,Users u)
        {
            Request.Headers.TryGetValue("oldPwd", out var oldPwd);

            if (id != u.UserId)
            {
                return BadRequest();
            }
            
            Users a = await _context.Users.FindAsync(id);
            if (a == null)
            {
                return NotFound();
            }
            if (!a.Password.Equals(Utilities.HashMD5(oldPwd)))
            {
                return BadRequest("Invalid Password");
            }
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            int Id = int.Parse(userId);

            if (a.UserId != Id)
            {
                return NotFound();
            }
            
            if(a.Email != u.Email)
            {
                Users uEmail = _context.Users.Where(u => u.Email == u.Email).FirstOrDefault();
                if (uEmail != null)
                {
                    return BadRequest("Email have been exist");
                }
            }

            a.Username = u.Username;
            a.Password = Utilities.HashMD5(u.Password);
            a.Email = u.Email;
            a.RoleId = u.RoleId;

            _context.Users.Update(a);
            await _context.SaveChangesAsync();

            return StatusCode(200, "Update user success");

        }
    }
}
