using OrderEase.Models;

namespace OrderEase.Data.Services
{
    public interface IItemsService
    {
        Item GetItemByID(int ItemId);
        IEnumerable<Item> GetAllItems();
        void CreateItem(Item item);
        void UpdateItem(Item item);
        void DeleteItem(int ItemID);
        bool ItemExists (int ItemID);
    }
}

