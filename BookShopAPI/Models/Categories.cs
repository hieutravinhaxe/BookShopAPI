using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookShopAPI.Models
{
    public partial class Categories
    {
        public Categories()
        {
            Books = new HashSet<Books>();
        }
        [Key]
        public int CatId { get; set; }
        [Required(ErrorMessage = "You must enter category's name")]
        public string CatName { get; set; }
        public string CatDesc { get; set; }

        [Required(ErrorMessage ="You must set category is available or not")]
        public bool? Active { get; set; }

        public virtual ICollection<Books> Books { get; set; }
    }
}
