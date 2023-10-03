using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace OrderEase.Models
{
    public class OrderCSVMap:ClassMap<Order>
    {
        public OrderCSVMap()
        {
            Map(m => m.Quantity).Name("Quantity").TypeConverter<Int32Converter>();
            Map(m => m.TotalPrice).Name("TotalPrice");
            Map(m => m.OrderDate).Name("OrderDate");
            Map(m => m.DeliveryDate).Name("DeliveryDate");
            Map(m => m.Supplier).Name("Supplier");
            Map(m => m.OrderStatus).Name("OrderStatus");
        }
    }
}
