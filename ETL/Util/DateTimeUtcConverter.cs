using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using CsvHelper;
using TimeZoneConverter;

namespace ETL.Util
{
    public class DateTimeUtcConverter : ITypeConverter
    {
        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new Exception("DateTime field is empty or null");

            if (DateTime.TryParseExact(text, "MM/dd/yyyy hh:mm:ss tt",
                                       CultureInfo.InvariantCulture,
                                       DateTimeStyles.None, out DateTime estTime))
            {
                TimeZoneInfo estZone = TZConvert.GetTimeZoneInfo("America/New_York");
                return TimeZoneInfo.ConvertTimeToUtc(estTime, estZone);
            }
            throw new Exception($"Invalid DateTime format: {text}");
        }

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            return ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        }
    }
}
