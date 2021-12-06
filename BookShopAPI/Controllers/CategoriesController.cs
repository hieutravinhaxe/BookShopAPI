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
    [AllowAnonymous]
    [Route("api/[controller]")]
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
            return await _context.Categories.Where(c => c.Active==true).ToListAsync();
        }

        // GET api/<CategoriesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Categories>> Get(int id)
        {
            Categories c = await _context.Categories.FindAsync(id);
            if(c== null || c.Active == false)
            {
                return NotFound();
            }
            return c;
        }
    }
}
