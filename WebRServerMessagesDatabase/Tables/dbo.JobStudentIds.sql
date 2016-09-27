CREATE TABLE [dbo].[JobStudentIds]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[JobGUID] [uniqueidentifier] NOT NULL,
[StudentId] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[JobStudentIds] ADD CONSTRAINT [PK_JobStudentIds] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[JobStudentIds] WITH NOCHECK ADD CONSTRAINT [FK_JobStudentIds_Jobs] FOREIGN KEY ([JobGUID]) REFERENCES [dbo].[Jobs] ([JobGUID])
GO
ALTER TABLE [dbo].[JobStudentIds] NOCHECK CONSTRAINT [FK_JobStudentIds_Jobs]
GO
