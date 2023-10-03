using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OrderEase.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderEase.Models
{
    public class Order
    {
        [Key]   
        [Display(Name ="Order ID")]
        public int OrderID { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int Quantity { get; set; }

        [Display(Name ="Total Price")]
        public decimal TotalPrice { get; set; }

        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }

        [Display(Name = "Delivery Date")]
        public DateTime DeliveryDate { get; set; }
        public string Supplier { get; set; }

        [Display(Name = "Order Status")]
        public OrderStatus OrderStatus { get; set; } 

        public virtual ICollection<Item> Items { get; set; }
        
    }
}
