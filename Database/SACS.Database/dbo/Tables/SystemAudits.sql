CREATE TABLE [dbo].[SystemAudits] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [Message]         NVARCHAR (MAX) NOT NULL,
    [CreatedByUserId] NVARCHAR (MAX) NOT NULL,
    [CreatedDate]     DATETIME       NOT NULL,
	[CpuCounter]      DECIMAL (18,4)   NULL,
    [RamCounter]      DECIMAL (18,4)   NULL,
    [AuditTypeId]     INT            NOT NULL,
    CONSTRAINT [PK_SystemAudits] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AuditTypeSystemAudit] FOREIGN KEY ([AuditTypeId]) REFERENCES [dbo].[AuditTypes] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_AuditTypeSystemAudit]
    ON [dbo].[SystemAudits]([AuditTypeId] ASC);

