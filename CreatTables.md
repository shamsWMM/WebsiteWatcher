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

-- Delete all the rows
DELETE Websites;
GO
DELETE Snapshots;
GO

--Add the Timestamp column to the Snapshots table
ALTER TABLE Snapshots
ADD [Timestamp] DATETIME NOT NULL DEFAULT GETUTCDATE ();
GO

--Add the Timestamp column to the Websites table
ALTER TABLE Websites
ADD [Timestamp] DATETIME NOT NULL DEFAULT GETUTCDATE();
GO

-- Drop the existing primary key constraint
ALTER TABLE Snapshots DROP CONSTRAINT PK_Snapshots;
GO

-- Add the new primary key constraint with Id and Timestamp
ALTER TABLE Snapshots
ADD CONSTRAINT PK_Snapshots PRIMARY KEY ([Id], [Timestamp])
```