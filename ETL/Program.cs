using ETL;

DownloadCSV();

Console.ReadKey();

static async void DownloadCSV()
{
    using HttpClient client = new HttpClient();
    var response = await client.GetAsync(AppConfig.CloudUrl);

    if (response.IsSuccessStatusCode)
    {
        var fileBytes = await response.Content.ReadAsByteArrayAsync();
        await File.WriteAllBytesAsync(AppConfig.FileName, fileBytes);
        Console.WriteLine($"File downloaded to: {AppConfig.FilePath}");
    }
    else
    {
        Console.WriteLine($"Error: {response.StatusCode}");
    }
}