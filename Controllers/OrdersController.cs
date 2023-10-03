using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OrderEase.Data.Services;
using OrderEase.Migrations;
using OrderEase.Models;
using SQLitePCL;

namespace OrderEase.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrdersService _ordersService;
        private readonly IItemsService _itemsService;

        public OrdersController(IOrdersService ordersService, IItemsService itemsService)
        {
            _ordersService = ordersService;
            _itemsService = itemsService;

        }

        // GET: Orders
        public IActionResult Index()
        {
            var orders = _ordersService.GetAllOrders();
            return View(orders);
        }

        // GET: Orders/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var order = _ordersService.GetOrderByID(id.Value);

            order.Items = _itemsService.GetItemsForOrder(id);

            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Quantity,TotalPrice,OrderDate,DeliveryDate,Supplier,OrderStatus,Items")] Order order)
        {
            if (ModelState.IsValid)
            {
                return View(order);
            }
            _ordersService.CreateOrder(order);
            return RedirectToAction(nameof(Index));
        }

        // GET: Orders/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = _ordersService.GetOrderByID(id.Value);
            order.Items = _itemsService.GetItemsForOrder(id);

            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, [Bind("OrderID,Quantity,TotalPrice,OrderDate,DeliveryDate,Supplier,OrderStatus,Items")] Order order)
        {
            if (id != order.OrderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _ordersService.UpdateOrder(order);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_ordersService.OrderExists(order.OrderID))
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
            return View(order);
        }

        // GET: Orders/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = _ordersService?.GetOrderByID(id.Value);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!_ordersService.OrderExists(id))
            {
                return Problem("Entity set 'OrderDbContext.Orders'  is null.");
            }
            _ordersService.DeleteOrder(id);
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return (_ordersService.OrderExists(id));
        }

        //POST: Orders/Filter/5
        //Using LINQ to filter Orders
        [HttpPost]
        public IActionResult FilterOrders(int OrderID, DateTime? startDate, DateTime? endDate, string Supplier)
        {
            var filteredOrders = _ordersService.GetAllOrders();

            if (OrderID != 0)
            {
                filteredOrders = filteredOrders.Where(o => o.OrderID == OrderID);
            }

            if (startDate.HasValue)
            {
                filteredOrders = filteredOrders.Where(o => o.OrderDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                filteredOrders = filteredOrders.Where(o => o.OrderDate <= endDate.Value);
            }

            if (!string.IsNullOrWhiteSpace(Supplier))
            {
                filteredOrders = filteredOrders.Where((o) => o.Supplier == Supplier);
            }

            var filteredOrderList = filteredOrders.ToList();

            return View("Index", filteredOrderList);

        }

        public IActionResult ClearFilter()
        {
            TempData.Remove("FilteredOrders");
            return RedirectToAction(nameof(Index));
        }

        //GET: Orders/ExportPDF/5
        public IActionResult GeneratePdf(int OrderID)
        {
            //Create a PDF Document using iText 7
            var Order = _ordersService.GetOrderByID(OrderID);

            if (Order != null)
            {
                var pdfstream = new MemoryStream();
                var writer = new PdfWriter(pdfstream);
                var pdfDocument = new PdfDocument(writer);
                var document = new iText.Layout.Document(pdfDocument);

                // Create a font for the header
                var headerFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

                // Create a font for the order details
                var orderDetailsFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

                // Add content to the PDF document
                document.Add(new Paragraph("Order Report").SetFont(headerFont).SetFontSize(16));

                // Add current date to the top right corner
                Paragraph dateParagraph = new Paragraph()
                     .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT)
                     .Add(DateTime.Now.ToString("yyyy-MM-dd"));
                document.Add(dateParagraph);


                document.Add(new Paragraph($"Order ID: {Order.OrderID}")
                    .SetFont(orderDetailsFont)
                    .SetFontSize(12));

                document.Add(new Paragraph($"Quantity: {Order.Quantity}")
                    .SetFont(orderDetailsFont)
                    .SetFontSize(12));

                document.Add(new Paragraph($"Total Price: R {Order.TotalPrice:F2}")
                    .SetFont(orderDetailsFont)
                    .SetFontSize(12));

                document.Add(new Paragraph($"Order Date: {Order.OrderDate.ToShortDateString()}")
                    .SetFont(orderDetailsFont)
                    .SetFontSize(12));

                document.Add(new Paragraph($"Delivery Date: {Order.DeliveryDate.ToShortDateString()}")
                    .SetFont(orderDetailsFont)
                    .SetFontSize(12));

                document.Add(new Paragraph($"Supplier: {Order.Supplier}")
                    .SetFont(orderDetailsFont)
                    .SetFontSize(12));

                document.Add(new Paragraph($"Order Status: {Order.OrderStatus}")
                    .SetFont(orderDetailsFont)
                    .SetFontSize(12));

                //Fetch and add Item data associated with this Order
                var items = _itemsService.GetItemsForOrder(Order.OrderID);
                if (items != null && items.Any())
                {
                    document.Add(new Paragraph("Item(s):"));

                    Table itemTable = new Table(new float[] { 1, 2, 2, 1 });

                    itemTable.AddCell("Item ID").SetBold();
                    itemTable.AddCell("Item Name").SetBold();
                    itemTable.AddCell("Quantity in Stock").SetBold();
                    itemTable.AddCell("Price").SetBold();

                    foreach (var item in items)
                    {
                        itemTable.AddCell(item.ItemID.ToString());
                        itemTable.AddCell(item.ItemName.ToString());
                        itemTable.AddCell(item.QuantityInStock.ToString());
                        itemTable.AddCell(item.Price.ToString());
                    }
                    document.Add(itemTable);
                }

                document.Close();

                //Return the PDF as a File Result

                return File(pdfstream.ToArray(), "application/pdf", "OrderReport.pdf");
            }
            else
            {
                return NotFound("Order not found");
            }


        }
    }
}

