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
```sql
-- Create the database on Azure SQL Studio
USE master;
GO

IF NOT EXISTS (
      SELECT name
      FROM sys.databases
      WHERE name = N'WebsiteWatcher'
      )
   CREATE DATABASE [WebsiteWatcher];
GO

-- Change context before executing here
IF OBJECT_ID('dbo.Websites', 'U') IS NOT NULL
   DROP TABLE dbo.Websites;
GO
CREATE TABLE [dbo].[Websites](
    [Id] [UNIQUEIDENTIFIER] NOT NULL,
    [Url] [NVARCHAR](MAX) NOT NULL,
    [XPathExpression] [NVARCHAR](MAX) NULL,
CONSTRAINT [PK_Websites] PRIMARY KEY CLUSTERED
(
        [Id] ASC
));
GO


ALTER DATABASE WebsiteWatcher SET CHANGE_TRACKING = ON;
GO
ALTER TABLE [dbo].[Websites] ENABLE CHANGE_TRACKING;
GO
```
