using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopAPI.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        public static IWebHostEnvironment _environment;

        public ImagesController(IWebHostEnvironment hosting)
        {
            _environment = hosting;
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            Byte[] b = System.IO.File.ReadAllBytes(_environment.WebRootPath + "\\Images\\" + id);
            return File(b, "image/jpeg");
        }
    }
}
