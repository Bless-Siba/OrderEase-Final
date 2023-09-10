using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using OrderEase.Data.Services;
using OrderEase.Models;
using System.Linq.Expressions;

namespace OrderEase.Controllers
{
    public class CSVController : Controller
    {
        private  readonly ICSVService _csvService;
        private readonly ILogger<CSVController> _logger;

        public CSVController(ICSVService csvService, ILogger<CSVController> logger)
        {
            _csvService = csvService;
            _logger = logger;
        }

        //GET: CSV Import
        public IActionResult CSVImpport()
        {
            return View();
        }

        //POST: CSV Import
        [HttpPost("upload-csv")]
        public IActionResult UploadCSV(IFormFile file)
        {
            if (file != null && file.Length>0)
            {
                try
                {
                    using (var stream = file.OpenReadStream())
                    {
                        var orders = _csvService.ReadCSV<Order>(stream);

                      if (orders.Any())
                        {
                            return View("CSVPreview", orders); //Render the preview view when there are orders to display. 
                        }
                        TempData["SuccessMessage"] = "CSV Import was successful.";
                    }
                }
                catch (Exception ex) 
                {
                    _logger.LogError(ex, "Error importing CSV.");
                    ModelState.AddModelError("", "An error occurred during CSV import.");
                }
            } 
            else
            {
                ModelState.AddModelError("", "Please select a CSV file to import.");
            }
            return RedirectToAction("CSVImport"); //Return to the CVSImport page (GET) after processing the file. 
        } 
    }
}
