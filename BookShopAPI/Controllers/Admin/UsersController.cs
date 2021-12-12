using BookShopAPI.Helpers;
using BookShopAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/admin/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ShopOnlineAPIContext _context;
        public UsersController(ShopOnlineAPIContext context)
        {
            _context = context;
        }
        // GET: api/<UsersController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> Get()
        {
            return await _context.Users.ToListAsync();
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> Get(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        // POST api/<UsersController>
        [HttpPost("add")]
        public async Task<ActionResult<Users>> Post(Users o)
        {
            Users user = _context.Users.Where(us => us.Email == o.Email).FirstOrDefault();
            if (user != null)
            {
                return BadRequest("Email have been existed");
            }

            o.RoleId = o.RoleId == 2 ? 2 : 1;
            o.Password = Utilities.HashMD5(o.Password);
            o.Active = true;

            _context.Users.Add(o);
            await _context.SaveChangesAsync();

            return StatusCode(201, "Add user success");
        }

        // PUT api/<UsersController>/5
        [HttpPut("update/{id}")]
        public async Task<ActionResult<Users>> Put(int id, Users user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }
            Users a = await _context.Users.FindAsync(id);
            if (a == null)
            {
                return NotFound();
            }
     

            a.Username = user.Username;
            a.Password = Utilities.HashMD5(user.Password);
            a.Email = user.Email;
            a.RoleId = user.RoleId;

            _context.Users.Update(a);
            await _context.SaveChangesAsync();

            return StatusCode(200, "Update user success");

        }

        // DELETE api/<UsersController>/5
        [HttpDelete("disable/{id}")]
        public async Task<ActionResult<Users>> Delete(int id)
        {
            Users a = await _context.Users.FindAsync(id);
            if (a == null)
            {
                return NotFound();
            }

            a.Active = false;

            _context.Users.Update(a);
            await _context.SaveChangesAsync();

            return StatusCode(200, "Disable user success");
        }
    }
}
