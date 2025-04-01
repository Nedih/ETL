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