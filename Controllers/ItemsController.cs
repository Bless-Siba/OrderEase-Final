using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OrderEase.Data;
using OrderEase.Data.Services;
using OrderEase.Models;

namespace OrderEase.Controllers
{
    public class ItemsController : Controller
    {
        private readonly IItemsService _itemsService;
        private readonly IOrdersService _ordersService;
        private SelectList OrderIDList;

        public ItemsController(IItemsService itemsService, IOrdersService ordersService)
        {
            _itemsService = itemsService;
            _ordersService = ordersService;
        }

        // GET: Items
        public IActionResult Index()
        {
            var items = _itemsService.GetAllItems();
            return View(items);
        }

        // GET: Items/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = _itemsService.GetItemByID(id.Value);

            if(item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            var itemsWithOrders = _ordersService.GetAllOrders();
            var orderIDs = itemsWithOrders.Select(item => new
            {
                OrderID = item.OrderID,
            }).ToList();
            ViewBag.OrderIDList = new SelectList(orderIDs, "OrderID");
            return View();
        }

        // POST: Items/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ItemName,Price,QuantityInStock")] Item item)
        {
            if (ModelState.IsValid)
            {
                return View(item);
            }
            _itemsService.CreateItem(item);
            return RedirectToAction(nameof(Index));
        }

        // GET: Items/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = _itemsService.GetItemByID(id.Value);

            if (item == null)
            {
                return NotFound();
            }

            var Orders = _ordersService.GetAllOrders();
            var orderIDList = new SelectList(Orders, "OrderID");
            {
                OrderIDList = orderIDList;
            };

            return View(item);
            
        }

        // POST: Items/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int? id, [Bind("ItemName,Price,QuantityInStock,OrderID")] Item item)
        {
            if (id != item.ItemID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _itemsService.UpdateItem(item);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_itemsService.ItemExists(item.ItemID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        // GET: Items/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = _itemsService.GetItemByID(id.Value);

            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!_itemsService.ItemExists(id))
            {
                return Problem("Entity set 'OrderDbContext.Items'  is null.");
            }
           _itemsService.DeleteItem(id);
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
          return (_itemsService.ItemExists(id));
        }
    }
}
