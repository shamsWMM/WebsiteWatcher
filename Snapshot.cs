using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;
using HtmlAgilityPack;
using System.Xml.XPath;

namespace WebsiteWatcher;

public class Snapshot(ILogger<Snapshot> logger)
{
    [Function(nameof(Snapshot))]
    public void Run(
                [SqlTrigger("dbo.Websites", "WebsiteWatcherConnect")] IReadOnlyList<SqlChange<Website>> changes)
    {
        foreach (var change in changes)
        {
            logger.LogInformation($"{change.Operation}");
            logger.LogInformation($"Id: {change.Item.Id} Url: {change.Item.Url}");
            if (change.Operation != SqlChangeOperation.Insert)
            {
                continue;
            }

            if (string.IsNullOrEmpty(change.Item.XPathExpression))
            {
                logger.LogInformation($"No XPathExpression for {change.Item.Url}");
                continue;
            }

            try
            {
                HtmlWeb web = new ();
                HtmlDocument doc = web.Load(change.Item.Url);

                var divWithContent = doc.DocumentNode.SelectSingleNode(change.Item.XPathExpression);
                var content = divWithContent != null ? divWithContent.InnerText.Trim() : "No content";

                logger.LogInformation(content);
            }
            catch (XPathException ex)
            {
                logger.LogError($"Invalid XPathExpression {change.Item.Url}: {ex.Message}");
            }
            catch (Exception ex)
            {
                logger.LogError($"Error {change.Item.Url}: {ex.Message}");
            }
        }
    }
}