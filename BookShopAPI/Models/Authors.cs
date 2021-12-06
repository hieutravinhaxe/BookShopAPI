using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookShopAPI.Models
{
    public partial class Authors
    {
        public Authors()
        {
            Books = new HashSet<Books>();
        }
        [Key]
        public int AuthorId { get; set; }
        [Required(ErrorMessage = "You must enter author's name")]
        public string AuthorName { get; set; }
        public string AuthorBio { get; set; }

        [Required(ErrorMessage ="You must set author is available or not")]
        public bool? Active { get; set; }

        public virtual ICollection<Books> Books { get; set; }
    }
}
