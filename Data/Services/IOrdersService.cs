using OrderEase.Models;

namespace OrderEase.Data.Services
{
    public interface IOrdersService
    {
        Order GetOrderByID(int OrderID);
        IEnumerable<Order> GetAllOrders();
        void CreateOrder (Order order);
        void UpdateOrder (Order order);
        void DeleteOrder (int OrderID);
        bool OrderExists(int orderID);
    }
}
