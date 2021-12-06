using BookShopAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopAPI.Helpers
{
    public partial class LoginModel
    {
        public LoginModel()
        {
        }

        [Required(ErrorMessage = "You must enter Password")]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; }

        [Required(ErrorMessage = "You must enter Email")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
