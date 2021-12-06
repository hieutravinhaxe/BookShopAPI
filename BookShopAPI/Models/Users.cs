using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookShopAPI.Models
{
    public partial class Users
    {
        public Users()
        {
            Orders = new HashSet<Orders>();
            Reviews = new HashSet<Reviews>();
        }

        public int UserId { get; set; }

        [Required(ErrorMessage = "You must enter Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "You must enter Password")]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; }

        [Required(ErrorMessage = "You must enter Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Range(1, 2, ErrorMessage = "Invalid role")]
        public int? RoleId { get; set; }

        public bool Active { get; set; }
        public virtual Roles Role { get; set; }
        public virtual ICollection<Orders> Orders { get; set; }
        public virtual ICollection<Reviews> Reviews { get; set; }
    }
}
