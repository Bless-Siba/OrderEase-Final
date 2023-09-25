using Microsoft.AspNetCore.Mvc;
using OrderEase.Data.Services;
using OrderEase.Models;


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
        [HttpGet("CSVImport")]
        public IActionResult CSVImport()
        {
            return View("CSVImport");
        }

        //POST: Orders/CSV Import/5
        [HttpPost("CSVImport")]
        public IActionResult CSVImport(IFormFile file)
        {
            if (file != null && file.Length>0)
            {
                try
                {
                    using (var stream = file.OpenReadStream())
                    {
                      _csvService.ImportOrdersFromCSV(stream);
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
            return View("CSVImport");
        } 
    }
}
