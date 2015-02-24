CREATE TABLE [dbo].[AuditTypes] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (MAX) NOT NULL,
    [Description] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_AuditTypes] PRIMARY KEY CLUSTERED ([Id] ASC)
);

