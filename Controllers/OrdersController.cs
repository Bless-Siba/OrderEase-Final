﻿using iText.Kernel.Pdf;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderEase.Data.Services;
using OrderEase.Models;



namespace OrderEase.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrdersService _ordersService;
     
        public OrdersController(IOrdersService ordersService, ICSVService csvService)
        {
            _ordersService = ordersService;
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
        public IActionResult Create([Bind("OrderID,Quantity,TotalPrice,OrderDate,DeliveryDate,Supplier,OrderStatus")] Order order)
        {
            if (ModelState.IsValid)
            {
                _ordersService.CreateOrder(order);
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = _ordersService.GetOrderByID(id.Value);

            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind("OrderID,Quantity,TotalPrice,OrderDate,DeliveryDate,Supplier,OrderStatus")] Order order)
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
            _ordersService?.DeleteOrder(id);
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _ordersService.OrderExists(id);
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

                //Add content to the PDF document 

                document.Add(new Paragraph("Order Report"));
                document.Add(new Paragraph($"Order ID:{Order.OrderID}"));
                document.Add(new Paragraph($"Quantity:{Order.Quantity}"));
                document.Add(new Paragraph($"Total Price:{Order.TotalPrice}"));
                document.Add(new Paragraph($"Order Date:{Order.OrderDate}"));
                document.Add(new Paragraph($"Delivery Date:{Order.DeliveryDate}"));
                document.Add(new Paragraph($"Supplier:{Order.Supplier}"));
                document.Add(new Paragraph($"OrderStatus:{Order.OrderStatus}"));

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

