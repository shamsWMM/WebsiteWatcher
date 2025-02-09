using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;

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
        }
    }
}