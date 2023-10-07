using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderEase.Models
{
    public class Item
    {
        [Key]
        [Display(Name = "Item ID")]
        public int ItemID { get; set; }

        [Display(Name = "Item Name")]
        public string ItemName { get; set; }
        public decimal Price { get; set; }

        [Display(Name = "Quantity in stock")]
        [Required(ErrorMessage = "Quantity in stock is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be non-negative.")]
        public int QuantityInStock { get; set; }

        //One-to-Many Relationship with Order
        [Display(Name ="Order ID")]
        public int OrderID { get; set; } //Foreign Key referecing Order

        public Order Order { get; set; }
    }
}
