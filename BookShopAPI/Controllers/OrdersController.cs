using BookShopAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace BookShopAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ShopOnlineAPIContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OrdersController(ShopOnlineAPIContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }
        // GET: api/<OrdersController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Orders>>> Get()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            int Id = int.Parse(userId);
            return await _context.Orders
                .Where(o => o.UserId == Id)
                .Where(o => o.Active == true)
                .ToListAsync();
        }

        // GET api/<OrdersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Orders>> Get(int id)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            int Id = int.Parse(userId);
            Orders o = await _context.Orders.FindAsync(id);
            if(o == null || o.Active == false || o.UserId!=Id)
            {
                return NotFound();
            }
            return o;
        }

        
        // POST api/<OrdersController>
        [HttpPost("add")]
        public async Task<ActionResult<Orders>> Post(Orders o)
        {
            Users u = await _context.Users.FindAsync(o.UserId);
            if(u == null)
            {
                return BadRequest();
            }
            o.Verify = false;
            o.Delivery = false;
            o.OrderDate = DateTime.Now;
            o.Active = true;
            int MyID = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name));
            if(o.UserId != MyID)
            {
                return BadRequest();
            }


            _context.Orders.Add(o);
            await _context.SaveChangesAsync();

            return StatusCode(201, "Add item success");
        }

        // PUT api/<OrdersController>/5
        [HttpPut("update/{id}")]
        public async Task<ActionResult<Orders>> Put(int id, Orders order)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            int Id = int.Parse(userId);

            if (id != order.OrderId)
            {
                return BadRequest();
            }
            Orders a = await _context.Orders.FindAsync(id);
            if (a == null)
            {
                return NotFound();
            }
            Users u = await _context.Users.FindAsync(order.UserId);
            if (u == null)
            {
                return BadRequest();
            }
            if(a.UserId != Id)
            {
                return NotFound();
            }

            a.Phone = order.Phone;
            a.Address = order.Address;
            a.Verify = order.Verify;
            a.Delivery = order.Delivery;
            a.Active = order.Active;


            _context.Orders.Update(a);
            await _context.SaveChangesAsync();

            return StatusCode(200, "Update item success");

        }

        // DELETE api/<OrdersController>/5
        [HttpDelete("disable/{id}")]
        public async Task<ActionResult<Orders>> Delete(int id)
        {
            Orders a = await _context.Orders.FindAsync(id);
            if (a == null || a.Active == false)
            {
                return NotFound();
            }
            if(a.Verify == true)
            {
                return BadRequest();
            }

            a.Active = false;

            _context.Orders.Update(a);
            await _context.SaveChangesAsync();

            return StatusCode(200, "Disable item success");
        }
    }
}
