using CsvHelper;
using CsvHelper.Configuration;
using OrderEase.Models;
using System.Globalization;

namespace OrderEase.Data.Services
{
    public class CSVService : ICSVService
    {
        private readonly OrderDbContext _context;
        private readonly ILogger<CSVService> _logger;
        public CSVService(OrderDbContext context, ILogger<CSVService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public void ProcessOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public void ImportOrdersFromCVS(Stream csvStream)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                IgnoreBlankLines = true,
                Delimiter = ",",
            };

            //Create a CsvReader with the custom mapping class
            using (var reader = new StreamReader(csvStream))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Context.RegisterClassMap<OrderCSVMap>(); //Register the mapping class 

                //Read and map the CSV records to the existing Order model
                var orders = csv.GetRecords<Order>().ToList();

                //Save the orders to the database
                foreach(var order in orders)
                {
                    try
                    {
                        _context.Orders.Add(order);
                        _context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error saving order to the database.");
                    }
                }
            }
        
        }

        public IEnumerable<T> ReadCSV<T>(Stream file)
        {
            try
            {
                using (var reader = new StreamReader(file))
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    return csv.GetRecords<T>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading CSV.");
                throw;
            }
        }
    }
}
