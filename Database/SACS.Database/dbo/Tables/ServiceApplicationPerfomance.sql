-- Creating table 'ServiceApplicationPerfomance'
CREATE TABLE [dbo].[ServiceApplicationPerfomance] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [StartTime] datetime  NOT NULL,
    [EndTime] datetime  NULL,
    [Message] nvarchar(max)  NULL,
    [Source] nvarchar(max)  NOT NULL,
    [ServiceApplicationId] int  NOT NULL,
    [Guid] nvarchar(max)  NOT NULL
);
GO
-- Creating foreign key on [ServiceApplicationId] in table 'ServiceApplicationPerfomance'
ALTER TABLE [dbo].[ServiceApplicationPerfomance]
ADD CONSTRAINT [FK_ServiceApplicationServiceApplicationPerfomance]
    FOREIGN KEY ([ServiceApplicationId])
    REFERENCES [dbo].[ServiceApplication]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO
-- Creating primary key on [Id] in table 'ServiceApplicationPerfomance'
ALTER TABLE [dbo].[ServiceApplicationPerfomance]
ADD CONSTRAINT [PK_ServiceApplicationPerfomance]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO
-- Creating non-clustered index for FOREIGN KEY 'FK_ServiceApplicationServiceApplicationPerfomance'
CREATE INDEX [IX_FK_ServiceApplicationServiceApplicationPerfomance]
ON [dbo].[ServiceApplicationPerfomance]
    ([ServiceApplicationId]);