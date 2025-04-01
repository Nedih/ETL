using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETL
{
    static class AppConfig
    {
        public static readonly string DataFileName = "data.csv";
        public static readonly string DataFilePath = Path.Combine(Directory.GetCurrentDirectory(), DataFileName);
        public static readonly string DataFileId = "1l2ARvh1-tJBqzomww45TrGtIh5j8Vud4";
        public static readonly string CloudUrl = $"https://drive.google.com/uc?export=download&id={DataFileId}";

        public static readonly string DuplicatesFileName = "duplicates.csv";
        public static readonly string DuplicatesFilePath = Path.Combine(Directory.GetCurrentDirectory(), DuplicatesFileName);
        
    }
}
