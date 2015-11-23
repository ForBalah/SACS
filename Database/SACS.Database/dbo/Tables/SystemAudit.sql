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
-- Creating primary key on [Id] in table 'SystemAudit'
ALTER TABLE [dbo].[SystemAudit]
ADD CONSTRAINT [PK_SystemAudit]
    PRIMARY KEY CLUSTERED ([Id] ASC);