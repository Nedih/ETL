using ETL;
using ETL.EF;
using ETL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

DownloadCSV();

var services = new ServiceCollection()
    .AddDbContext<AppDbContext>()
    .AddScoped<IRepository<Record>, Repository<Record>>()
    .AddScoped<RecordService>()
    .BuildServiceProvider();

using var scope = services.CreateScope();
var recordService = services.GetRequiredService<RecordService>();

recordService.ParseCsv();

//recordService.RemoveRecords(recordService.GetRecords());

Console.WriteLine($"Row count: {recordService.Count()}");

Console.WriteLine("Data inserted successfully!");

var records = recordService.GetRecords();



Console.ReadKey();

static async void DownloadCSV()
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

static void HighestAvgTipLocation(List<Record> records)
{
    var highestAvgTipLocation = records
        .GroupBy(r => r.PULocationID)
        .Select(g => new { PULocationID = g.Key, AvgTip = g.Average(r => r.TipAmount) })
        .OrderByDescending(g => g.AvgTip)
        .FirstOrDefault();

    Console.WriteLine($"🚖 Highest Avg Tip Location: {highestAvgTipLocation?.PULocationID} with Avg Tip: {highestAvgTipLocation?.AvgTip:C}");
}

static void LongestFaresByDistance(List<Record> records)
{
    var top100LongestByDistance = records
        .OrderByDescending(r => r.TripDistance)
        .Take(100)
        .ToList();

    Console.WriteLine($"📏 Top 100 Longest Fares (by Distance) Found: {top100LongestByDistance.Count}");
}

static void LongestFaresByTime(List<Record> records)
{
    var top100LongestByTime = records
        .OrderByDescending(r => (r.TpepDropoffDatetime - r.TpepPickupDatetime).TotalMinutes)
        .Take(100)
        .ToList();

    Console.WriteLine($"⏳ Top 100 Longest Fares (by Time) Found: {top100LongestByTime.Count}");
}

static void SearchByPULocation(List<Record> records, int searchPULocationId)
{
    var searchResults = records
        .Where(r => r.PULocationID == searchPULocationId)
        .ToList();

    Console.WriteLine($"🔍 Found {searchResults.Count} records for PULocationId: {searchPULocationId}");
}