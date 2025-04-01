using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETL
{
    static class AppConfig
    {
        public static readonly string FileName = "data.csv";
        public static readonly string FilePath = Path.Combine(Directory.GetCurrentDirectory(), FileName);
        public static readonly string FileId = "1l2ARvh1-tJBqzomww45TrGtIh5j8Vud4";
        public static readonly string CloudUrl = $"https://drive.google.com/uc?export=download&id={FileId}";
    }
}
