using iText.Layout.Borders;
using OrderEase.Models;

namespace OrderEase.Data.Services
{
    public class ItemsService : IItemsService
    {
        private readonly OrderDbContext _context;
        public ItemsService(OrderDbContext context)
        {
            _context = context;
        }

        public Item GetItemByID(int ItemID)
        {
            var item = _context.Items.Find(ItemID);

            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));  
            }
            return item;
        }

        public IEnumerable<Item> GetAllItems()
        {
            return _context.Items.ToList();
        }

        public void CreateItem(Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            _context.Items.Add(item);
            _context.SaveChanges();
        }

        public void UpdateItem(Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));    
            }
            _context.Items.Update(item);
            _context.SaveChanges();
        }

        public void DeleteItem(int ItemID)
        {
            var ItemToDelete = _context.Items.Find(ItemID);
            if (ItemToDelete != null)
            {
                _context.Items.Remove(ItemToDelete);
                _context.SaveChanges();
            }
        }

        public bool ItemExists(int ItemID)
        {
            return _context.Items.Any(item => item.ItemID == ItemID);
        }
    }
}
