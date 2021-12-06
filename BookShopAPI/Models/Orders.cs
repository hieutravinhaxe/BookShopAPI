using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookShopAPI.Models
{
    public partial class Orders
    {
        public Orders()
        {
            OrderItems = new HashSet<OrderItems>();
        }
        [Key]
        public int OrderId { get; set; }
        [Required(ErrorMessage ="You must set order form user")]
        public int? UserId { get; set; }
        [Phone]
        [Required(ErrorMessage ="You must enter your phone number")]
        public string Phone { get; set; }

        [Required(ErrorMessage ="You must enter your adress")]
        public string Address { get; set; }
        public DateTime OrderDate { get; set; }

        public bool Verify { get; set; }

        public bool Delivery { get; set; }
        public bool Active { get; set; }

        public virtual Users User { get; set; }
        public virtual ICollection<OrderItems> OrderItems { get; set; }
    }
}
