CREATE TABLE [dbo].[ServiceApplicationAudits] (
    [Id]                   INT            IDENTITY (1, 1) NOT NULL,
    [Message]              NVARCHAR (MAX) NULL,
    [CreatedByUserId]      NVARCHAR (MAX) NOT NULL,
    [CreatedDate]          DATETIME       NOT NULL,
    [AuditTypeId]          INT            NOT NULL,
    [ServiceApplicationId] INT            NOT NULL,
    CONSTRAINT [PK_ServiceApplicationAudits] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AuditTypeServiceApplicationAudit] FOREIGN KEY ([AuditTypeId]) REFERENCES [dbo].[AuditTypes] ([Id]),
    CONSTRAINT [FK_ServiceApplicationServiceApplicationAudit] FOREIGN KEY ([ServiceApplicationId]) REFERENCES [dbo].[ServiceApplications] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_AuditTypeServiceApplicationAudit]
    ON [dbo].[ServiceApplicationAudits]([AuditTypeId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FK_ServiceApplicationServiceApplicationAudit]
    ON [dbo].[ServiceApplicationAudits]([ServiceApplicationId] ASC);

