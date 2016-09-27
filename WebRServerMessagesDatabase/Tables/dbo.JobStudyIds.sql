CREATE TABLE [dbo].[JobStudyIds]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[JobGUID] [uniqueidentifier] NOT NULL,
[StudyId] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[JobStudyIds] ADD CONSTRAINT [PK_JobStudyIds] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[JobStudyIds] ADD CONSTRAINT [FK_JobStudyIds_Jobs] FOREIGN KEY ([JobGUID]) REFERENCES [dbo].[Jobs] ([JobGUID])
GO
