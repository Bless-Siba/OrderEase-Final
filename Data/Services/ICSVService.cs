using OrderEase.Models;

namespace OrderEase.Data.Services
{
    public interface ICSVService
    {

        public IEnumerable<Order> ReadCSV<Order>(Stream file);
        void ProcessOrder (Order order); 
    }
}
