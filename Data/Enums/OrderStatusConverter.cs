using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace OrderEase.Data.Enums
{
    public class OrderStatusConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
        {
            if (Enum.TryParse(text, out OrderStatus status))
            {
                return status;
            }
            throw new InvalidOperationException($"Invalid OrderStatus value: {text}");
        }
    }

}

