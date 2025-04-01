using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using ETL.EF;
using ETL.Models;

namespace ETL
{
    internal class RecordService
    {
        private readonly IRepository<Record> _repo;

        public RecordService(IRepository<Record> repo)
        {
            _repo = repo;
        }

        public void ParseCsv()
        {
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                TrimOptions = TrimOptions.Trim 
            };

            using var reader = new StreamReader(AppConfig.DataFilePath);
            using var csv = new CsvReader(reader, csvConfig);

            csv.Context.RegisterClassMap<RecordMap>();

            var records = csv.GetRecords<Record>()
                //.Where(record => record.FareAmount >= 0 &&
                //record.PassengerCount >= 0)
                .ToList();

            var (uniqueRecords, duplicates) = RemoveDuplicates(records);

            WriteDuplicatesToCsv(duplicates);

            _repo.BulkAddAndSave(uniqueRecords);

            Console.WriteLine($"Inserted {uniqueRecords.Count} records.");
            Console.WriteLine($"Removed {duplicates.Count} duplicate records (saved to {AppConfig.DuplicatesFilePath}).");
        }

        public (List<Record> uniqueRecords, List<Record> duplicates) RemoveDuplicates(IEnumerable<Record> records)
        {
            var seenRecords = new HashSet<string>();
            var uniqueRecords = new List<Record>();
            var duplicateRecords = new List<Record>();

            foreach (var record in records)
            {
                string key = $"{record.TpepPickupDatetime:O}-{record.TpepDropoffDatetime:O}-{record.PassengerCount}";

                if (seenRecords.Contains(key))
                {
                    duplicateRecords.Add(record);
                }
                else
                {
                    seenRecords.Add(key);
                    uniqueRecords.Add(record);
                }
            }

            return (uniqueRecords, duplicateRecords);
        }

        public void WriteDuplicatesToCsv(IEnumerable<Record> duplicates)
        {
            using var writer = new StreamWriter(AppConfig.DuplicatesFilePath);
            using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture));

            csv.WriteRecords(duplicates);
        }

        public void AddRecords(IEnumerable<Record> records)
        {
            _repo.BulkAddAndSave(records);
        }

        public void UpdateRecords(IEnumerable<Record> records)
        {
            _repo.BulkUpdateAndSave(records);
        }

        public void RemoveRecords(IEnumerable<Record> records)
        {
            _repo.BulkRemoveAndSave(records);
        }
        public int Count()
        {
            return _repo.Count();
        }
        public IEnumerable<Record> GetRecords()
        {
            return _repo.GetAll();
        }
        public IEnumerable<Record> GetRecords(Func<Record, bool> predicate)
        {
            return _repo.Where(predicate);
        }
        public int Count(Func<Record, bool> predicate)
        {
            return _repo.Count(predicate);
        }
    }
}
