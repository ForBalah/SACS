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
-- Creating primary key on [Id] in table 'ServiceApplicationAudit'
ALTER TABLE [dbo].[ServiceApplicationAudit]
ADD CONSTRAINT [PK_ServiceApplicationAudit]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO
-- Creating non-clustered index for FOREIGN KEY 'FK_ServiceApplicationServiceApplicationAudit'
CREATE INDEX [IX_FK_ServiceApplicationServiceApplicationAudit]
ON [dbo].[ServiceApplicationAudit]
    ([ServiceApplicationId]);