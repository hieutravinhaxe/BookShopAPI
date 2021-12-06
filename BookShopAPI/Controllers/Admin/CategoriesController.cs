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
    public class CategoriesController : ControllerBase
    {
        private readonly ShopOnlineAPIContext _context;
        public CategoriesController(ShopOnlineAPIContext context)
        {
            _context = context;
        }
        // GET: api/<CategoriesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categories>>> Get()
        {
            return await _context.Categories.ToListAsync();
        }


        // GET api/<CategoriesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Categories>> Get(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        // POST api/<CategoriesController>
        [HttpPost("add")]
        public async Task<ActionResult<Categories>> Post(Categories Cate)
        {
            _context.Categories.Add(Cate);
            await _context.SaveChangesAsync();

            return StatusCode(201, "Add cate success");
        }

        // PUT api/<CategoriesController>/5
        [HttpPut("update/{id}")]
        public async Task<ActionResult<Categories>> Put(int id, Categories Cate)
        {
            if (id != Cate.CatId)
            {
                return BadRequest();
            }
            Categories a = await _context.Categories.FindAsync(id);
            if (a == null)
            {
                return NotFound();
            }

            a.CatName = Cate.CatName;
            a.CatDesc = Cate.CatDesc;
            a.Active = Cate.Active;

            _context.Categories.Update(a);
            await _context.SaveChangesAsync();

            return StatusCode(200, "Update Cate success");

        }

        // DELETE api/<CategoriesController>/5
        [HttpDelete("disable/{id}")]
        public async Task<ActionResult<Categories>> Delete(int id)
        {
            Categories a = await _context.Categories.FindAsync(id);
            if (a == null)
            {
                return NotFound();
            }

            a.Active = false;

            _context.Categories.Update(a);
            await _context.SaveChangesAsync();

            return StatusCode(200, "Disable Cate success");
        }
    }
}
