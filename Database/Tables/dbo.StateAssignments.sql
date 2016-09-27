CREATE TABLE [dbo].[StateAssignments]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[UserName] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[StateId] [int] NOT NULL,
[Role] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Year] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[StateAssignments] ADD CONSTRAINT [PK_StateAssignments] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[StateAssignments] ADD CONSTRAINT [FK_StateAssignments_States] FOREIGN KEY ([StateId]) REFERENCES [dbo].[States] ([StateId])
GO
