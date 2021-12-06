using BookShopAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopAPI.Controllers.Admin
{
    [Authorize(Roles ="Admin")]
    [Route("api/admin/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly ShopOnlineAPIContext _context;
        public AuthorsController(ShopOnlineAPIContext context)
        {
            _context = context;
        }
        // GET: api/<AuthorsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Authors>>> Get()
        {
            return await _context.Authors.ToListAsync();
        }

        // GET api/<AuthorsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Authors>> Get(int id)
        {
            return await _context.Authors.FindAsync(id);
        }

        // POST api/<AuthorsController>
        [HttpPost("add")]
        public async Task<ActionResult<Authors>> Post(Authors author)
        {
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return StatusCode(201, "Add cate success");
        }

        // PUT api/<AuthorsController>/5
        [HttpPut("update/{id}")]
        public async Task<ActionResult<Authors>> Put(int id, Authors author)
        {
            if (id != author.AuthorId)
            {
                return BadRequest();
            }
            Authors a = await _context.Authors.FindAsync(id);
            if (a == null)
            {
                return NotFound();
            }

            a.AuthorName = author.AuthorName;
            a.AuthorBio = author.AuthorBio;
            a.Active = author.Active;

            _context.Authors.Update(a);
            await _context.SaveChangesAsync();

            return StatusCode(200, "Update author success");

        }

        // DELETE api/<AuthorsController>/5
        [HttpDelete("disable/{id}")]
        public async Task<ActionResult<Authors>> Delete(int id)
        {
            Authors a = await _context.Authors.FindAsync(id);
            if (a == null)
            {
                return NotFound();
            }

            a.Active = false;

            _context.Authors.Update(a);
            await _context.SaveChangesAsync();

            return StatusCode(200, "Disable authors success");
        }
    }
}
