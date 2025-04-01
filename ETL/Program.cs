using ETL;
using ETL.EF;
using ETL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection()
    .AddDbContext<AppDbContext>()
    .AddScoped<IRepository<Record>, Repository<Record>>()
    .AddScoped<RecordService>()
    .BuildServiceProvider();

using var scope = services.CreateScope();
var recordService = services.GetRequiredService<RecordService>();

await DownloadCSV();

recordService.ParseCsv();

Console.WriteLine($"Row count: {recordService.Count()}");
Console.WriteLine("Data inserted successfully!");

while (true)
{
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();

    var records = recordService.GetRecords();

    Console.Clear();
    Console.WriteLine("=== ETL Console Interface ===");
    Console.WriteLine("1. Find highest average tip location");
    Console.WriteLine("2. Show top 100 longest trips (by distance)");
    Console.WriteLine("3. Show top 100 longest trips (by time)");
    Console.WriteLine("4. Search by custom PULocationId");
    Console.WriteLine("5. Clear database");
    Console.WriteLine("6. Repopulate database from CSV");
    Console.WriteLine("0. Exit");
    Console.WriteLine($"\nCurrent number of rows in the DB: {recordService.Count()}");
    Console.Write("\nSelect an option: ");

    switch (Console.ReadLine())
    {
        case "1":
            HighestAvgTipLocation(records);
            break;
        case "2":
            LongestFaresByDistance(records);
            break;
        case "3":
            LongestFaresByTime(records);
            break;
        case "4":
            int id;
            while (true)
            {
                Console.Clear();
                Console.Write("Enter PULocationID: ");
                if (int.TryParse(Console.ReadLine(), out id))
                {
                    SearchByPULocation(records, id);
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer.");
                }
            }
            break;
        case "5":
            recordService.RemoveRecords(recordService.GetRecords());
            Console.Write("All records have successfully been removed!");
            break;
        case "6":
            recordService.ParseCsv();
            Console.WriteLine($"Row count: {recordService.Count()}");
            Console.WriteLine("Data inserted successfully!");
            break;
        case "0":
            return;
        default:
            Console.WriteLine("Invalid option. Press any key to continue...");
            Console.ReadKey();
            break;
    }
}

static async Task DownloadCSV()
{
    using HttpClient client = new HttpClient();
    var response = await client.GetAsync(AppConfig.CloudUrl);

    if (response.IsSuccessStatusCode)
    {
        var fileBytes = await response.Content.ReadAsByteArrayAsync();
        await File.WriteAllBytesAsync(AppConfig.DataFileName, fileBytes);
        Console.WriteLine($"File downloaded to: {AppConfig.DataFilePath}");
    }
    else
    {
        Console.WriteLine($"Error: {response.StatusCode}");
    }
}

static void HighestAvgTipLocation(IEnumerable<Record> records)
{
    var highestAvg = records
        .GroupBy(r => r.PULocationID)
        .Select(g => new { PULocationID = g.Key, AvgTip = g.Average(r => r.TipAmount) })
        .OrderByDescending(g => g.AvgTip)
        .FirstOrDefault();

    Console.WriteLine($"Pick-up location ID: {highestAvg?.PULocationID} has the highest Avg Tip amount: {highestAvg?.AvgTip:C}");
}

static void LongestFaresByDistance(IEnumerable<Record> records)
{
    var top100 = records
        .OrderByDescending(r => r.TripDistance)
        .Take(100)
        .ToList();

    Console.WriteLine($"Top 100 Longest Fares (by Distance) Found: {top100.Count}");

    PrintRecords(top100);
}

static void LongestFaresByTime(IEnumerable<Record> records)
{
    var top100 = records
        .OrderByDescending(r => (r.TpepDropoffDatetime - r.TpepPickupDatetime).TotalMinutes)
        .Take(100)
        .ToList();

    Console.WriteLine($"Top 100 Longest Fares (by Time) Found: {top100.Count}");

    foreach (var record in top100)
    {
        var duration = (record.TpepDropoffDatetime - record.TpepPickupDatetime).TotalMinutes;

        var properties = typeof(Record).GetProperties();
        foreach (var prop in properties)
        {
            Console.WriteLine($"{prop.Name}: {prop.GetValue(record)}");
        }
        Console.WriteLine($"Duration: {duration:F2} min");
        Console.WriteLine(new string('-', 50));
    }
}

static void SearchByPULocation(IEnumerable<Record> records, int searchPULocationId)
{
    var searchResults = records
        .Where(r => r.PULocationID == searchPULocationId)
        .ToList();

    Console.WriteLine($"Found {searchResults.Count} records for PULocationId: {searchPULocationId}\n");
    PrintRecords(searchResults);
}

static void PrintRecords(IEnumerable<Record> records)
{
    foreach (var record in records)
    {
        var properties = typeof(Record).GetProperties();
        foreach (var prop in properties)
        {
            Console.WriteLine($"{prop.Name}: {prop.GetValue(record)}");
        }
        Console.WriteLine(new string('-', 50));
    }
}