
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/23/2016 12:00:04
-- Generated from EDMX file: C:\Development\Projects\Open Box\SACS\Libraries\SACS.DataAccessLayer\Entitites\SACSEntities.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_ServiceApplicationServiceApplicationAudit]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ServiceApplicationAudit] DROP CONSTRAINT [FK_ServiceApplicationServiceApplicationAudit];
GO
IF OBJECT_ID(N'[dbo].[FK_ServiceApplicationServiceApplicationPerfomance]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ServiceApplicationPerfomance] DROP CONSTRAINT [FK_ServiceApplicationServiceApplicationPerfomance];
GO
IF OBJECT_ID(N'[dbo].[FK_ServiceApplicationHistoryServiceApplication]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ServiceApplicationHistory] DROP CONSTRAINT [FK_ServiceApplicationHistoryServiceApplication];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[ServiceApplication]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ServiceApplication];
GO
IF OBJECT_ID(N'[dbo].[ServiceApplicationAudit]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ServiceApplicationAudit];
GO
IF OBJECT_ID(N'[dbo].[SystemAudit]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SystemAudit];
GO
IF OBJECT_ID(N'[dbo].[ServiceApplicationPerfomance]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ServiceApplicationPerfomance];
GO
IF OBJECT_ID(N'[dbo].[ServiceApplicationHistory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ServiceApplicationHistory];
GO
IF OBJECT_ID(N'[dbo].[SacsVersionSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SacsVersionSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

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

-- Creating primary key on [Id] in table 'ServiceApplication'
ALTER TABLE [dbo].[ServiceApplication]
ADD CONSTRAINT [PK_ServiceApplication]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ServiceApplicationAudit'
ALTER TABLE [dbo].[ServiceApplicationAudit]
ADD CONSTRAINT [PK_ServiceApplicationAudit]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SystemAudit'
ALTER TABLE [dbo].[SystemAudit]
ADD CONSTRAINT [PK_SystemAudit]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ServiceApplicationPerfomance'
ALTER TABLE [dbo].[ServiceApplicationPerfomance]
ADD CONSTRAINT [PK_ServiceApplicationPerfomance]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ServiceApplicationHistory'
ALTER TABLE [dbo].[ServiceApplicationHistory]
ADD CONSTRAINT [PK_ServiceApplicationHistory]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SacsVersion'
ALTER TABLE [dbo].[SacsVersion]
ADD CONSTRAINT [PK_SacsVersion]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ServiceApplicationId] in table 'ServiceApplicationAudit'
ALTER TABLE [dbo].[ServiceApplicationAudit]
ADD CONSTRAINT [FK_ServiceApplicationServiceApplicationAudit]
    FOREIGN KEY ([ServiceApplicationId])
    REFERENCES [dbo].[ServiceApplication]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ServiceApplicationServiceApplicationAudit'
CREATE INDEX [IX_FK_ServiceApplicationServiceApplicationAudit]
ON [dbo].[ServiceApplicationAudit]
    ([ServiceApplicationId]);
GO

-- Creating foreign key on [ServiceApplicationId] in table 'ServiceApplicationPerfomance'
ALTER TABLE [dbo].[ServiceApplicationPerfomance]
ADD CONSTRAINT [FK_ServiceApplicationServiceApplicationPerfomance]
    FOREIGN KEY ([ServiceApplicationId])
    REFERENCES [dbo].[ServiceApplication]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ServiceApplicationServiceApplicationPerfomance'
CREATE INDEX [IX_FK_ServiceApplicationServiceApplicationPerfomance]
ON [dbo].[ServiceApplicationPerfomance]
    ([ServiceApplicationId]);
GO

-- Creating foreign key on [ServiceApplicationId] in table 'ServiceApplicationHistory'
ALTER TABLE [dbo].[ServiceApplicationHistory]
ADD CONSTRAINT [FK_ServiceApplicationHistoryServiceApplication]
    FOREIGN KEY ([ServiceApplicationId])
    REFERENCES [dbo].[ServiceApplication]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ServiceApplicationHistoryServiceApplication'
CREATE INDEX [IX_FK_ServiceApplicationHistoryServiceApplication]
ON [dbo].[ServiceApplicationHistory]
    ([ServiceApplicationId]);
GO

INSERT INTO [dbo].[SacsVersion] ([VersionNumber], [InstallationDate], [Description])
VALUES ('1.1.0.0', GETDATE(), 'Adds version table and upgrades security.')
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------