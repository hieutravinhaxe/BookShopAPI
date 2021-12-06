using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookShopAPI.Models
{
    public partial class Books
    {
        public Books()
        {
            OrderItems = new HashSet<OrderItems>();
            Reviews = new HashSet<Reviews>();
        }
        [Key]
        public int BookId { get; set; }

        [Required(ErrorMessage ="You must enter book's title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "You must enter book's price")]
        [Range(0,int.MaxValue, ErrorMessage ="You must enter price more than 0")]
        public float? Price { get; set; }
        public float? Discound { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "You must enter book's author")]
        public int? AuthorId { get; set; }

        [Required(ErrorMessage = "You must enter book's cateory")]
        public int? CategoryId { get; set; }
        public string CoverImage { get; set; }
        [Required(ErrorMessage ="You must set book is available or not")]
        public bool? Active { get; set; }

        public virtual Authors Author { get; set; }
        public virtual Categories Category { get; set; }
        public virtual ICollection<OrderItems> OrderItems { get; set; }
        public virtual ICollection<Reviews> Reviews { get; set; }
    }
}
