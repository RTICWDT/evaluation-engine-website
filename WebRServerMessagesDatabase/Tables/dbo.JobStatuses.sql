CREATE TABLE [dbo].[JobStatuses]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Name] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]
ALTER TABLE [dbo].[JobStatuses] ADD 
CONSTRAINT [PK_JobStatuses] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
