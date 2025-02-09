using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;

namespace WebsiteWatcher;
public class Register(ILogger<Register> logger)
{
    [Function(nameof(Register))]
    [SqlOutput("dbo.Websites", "WebsiteWatcherConnect")]
    public async Task<Website> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req
//    , SafeBrowsingService safeBrowsingService
    )
    {
        logger.LogInformation("C# HTTP trigger function processed a request.");
        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var newWebsite = JsonSerializer.Deserialize<Website>(requestBody, options);
        newWebsite.Id = Guid.NewGuid();

        // var result = safeBrowsingService.Check(newWebsite.Url);
        // if (result.HasThreat)
        // {
        //     var threats = string.Join(", ", result.Threats);
        //     logger.LogError($"Url {newWebsite.Url} has threats: {threats}");
        //     return null;
        // }
        return newWebsite;
    }
}

public class Website
{
    public Guid Id { get; set; }
    public string Url { get; set; }
    public string? XPathExpression { get; set; }
}