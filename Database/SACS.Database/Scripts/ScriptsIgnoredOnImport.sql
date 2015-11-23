
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/21/2015 11:24:29
-- Generated from EDMX file: E:\Development\SACS\Head\Libraries\SACS.DataAccessLayer\Entitites\SACSEntities.edmx
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


-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------

GO
