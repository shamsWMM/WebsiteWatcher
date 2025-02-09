using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;

namespace WebsiteWatcher.Functions;

public class Query(ILogger<Query> logger)
{
 private const string SqlInputQuery = @"SELECT w.Id, w.Url, s.[Timestamp] AS LastTimestamp 
                                            FROM dbo.Websites w 
                                            LEFT JOIN dbo.Snapshots s ON w.Id = s.Id 
                                            WHERE s.[Timestamp] = (SELECT MAX([Timestamp]) FROM dbo.Snapshots WHERE Id = w.Id)  
                                            AND s.[Timestamp] BETWEEN DATEADD(hour, -3, GETUTCDATE()) AND GETUTCDATE()";
    [Function(nameof(Query))]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req,
    [SqlInput(SqlInputQuery, "WebsiteWatcherConnect")] IReadOnlyList<dynamic> websites)
    {
        logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult(websites);
    }
}

