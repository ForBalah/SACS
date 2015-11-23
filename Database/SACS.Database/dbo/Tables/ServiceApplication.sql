-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'ServiceApplication'
CREATE TABLE [dbo].[ServiceApplication] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [Environment] nvarchar(max)  NULL,
    [AppFilePath] nvarchar(max)  NOT NULL,
    [CronSchedule] nvarchar(max)  NOT NULL,
    [Active] bit  NOT NULL,
    [CreatedByUserId] nvarchar(max)  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [ModifiedByUserId] nvarchar(max)  NULL,
    [ModifiedDate] datetime  NULL,
    [StartupType] int  NOT NULL,
    [SendSuccessNotification] bit  NOT NULL
);
GO
-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'ServiceApplication'
ALTER TABLE [dbo].[ServiceApplication]
ADD CONSTRAINT [PK_ServiceApplication]
    PRIMARY KEY CLUSTERED ([Id] ASC);