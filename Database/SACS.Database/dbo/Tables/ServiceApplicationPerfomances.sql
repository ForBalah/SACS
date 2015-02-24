CREATE TABLE [dbo].[ServiceApplicationPerfomances] (
    [Id]                   INT            IDENTITY (1, 1) NOT NULL,
    [StartTime]            DATETIME       NOT NULL,
    [EndTime]              DATETIME       NULL,
    [Message]              NVARCHAR (MAX) NULL,
    [Source]               NVARCHAR (MAX) NOT NULL,
    [ServiceApplicationId] INT            NOT NULL,
    CONSTRAINT [PK_ServiceApplicationPerfomances] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ServiceApplicationServiceApplicationPerfomance] FOREIGN KEY ([ServiceApplicationId]) REFERENCES [dbo].[ServiceApplications] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_ServiceApplicationServiceApplicationPerfomance]
    ON [dbo].[ServiceApplicationPerfomances]([ServiceApplicationId] ASC);

