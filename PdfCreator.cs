using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;
using Microsoft.Extensions.Logging;
using PuppeteerSharp;

namespace WebsiteWatcher;

public class PdfCreator(ILogger<PdfCreator> logger, PdfCreatorServivce pdfCreatorServivce)
{
    [Function(nameof(PdfCreator))]
    public async Task Run(
      [SqlTrigger("dbo.Websites", "WebsiteWatcherConnect")] SqlChange<Website>[] changes)
    {
        foreach (var change in changes)
        {
            if (change.Operation == SqlChangeOperation.Insert)
            {
                var result = await pdfCreatorServivce.ConvertPageToPdfAsync(change.Item.Url);

                logger.LogInformation($"PDF stream length is: {result.Length}");

                var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings:WebsiteWatcherStorage");
                var blobClient = new BlobClient(connectionString, "pdfs", $"{change.Item.Id}.pdf");
                await blobClient.UploadAsync(result);
            }
        }
    }
}