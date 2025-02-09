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

CREATE TABLE [dbo]. [Snapshots](
    [Id] [uniqueidentifier] NOT NULL, 
    [Content] [nvarchar](max) NOT NULL,
CONSTRAINT [PK_Snapshots] PRIMARY KEY CLUSTERED
(
    [Id] ASC
))
GO
```