using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using OrderEase.Models;

namespace OrderEase.Data.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly OrderDbContext _context;
        public OrdersService(OrderDbContext context)
        {
            _context = context;
        }

        public Order GetOrderByID(int? OrderID)
        {
            var order = _context.Orders.Find(OrderID);

            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }
            return order;
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _context.Orders.ToList();
        }

        public void CreateOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }
        public void UpdateOrder(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }
            _context.Orders.Update(order);
            _context.SaveChanges();
        }

        public void DeleteOrder(int OrderID)
        {
            var OrderToDelete = _context.Orders.Find(OrderID);
            if (OrderToDelete != null)
            {
                _context.Orders.Remove(OrderToDelete);
                _context.SaveChanges();
            }
        }

        public bool OrderExists(int OrderID)
        {
            return _context.Orders.Any(o => o.OrderID == OrderID);
        }

        public async Task SaveChangesAsync()
        {
          await _context.SaveChangesAsync();
        }
    }
}
