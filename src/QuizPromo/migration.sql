IF OBJECT_ID(N'__EFMigrationsHistory') IS NULL
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK_HistoryRow] PRIMARY KEY ([MigrationId])
    );

GO

CREATE TABLE [Category] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Category] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Test] (
    [Id] int NOT NULL IDENTITY,
    CONSTRAINT [PK_Test] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [User] (
    [Id] int NOT NULL IDENTITY,
    [FullName] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Question] (
    [Id] int NOT NULL IDENTITY,
    [CategoryId] int NOT NULL,
    [Created] datetimeoffset NOT NULL,
    [Discriminator] nvarchar(max) NOT NULL,
    [IsDeleted] bit NOT NULL,
    [Name] nvarchar(max),
    [Statement] nvarchar(max) NOT NULL,
    [TestId] int,
    [Updated] datetimeoffset NOT NULL,
    CONSTRAINT [PK_Question] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Question_Category_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Category] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Question_Test_TestId] FOREIGN KEY ([TestId]) REFERENCES [Test] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [AnswerVariant] (
    [Id] int NOT NULL IDENTITY,
    [QuestionId] int NOT NULL,
    [Value] nvarchar(max),
    CONSTRAINT [PK_AnswerVariant] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AnswerVariant_ChoiceQuestion_QuestionId] FOREIGN KEY ([QuestionId]) REFERENCES [Question] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Session] (
    [Id] int NOT NULL IDENTITY,
    [BeginDate] datetimeoffset NOT NULL,
    [CurrentQuestionId] int,
    [EndDate] datetimeoffset NOT NULL,
    [IsCompleted] bit NOT NULL,
    [TestId] int NOT NULL,
    [UserId] int NOT NULL,
    CONSTRAINT [PK_Session] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Session_Question_CurrentQuestionId] FOREIGN KEY ([CurrentQuestionId]) REFERENCES [Question] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Session_Test_TestId] FOREIGN KEY ([TestId]) REFERENCES [Test] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Session_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [User] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Answer] (
    [Id] int NOT NULL IDENTITY,
    [Discriminator] nvarchar(max) NOT NULL,
    [IsCorrect] bit NOT NULL,
    [QuestionId] int,
    [SessionId] int,
    [Choice] nvarchar(max),
    CONSTRAINT [PK_Answer] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Answer_Question_QuestionId] FOREIGN KEY ([QuestionId]) REFERENCES [Question] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Answer_Session_SessionId] FOREIGN KEY ([SessionId]) REFERENCES [Session] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [SessionQuestion] (
    [QuestionId] int NOT NULL,
    [SessionId] int NOT NULL,
    CONSTRAINT [PK_SessionQuestion] PRIMARY KEY ([QuestionId], [SessionId]),
    CONSTRAINT [FK_SessionQuestion_Question_QuestionId] FOREIGN KEY ([QuestionId]) REFERENCES [Question] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_SessionQuestion_Session_SessionId] FOREIGN KEY ([SessionId]) REFERENCES [Session] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [SingleAnswerInMultipleAnswer] (
    [SingleAnswerId] int NOT NULL,
    [MultipleAnswerId] int NOT NULL,
    CONSTRAINT [PK_SingleAnswerInMultipleAnswer] PRIMARY KEY ([SingleAnswerId], [MultipleAnswerId]),
    CONSTRAINT [FK_SingleAnswerInMultipleAnswer_MultipleAnswer_MultipleAnswerId] FOREIGN KEY ([MultipleAnswerId]) REFERENCES [Answer] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_SingleAnswerInMultipleAnswer_SingleAnswer_SingleAnswerId] FOREIGN KEY ([SingleAnswerId]) REFERENCES [Answer] ([Id]) ON DELETE NO ACTION
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20160209143107_Init', N'7.0.0-rc1-16348');

GO

