using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookShopAPI.Models
{
    public partial class OrderItems
    {
        [Key]
        public int ItemId { get; set; }
        [Required(ErrorMessage ="You must choose book")]
        public int? BookId { get; set; }
        [Required(ErrorMessage ="You must choose quantity")]
        [Range(1, 10, ErrorMessage = "Quantity just from 1 - 10")]
        public int? Quantity { get; set; }
        [Required(ErrorMessage ="You must set price")]
        [Range(0, int.MaxValue, ErrorMessage = "You must enter price more than 0")]
        public float? Price { get; set; }
        [Required(ErrorMessage ="You must choose from order")]
        public int? OrderId { get; set; }

        public virtual Books Book { get; set; }
        public virtual Orders Order { get; set; }
    }
}
