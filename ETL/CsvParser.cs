using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;

namespace ETL
{
    internal class CsvParser
    {
        void Parse(string location)
        {
            using (var reader = new StreamReader(location))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<Record>().ToList();
            }
        }
    }
}
