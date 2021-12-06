﻿using BookShopAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class ReviewsController : ControllerBase
    {
        private readonly ShopOnlineAPIContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ReviewsController(ShopOnlineAPIContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
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

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<Reviews>> Delete(int id)
        {
            Reviews a = await _context.Reviews.FindAsync(id);
            if (a == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(a);
            await _context.SaveChangesAsync();

            return StatusCode(200, "Delete review success");
        }
    }
}
