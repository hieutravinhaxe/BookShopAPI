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
    [Authorize(Roles = "Admin")]
    [Route("api/admin/[controller]")]
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Orders>>> Get()
        {
            return await _context.Orders.ToListAsync();
        }

        // GET api/<OrdersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Orders>> Get(int id)
        {
            return await _context.Orders.FindAsync(id);
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult<Orders>> Put(int id, Orders order)
        {
           
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

            a.Phone = order.Phone;
            a.Address = order.Address;
            a.Verify = order.Verify;
            a.Delivery = order.Delivery;
            a.Active = order.Active;


            _context.Orders.Update(a);
            await _context.SaveChangesAsync();

            return StatusCode(200, "Update order success");

        }

        [HttpDelete("disable/{id}")]
        public async Task<ActionResult<Orders>> Delete(int id)
        {
            Orders a = await _context.Orders.FindAsync(id);
            if (a == null)
            {
                return NotFound();
            }
            a.Active = false;

            _context.Orders.Update(a);
            await _context.SaveChangesAsync();

            return StatusCode(200, "Disable order success");
        }
    }
}
