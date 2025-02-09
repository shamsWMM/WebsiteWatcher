using System;
using Azure.Storage.Blobs;
using HtmlAgilityPack;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;
using Microsoft.Extensions.Logging;
using PuppeteerSharp;

namespace WebsiteWatcher;

public class Watcher(ILogger<Watcher> logger)
{
    private const string SqlInputQuery = @"SELECT w.Id, w.Url, w.XPathExpression, s.Content AS LatestContent 
                                        FROM dbo.Websites w 
                                        LEFT JOIN dbo.Snapshots s ON w.Id = s.Id 
                                        WHERE s.Timestamp = (SELECT MAX(Timestamp) FROM dbo.Snapshots WHERE Id = w.Id)";

    [Function(nameof(Watcher))]
    [SqlOutput("dbo.Snapshots", "WebsiteWatcherConnect")]
    public async Task<SnapshotRecord?> Run([TimerTrigger("*/20 * * * * *")] TimerInfo myTimer,
        [SqlInput(SqlInputQuery, "WebsiteWatcherConnect")] IReadOnlyList<WebsiteModel> websites)
    {
        logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        SnapshotRecord? result = null;

        foreach (var website in websites)
        {
            HtmlWeb web = new();
            HtmlDocument doc = web.Load(website.Url);

            var divWithContent = doc.DocumentNode.SelectSingleNode(website.XPathExpression);
            var content = divWithContent != null ? divWithContent.InnerText.Trim() : "No content";
            content = content.Replace("Microsoft", "MS");

            var contentChanged = website.LatestContent != content;
            if (contentChanged)
            {
                logger.LogInformation($"Content changed for {website.Url}");
                var newPdf = await ConvertPageToPdfAsync(website.Url);

                var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings:WebsiteWatcherStorage");
                var blobClient = new BlobClient(connectionString, "pdfs", $"{website.Id}-{DateTime.UtcNow:mmddyyy}.pdf");
                await blobClient.UploadAsync(newPdf);
                logger.LogInformation($"PDF uploaded for {website.Url}");
                result = new SnapshotRecord(website.Id, content);
            }
        }

        return result;
    }
    
    private async Task<Stream> ConvertPageToPdfAsync(string url)
    {
        var browserFetcher = new BrowserFetcher();

        await browserFetcher.DownloadAsync();
        await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
        await using var page = await browser.NewPageAsync();
        await page.GoToAsync(url);
        await page.EvaluateExpressionHandleAsync("document.fonts.ready");
        var result = await page.PdfStreamAsync();
        result.Position = 0;

        return result;
    }
}

public class WebsiteModel
{
    public Guid Id { get; set; }
    public string Url { get; set; }
    public string? XPathExpression { get; set; }
    public string LatestContent { get; set; }
}