using BookShopAPI.Helpers;
using BookShopAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookShopAPI.Controllers
{
    [AllowAnonymous]
    [Route("/api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ShopOnlineAPIContext _context;
        public static IWebHostEnvironment _environment;

        public BooksController(ShopOnlineAPIContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
       
        // GET: api/<BooksController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Books>>> Get()
        {
            return await _context.Books.Where(b => b.Active == true).ToListAsync();
        }

        // GET api/<BooksController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Books>> GetBooks(int id)
        {
            var b = await _context.Books.FindAsync(id);

            if (b == null || b.Active==false)
            {
                return NotFound();
            }

            return b;
        }
    }
}
