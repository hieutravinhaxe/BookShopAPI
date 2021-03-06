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

namespace BookShopAPI.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    [Route("api/admin/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly ShopOnlineAPIContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OrderItemsController(ShopOnlineAPIContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<OrderItems>>> GetAll()
        {
            return await _context.OrderItems.ToListAsync();
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItems>>> Get()
        {
            var orderId = HttpContext.Request.Query["order"];
            if (orderId == "")
            {
                return BadRequest();
            }
            int OID = int.Parse(orderId);
            return await _context.OrderItems.Where(i => i.OrderId == OID).ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItems>> Get(int id)
        {
            OrderItems i = await _context.OrderItems.FindAsync(id);
            if (i == null)
            {
                return NotFound();
            }
            return i;
        }

        // POST api/<OrderItemsController>
        //[HttpPost("add")]
        //public async Task<ActionResult<OrderItems>> Post(OrderItems item)
        //{
        //    Orders o = await _context.Orders.FindAsync(item.OrderId);
        //    Books b = await _context.Books.FindAsync(item.BookId);

        //    if (o == null || b == null)
        //    {
        //        return BadRequest();
        //    }

        //    _context.OrderItems.Add(item);
        //    await _context.SaveChangesAsync();

        //    return StatusCode(201, "Add item success");
        //}

        // PUT api/<OrderItemsController>/5
        [HttpPut("update/{id}")]
        public async Task<ActionResult<OrderItems>> Put(int id, OrderItems item)
        {
            if (id != item.ItemId)
            {
                return BadRequest();
            }
            OrderItems a = await _context.OrderItems.FindAsync(id);
            if (a == null)
            {
                return NotFound();
            }
            Orders o = await _context.Orders.FindAsync(item.OrderId);
            Books b = await _context.Books.FindAsync(item.BookId);
            if (o == null || b == null)
            {
                return BadRequest();
            }
            

            a.BookId = item.BookId;
            a.Quantity = item.Quantity;
            a.Price = item.Price;
            a.OrderId = item.OrderId;

            _context.OrderItems.Update(a);
            await _context.SaveChangesAsync();

            return StatusCode(200, "Update item success");

        }

        // DELETE api/<OrderItemsController>/5
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<OrderItems>> Delete(int id)
        {
            OrderItems a = await _context.OrderItems.FindAsync(id);
            if (a == null)
            {
                return NotFound();
            }
            _context.OrderItems.Remove(a);
            await _context.SaveChangesAsync();

            return StatusCode(200, "Delete item success");
        }
    }
}
