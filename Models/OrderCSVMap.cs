using CsvHelper.Configuration;

namespace OrderEase.Models
{
    public class OrderCSVMap:ClassMap<Order>
    {
        public OrderCSVMap()
        {
            Map(m => m.OrderID).Name("Order ID");
            Map(m => m.Quantity).Name("Quantity");
            Map(m => m.TotalPrice).Name("Total Price");
            Map(m => m.OrderDate).Name("Order Date");
            Map(m => m.DeliveryDate).Name("Delivery Date");
            Map(m => m.Supplier).Name("Supplier");
            Map(m => m.OrderStatus).Name("Order Status");
        }
    }
}
