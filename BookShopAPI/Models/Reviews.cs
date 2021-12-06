using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookShopAPI.Models
{
    public partial class Reviews
    {
        [Key]
        public int ReviewId { get; set; }
        [Required(ErrorMessage ="You must enter review's title")]
        public string ReviewTitle { get; set; }

        [Required(ErrorMessage = "You must enter review's content")]
        public string ReviewDetail { get; set; }
     
        [Required(ErrorMessage = "You must enter review's rate")]
        [Range(1,5, ErrorMessage ="Rate is from 1-5 star")]
        public int? Rate { get; set; }

        [Required(ErrorMessage = "You must enter review for book")]
        public int? BookId { get; set; }

        [Required(ErrorMessage = "You must enter review from user")]
        public int? UserId { get; set; }
        public DateTime? ReviewDate { get; set; }

        public virtual Books Book { get; set; }
        public virtual Users User { get; set; }
    }
}
