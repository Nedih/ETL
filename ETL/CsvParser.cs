using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using ETL.Models;

namespace ETL
{
    internal class CsvParser
    {
        public static void Parse()
        {
            using var reader = new StreamReader(AppConfig.FilePath);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));

            csv.Context.RegisterClassMap<RecordMap>();

            var records = csv.GetRecords<Record>().ToList();
            
        }
    }
}
