CREATE TABLE [dbo].[JobMissingStudentIds]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[JobGUID] [uniqueidentifier] NOT NULL,
[StudentId] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[JobMissingStudentIds] ADD CONSTRAINT [PK_JobMissingStudentIds] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[JobMissingStudentIds] ADD CONSTRAINT [FK_JobMissingStudentIds_Jobs] FOREIGN KEY ([JobGUID]) REFERENCES [dbo].[Jobs] ([JobGUID])
GO
