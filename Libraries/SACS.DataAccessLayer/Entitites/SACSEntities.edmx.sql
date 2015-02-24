
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 02/19/2015 08:58:07
-- Generated from EDMX file: E:\Development\SACS\Libraries\SACS.DataAccessLayer\Entitites\SACSEntities.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [SACS];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_ServiceApplicationServiceApplicationAudit]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ServiceApplicationAudits] DROP CONSTRAINT [FK_ServiceApplicationServiceApplicationAudit];
GO
IF OBJECT_ID(N'[dbo].[FK_ServiceApplicationServiceApplicationPerfomance]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ServiceApplicationPerfomances] DROP CONSTRAINT [FK_ServiceApplicationServiceApplicationPerfomance];
GO
IF OBJECT_ID(N'[dbo].[FK_ServiceApplicationHistoryServiceApplication]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ServiceApplicationHistories] DROP CONSTRAINT [FK_ServiceApplicationHistoryServiceApplication];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[ServiceApplications]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ServiceApplications];
GO
IF OBJECT_ID(N'[dbo].[ServiceApplicationAudits]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ServiceApplicationAudits];
GO
IF OBJECT_ID(N'[dbo].[SystemAudits]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SystemAudits];
GO
IF OBJECT_ID(N'[dbo].[ServiceApplicationPerfomances]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ServiceApplicationPerfomances];
GO
IF OBJECT_ID(N'[dbo].[ServiceApplicationHistories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ServiceApplicationHistories];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'ServiceApplications'
CREATE TABLE [dbo].[ServiceApplications] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [Environment] nvarchar(max)  NULL,
    [Path] nvarchar(max)  NOT NULL,
    [AssemblyName] nvarchar(max)  NOT NULL,
    [CronSchedule] nvarchar(max)  NOT NULL,
    [ConfigPath] nvarchar(max)  NOT NULL,
    [Active] bit  NOT NULL,
    [CreatedByUserId] nvarchar(max)  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [ModifiedByUserId] nvarchar(max)  NULL,
    [ModifiedDate] datetime  NULL,
    [StartupType] int  NOT NULL,
    [EntryFile] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'ServiceApplicationAudits'
CREATE TABLE [dbo].[ServiceApplicationAudits] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Message] nvarchar(max)  NULL,
    [CreatedByUserId] nvarchar(max)  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [ServiceApplicationId] int  NOT NULL,
    [AuditType] int  NOT NULL
);
GO

-- Creating table 'SystemAudits'
CREATE TABLE [dbo].[SystemAudits] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Message] nvarchar(max)  NOT NULL,
    [CreatedByUserId] nvarchar(max)  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [CpuCounter] decimal(18,4)  NULL,
    [RamCounter] decimal(18,4)  NULL,
    [AuditType] int  NOT NULL
);
GO

-- Creating table 'ServiceApplicationPerfomances'
CREATE TABLE [dbo].[ServiceApplicationPerfomances] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [StartTime] datetime  NOT NULL,
    [EndTime] datetime  NULL,
    [Message] nvarchar(max)  NULL,
    [Source] nvarchar(max)  NOT NULL,
    [ServiceApplicationId] int  NOT NULL
);
GO

-- Creating table 'ServiceApplicationHistories'
CREATE TABLE [dbo].[ServiceApplicationHistories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(max)  NULL,
    [Environment] nvarchar(max)  NULL,
    [Path] nvarchar(max)  NOT NULL,
    [AssemblyName] nvarchar(max)  NOT NULL,
    [CronSchedule] nvarchar(max)  NOT NULL,
    [ConfigPath] nvarchar(max)  NOT NULL,
    [Active] bit  NOT NULL,
    [CreatedByUserId] nvarchar(max)  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [ServiceApplicationId] int  NULL,
    [EntryFile] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'ServiceApplications'
ALTER TABLE [dbo].[ServiceApplications]
ADD CONSTRAINT [PK_ServiceApplications]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ServiceApplicationAudits'
ALTER TABLE [dbo].[ServiceApplicationAudits]
ADD CONSTRAINT [PK_ServiceApplicationAudits]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SystemAudits'
ALTER TABLE [dbo].[SystemAudits]
ADD CONSTRAINT [PK_SystemAudits]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ServiceApplicationPerfomances'
ALTER TABLE [dbo].[ServiceApplicationPerfomances]
ADD CONSTRAINT [PK_ServiceApplicationPerfomances]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ServiceApplicationHistories'
ALTER TABLE [dbo].[ServiceApplicationHistories]
ADD CONSTRAINT [PK_ServiceApplicationHistories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ServiceApplicationId] in table 'ServiceApplicationAudits'
ALTER TABLE [dbo].[ServiceApplicationAudits]
ADD CONSTRAINT [FK_ServiceApplicationServiceApplicationAudit]
    FOREIGN KEY ([ServiceApplicationId])
    REFERENCES [dbo].[ServiceApplications]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ServiceApplicationServiceApplicationAudit'
CREATE INDEX [IX_FK_ServiceApplicationServiceApplicationAudit]
ON [dbo].[ServiceApplicationAudits]
    ([ServiceApplicationId]);
GO

-- Creating foreign key on [ServiceApplicationId] in table 'ServiceApplicationPerfomances'
ALTER TABLE [dbo].[ServiceApplicationPerfomances]
ADD CONSTRAINT [FK_ServiceApplicationServiceApplicationPerfomance]
    FOREIGN KEY ([ServiceApplicationId])
    REFERENCES [dbo].[ServiceApplications]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ServiceApplicationServiceApplicationPerfomance'
CREATE INDEX [IX_FK_ServiceApplicationServiceApplicationPerfomance]
ON [dbo].[ServiceApplicationPerfomances]
    ([ServiceApplicationId]);
GO

-- Creating foreign key on [ServiceApplicationId] in table 'ServiceApplicationHistories'
ALTER TABLE [dbo].[ServiceApplicationHistories]
ADD CONSTRAINT [FK_ServiceApplicationHistoryServiceApplication]
    FOREIGN KEY ([ServiceApplicationId])
    REFERENCES [dbo].[ServiceApplications]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ServiceApplicationHistoryServiceApplication'
CREATE INDEX [IX_FK_ServiceApplicationHistoryServiceApplication]
ON [dbo].[ServiceApplicationHistories]
    ([ServiceApplicationId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------