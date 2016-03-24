SET QUOTED_IDENTIFIER OFF;
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- Get the version
IF EXISTS (SELECT OBJECT_ID('tempdb..#Version')) DROP TABLE #Version
GO

CREATE TABLE #Version (v INT)
GO

IF OBJECT_ID(N'[dbo].[SacsVersion]', 'U') IS NOT NULL
    INSERT INTO #Version SELECT TOP 1 
		CAST(PARSENAME([VersionNumber], 1) AS INT) +
		CAST(PARSENAME([VersionNumber], 2) AS INT) * 100 +
		CAST(PARSENAME([VersionNumber], 3) AS INT) * 10000 +
		CAST(PARSENAME([VersionNumber], 4) AS INT) * 1000000
	FROM [dbo].[SacsVersion] ORDER BY [InstallationDate] DESC
GO
------

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF (SELECT v FROM #Version) < 1010000 AND OBJECT_ID(N'[dbo].[FK_ServiceApplicationServiceApplicationAudit]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ServiceApplicationAudit] DROP CONSTRAINT [FK_ServiceApplicationServiceApplicationAudit];
GO
IF (SELECT v FROM #Version) < 1010000 AND OBJECT_ID(N'[dbo].[FK_ServiceApplicationServiceApplicationPerfomance]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ServiceApplicationPerfomance] DROP CONSTRAINT [FK_ServiceApplicationServiceApplicationPerfomance];
GO
IF (SELECT v FROM #Version) < 1010000 AND OBJECT_ID(N'[dbo].[FK_ServiceApplicationHistoryServiceApplication]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ServiceApplicationHistory] DROP CONSTRAINT [FK_ServiceApplicationHistoryServiceApplication];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF (SELECT v FROM #Version) < 1010000 AND OBJECT_ID(N'[dbo].[ServiceApplication]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ServiceApplication];
GO
IF (SELECT v FROM #Version) < 1010000 AND OBJECT_ID(N'[dbo].[ServiceApplicationAudit]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ServiceApplicationAudit];
GO
IF (SELECT v FROM #Version) < 1010000 AND OBJECT_ID(N'[dbo].[SystemAudit]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SystemAudit];
GO
IF (SELECT v FROM #Version) < 1010000 AND OBJECT_ID(N'[dbo].[ServiceApplicationPerfomance]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ServiceApplicationPerfomance];
GO
IF (SELECT v FROM #Version) < 1010000 AND OBJECT_ID(N'[dbo].[ServiceApplicationHistory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ServiceApplicationHistory];
GO
IF (SELECT v FROM #Version) < 1010000 AND OBJECT_ID(N'[dbo].[SacsVersionSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SacsVersionSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

IF (SELECT v FROM #Version) < 1010000
-- Creating table 'ServiceApplication'
CREATE TABLE [dbo].[ServiceApplication] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [Environment] nvarchar(max)  NULL,
    [AppFilePath] nvarchar(max)  NOT NULL,
    [CronSchedule] nvarchar(max)  NOT NULL,
    [Active] bit  NOT NULL,
    [CreatedByUserId] nvarchar(max)  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [ModifiedByUserId] nvarchar(max)  NULL,
    [ModifiedDate] datetime  NULL,
    [StartupType] int  NOT NULL,
    [SendSuccessNotification] bit  NOT NULL,
    [EntropyValue2] nvarchar(max)  NULL,
    [Parameters] nvarchar(max)  NULL
);
GO

IF (SELECT v FROM #Version) < 1010000
-- Creating table 'ServiceApplicationAudit'
CREATE TABLE [dbo].[ServiceApplicationAudit] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Message] nvarchar(max)  NULL,
    [CreatedByUserId] nvarchar(max)  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [ServiceApplicationId] int  NOT NULL,
    [AuditType] int  NOT NULL
);
GO

IF (SELECT v FROM #Version) < 1010000
-- Creating table 'SystemAudit'
CREATE TABLE [dbo].[SystemAudit] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Message] nvarchar(max)  NOT NULL,
    [CreatedByUserId] nvarchar(max)  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [CpuCounter] decimal(18,4)  NULL,
    [RamCounter] decimal(18,4)  NULL,
    [AuditType] int  NOT NULL
);
GO

IF (SELECT v FROM #Version) < 1010000
-- Creating table 'ServiceApplicationPerfomance'
CREATE TABLE [dbo].[ServiceApplicationPerfomance] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [StartTime] datetime  NOT NULL,
    [EndTime] datetime  NULL,
    [Message] nvarchar(max)  NULL,
    [Source] nvarchar(max)  NOT NULL,
    [ServiceApplicationId] int  NOT NULL,
    [Guid] nvarchar(max)  NOT NULL,
    [Failed] bit  NOT NULL
);
GO

IF (SELECT v FROM #Version) < 1010000
-- Creating table 'ServiceApplicationHistory'
CREATE TABLE [dbo].[ServiceApplicationHistory] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(max)  NULL,
    [Environment] nvarchar(max)  NULL,
    [AppFilePath] nvarchar(max)  NOT NULL,
    [CronSchedule] nvarchar(max)  NOT NULL,
    [Active] bit  NOT NULL,
    [CreatedByUserId] nvarchar(max)  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [ServiceApplicationId] int  NULL
);
GO

IF (SELECT v FROM #Version) < 1010000
-- Creating table 'SacsVersion'
CREATE TABLE [dbo].[SacsVersion] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [VersionNumber] nvarchar(max)  NOT NULL,
    [InstallationDate] datetime  NOT NULL,
    [Description] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

IF (SELECT v FROM #Version) < 1010000
-- Creating primary key on [Id] in table 'ServiceApplication'
ALTER TABLE [dbo].[ServiceApplication]
ADD CONSTRAINT [PK_ServiceApplication]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

IF (SELECT v FROM #Version) < 1010000
-- Creating primary key on [Id] in table 'ServiceApplicationAudit'
ALTER TABLE [dbo].[ServiceApplicationAudit]
ADD CONSTRAINT [PK_ServiceApplicationAudit]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

IF (SELECT v FROM #Version) < 1010000
-- Creating primary key on [Id] in table 'SystemAudit'
ALTER TABLE [dbo].[SystemAudit]
ADD CONSTRAINT [PK_SystemAudit]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

IF (SELECT v FROM #Version) < 1010000
-- Creating primary key on [Id] in table 'ServiceApplicationPerfomance'
ALTER TABLE [dbo].[ServiceApplicationPerfomance]
ADD CONSTRAINT [PK_ServiceApplicationPerfomance]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

IF (SELECT v FROM #Version) < 1010000
-- Creating primary key on [Id] in table 'ServiceApplicationHistory'
ALTER TABLE [dbo].[ServiceApplicationHistory]
ADD CONSTRAINT [PK_ServiceApplicationHistory]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

IF (SELECT v FROM #Version) < 1010000
-- Creating primary key on [Id] in table 'SacsVersion'
ALTER TABLE [dbo].[SacsVersion]
ADD CONSTRAINT [PK_SacsVersion]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

IF (SELECT v FROM #Version) < 1010000
-- Creating foreign key on [ServiceApplicationId] in table 'ServiceApplicationAudit'
ALTER TABLE [dbo].[ServiceApplicationAudit]
ADD CONSTRAINT [FK_ServiceApplicationServiceApplicationAudit]
    FOREIGN KEY ([ServiceApplicationId])
    REFERENCES [dbo].[ServiceApplication]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

IF (SELECT v FROM #Version) < 1010000
-- Creating non-clustered index for FOREIGN KEY 'FK_ServiceApplicationServiceApplicationAudit'
CREATE INDEX [IX_FK_ServiceApplicationServiceApplicationAudit]
ON [dbo].[ServiceApplicationAudit]
    ([ServiceApplicationId]);
GO

IF (SELECT v FROM #Version) < 1010000
-- Creating foreign key on [ServiceApplicationId] in table 'ServiceApplicationPerfomance'
ALTER TABLE [dbo].[ServiceApplicationPerfomance]
ADD CONSTRAINT [FK_ServiceApplicationServiceApplicationPerfomance]
    FOREIGN KEY ([ServiceApplicationId])
    REFERENCES [dbo].[ServiceApplication]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

IF (SELECT v FROM #Version) < 1010000
-- Creating non-clustered index for FOREIGN KEY 'FK_ServiceApplicationServiceApplicationPerfomance'
CREATE INDEX [IX_FK_ServiceApplicationServiceApplicationPerfomance]
ON [dbo].[ServiceApplicationPerfomance]
    ([ServiceApplicationId]);
GO

IF (SELECT v FROM #Version) < 1010000
-- Creating foreign key on [ServiceApplicationId] in table 'ServiceApplicationHistory'
ALTER TABLE [dbo].[ServiceApplicationHistory]
ADD CONSTRAINT [FK_ServiceApplicationHistoryServiceApplication]
    FOREIGN KEY ([ServiceApplicationId])
    REFERENCES [dbo].[ServiceApplication]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

IF (SELECT v FROM #Version) < 1010000
-- Creating non-clustered index for FOREIGN KEY 'FK_ServiceApplicationHistoryServiceApplication'
CREATE INDEX [IX_FK_ServiceApplicationHistoryServiceApplication]
ON [dbo].[ServiceApplicationHistory]
    ([ServiceApplicationId]);
GO

IF (SELECT v FROM #Version) < 1010000
INSERT INTO [dbo].[SacsVersion] ([VersionNumber], [InstallationDate], [Description])
VALUES ('1.1.0.0', GETDATE(), 'Adds version table and upgrades security.')
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------
Finished: