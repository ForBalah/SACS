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
-- Creating foreign key on [ServiceApplicationId] in table 'ServiceApplicationHistory'
ALTER TABLE [dbo].[ServiceApplicationHistory]
ADD CONSTRAINT [FK_ServiceApplicationHistoryServiceApplication]
    FOREIGN KEY ([ServiceApplicationId])
    REFERENCES [dbo].[ServiceApplication]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
-- Creating primary key on [Id] in table 'ServiceApplicationHistory'
ALTER TABLE [dbo].[ServiceApplicationHistory]
ADD CONSTRAINT [PK_ServiceApplicationHistory]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO
-- Creating non-clustered index for FOREIGN KEY 'FK_ServiceApplicationHistoryServiceApplication'
CREATE INDEX [IX_FK_ServiceApplicationHistoryServiceApplication]
ON [dbo].[ServiceApplicationHistory]
    ([ServiceApplicationId]);