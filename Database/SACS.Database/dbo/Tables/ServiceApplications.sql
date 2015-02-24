CREATE TABLE [dbo].[ServiceApplications] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [Name]             NVARCHAR (MAX) NOT NULL,
    [Description]      NVARCHAR (MAX) NULL,
    [Environment]      NVARCHAR (MAX) NULL,
    [Path]             NVARCHAR (MAX) NOT NULL,
    [AssemblyName]     NVARCHAR (MAX) NOT NULL,
    [CronSchedule]     NVARCHAR (MAX) NOT NULL,
    [ConfigPath]       NVARCHAR (MAX) NOT NULL,
    [Active]           BIT            NOT NULL,
    [CreatedByUserId]  NVARCHAR (MAX) NOT NULL,
    [CreatedDate]      DATETIME       NOT NULL,
    [ModifiedByUserId] NVARCHAR (MAX) NULL,
    [ModifiedDate]     DATETIME       NULL,
    CONSTRAINT [PK_ServiceApplications] PRIMARY KEY CLUSTERED ([Id] ASC)
);

