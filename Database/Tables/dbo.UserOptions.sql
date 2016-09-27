CREATE TABLE [dbo].[UserOptions]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Label] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Value] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[StateId] [int] NOT NULL,
[Section] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Rank] [int] NOT NULL,
[IsLabel] [bit] NOT NULL,
[IsHeader] [bit] NOT NULL,
[ParentId] [int] NOT NULL,
[FigureTitle] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[OutcomeTitle] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Subject] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DataSource] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[IsPercent] [bit] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[UserOptions] ADD CONSTRAINT [PK_UserOptions] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
