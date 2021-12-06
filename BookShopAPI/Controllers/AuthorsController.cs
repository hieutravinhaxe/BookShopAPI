using BookShopAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookShopAPI.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
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
            return await _context.Authors.Where(a => a.Active == true).ToListAsync();
        }

        // GET api/<AuthorsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Authors>> Get(int id)
        {
            Authors a = await _context.Authors.FindAsync(id);
            if(a == null || a.Active == false)
            {
                return NotFound();
            }

            return a;
        }

    }
}
