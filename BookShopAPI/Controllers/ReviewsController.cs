using BookShopAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BookShopAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly ShopOnlineAPIContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ReviewsController(ShopOnlineAPIContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }
        // GET: api/<ReviewsController>
        [HttpGet("books/{id}")]
        public async Task<ActionResult<IEnumerable<Reviews>>> GetReviewOfBook(int id)
        {
            //return await _context.Reviews.ToListAsync();
            Books b = await _context.Books.FindAsync(id);
            if(b == null || b.Active == false)
            {
                return NotFound();
            }
            return await _context.Reviews.Where(r => r.BookId==id).ToListAsync();
        }

        // GET api/<ReviewsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reviews>> Get(int id)
        {
            Reviews r = await _context.Reviews.FindAsync(id);
            if(r == null)
            {
                return NotFound();
            }
            Books b = await _context.Books.FindAsync(r.BookId);
            if(b == null || b.Active == false)
            {
                return NotFound();
            }
            return r;
        }

        // POST api/<ReviewsController>
        [HttpPost("add")]
        public async Task<ActionResult<Reviews>> Post(Reviews item)
        {
            Books b = await _context.Books.FindAsync(item.BookId);
            Users u = await _context.Users.FindAsync(item.UserId);
            if (u == null || b == null)
            {
                return BadRequest();
            }
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            int Id = int.Parse(userId);
            if (item.UserId != Id)
            {
                return BadRequest();
            }

            item.ReviewDate = DateTime.Now;

            _context.Reviews.Add(item);
            await _context.SaveChangesAsync();

            return StatusCode(201, "Add review success");
        }

        // PUT api/<ReviewsController>/5
        [HttpPut("update/{id}")]
        public async Task<ActionResult<Reviews>> Put(int id, Reviews item)
        {
            if (id != item.ReviewId)
            {
                return BadRequest();
            }
            Reviews a = await _context.Reviews.FindAsync(id);
            if (a == null)
            {
                return NotFound();
            }
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            int Id = int.Parse(userId);
            if (a.UserId != Id)
            {
                return BadRequest();
            }
            Books b = await _context.Books.FindAsync(item.BookId);
            Users u = await _context.Users.FindAsync(item.UserId);
            if (u == null || b == null)
            {
                return BadRequest();
            }

            a.ReviewTitle = item.ReviewTitle;
            a.ReviewDetail = item.ReviewDetail;
            a.Rate = item.Rate;
            a.BookId = item.BookId;
            a.UserId = item.UserId;


            _context.Reviews.Update(a);
            await _context.SaveChangesAsync();

            return StatusCode(200, "Update review success");

        }

        // DELETE api/<ReviewsController>/5
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<Reviews>> Delete(int id)
        {
            Reviews a = await _context.Reviews.FindAsync(id);
            if (a == null)
            {
                return NotFound();
            }
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            int Id = int.Parse(userId);
            if (a.UserId != Id)
            {
                return BadRequest();
            }

            _context.Reviews.Remove(a);
            await _context.SaveChangesAsync();

            return StatusCode(200, "Delete review success");
        }
    }
}
