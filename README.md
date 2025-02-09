# WebsiteWatcher
References: [Azure Functions for Developers](https://github.com/LinkedInLearning/Azure-functions-for-developers-3379050.git) by [rdiazconcha](https://github.com/rdiazconcha).

## Required
- .NET SDK
- Azure Functions Core Tools

## Creating Azure Functions From Terminal
```bash
func init # select isolated worker and c#
# create a new function
func new # select type of trigger
# build the project and load functions
func start
```

## SQL Database
- https://hub.docker.com/r/microsoft/azure-sql-edge
- Azure Data Studio
- "Data Source=localhost;User Id=sa;Password=Admin123$;Integrated Security=True;TrustServerCertificate=true;Trusted_Connection=false"


# TimerTrigger - C<span>#</span>
## How it works
For a `TimerTrigger` to work, you provide a schedule in the form of a [cron expression](https://en.wikipedia.org/wiki/Cron#CRON_expression)(See the link for full details). A cron expression is a string with 6 separate expressions which represent a given schedule via patterns. The pattern we use to represent every 5 minutes is `0 */5 * * * *`. This, in plain text, means: "When seconds is equal to 0, minutes is divisible by 5, for any hour, day of the month, month, day of the week, or year".