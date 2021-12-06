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

namespace BookShopAPI.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    [Route("api/admin/[controller]")]
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Books>>> Get()
        {
            return await _context.Books.AsNoTracking().ToListAsync();
        }

        // GET api/<BooksController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Books>> GetBooks(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        public static readonly List<string> ImageExtensions = new List<string> { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG" };

       
        [HttpPost("Add")]
        public async Task<ActionResult<Books>> Post([FromForm] Books b, [FromForm] IFormFile file)
        {
            Authors author = await _context.Authors.FindAsync(b.AuthorId);
            Categories cate = await _context.Categories.FindAsync(b.CategoryId);
            if (author == null || cate == null)
            {
                return BadRequest();
            }
            if (file != null)
            {
                if (!ImageExtensions.Contains(Path.GetExtension(file.FileName.ToUpper())))
                {
                    return BadRequest("Invalid Extension File");
                }

                try
                {
                    if (!Directory.Exists(_environment.WebRootPath + "\\Images\\"))
                    {
                        Directory.CreateDirectory(_environment.WebRootPath + "\\Images\\");
                    }
                    var fileName = DateTime.Now.Ticks.ToString() + Path.GetExtension(file.FileName);

                    using (FileStream filestream = System.IO.File.Create(_environment.WebRootPath + "\\Images\\" + fileName))
                    {
                        file.CopyTo(filestream);
                        b.CoverImage = fileName;
                        filestream.Flush();
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest();
                }

            }
            else
            {
                b.CoverImage = "default.jpg";
            }
            _context.Books.Add(b);
            await _context.SaveChangesAsync();

            return StatusCode(201, "Add book success");
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult<Books>> Put(int id, [FromForm] Books b, [FromForm] IFormFile file)
        {
            Authors author = await _context.Authors.FindAsync(b.AuthorId);
            Categories cate = await _context.Categories.FindAsync(b.CategoryId);
            if (author == null || cate == null || id!=b.BookId)
            {
                return BadRequest("id");
            }
            Books book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            if (!book.CoverImage.Equals("default.jpg"))
            {
                System.IO.File.Delete(_environment.WebRootPath + "/Images/" + book.CoverImage);

            }

            if (file != null)
            {
                if (!ImageExtensions.Contains(Path.GetExtension(file.FileName.ToUpper())))
                {
                    return BadRequest("extension");
                }
                try
                {
                    if (!Directory.Exists(_environment.WebRootPath + "\\Images\\"))
                    {
                        Directory.CreateDirectory(_environment.WebRootPath + "\\Images\\");
                    }
                    var fileName = DateTime.Now.Ticks.ToString() + Path.GetExtension(file.FileName);

                    using (FileStream filestream = System.IO.File.Create(_environment.WebRootPath + "\\Images\\" + fileName))
                    {
                        file.CopyTo(filestream);
                        b.CoverImage = fileName;
                        filestream.Flush();
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest("ex");
                }
            }
            else
            {
                b.CoverImage = "default.jpg";
            }
            book.Title = b.Title;
            book.Price = b.Price;
            book.Discound = b.Discound;
            book.AuthorId = b.AuthorId;
            book.CategoryId = b.CategoryId;
            book.Description = b.Description;
            book.CoverImage = b.CoverImage;
            book.Active = b.Active;

            _context.Entry(book).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Ok("Update books success");
        }

        [HttpDelete("disable/{id}")]
        public async Task<ActionResult<Books>> DeleteProducts(int id)
        {
            var b = await _context.Books.FindAsync(id);
            if (b == null)
            {
                return NotFound();
            }

            b.Active = false;

            await _context.SaveChangesAsync();

            return StatusCode(200, "Book have been disable");
        }
    }
}
