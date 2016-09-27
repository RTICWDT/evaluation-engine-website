CREATE TABLE [dbo].[States]
(
[StateId] [int] NOT NULL IDENTITY(1, 1),
[StateAbbrev] [varchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FullName] [varchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IncludedInProject] [bit] NOT NULL CONSTRAINT [DF_States_IncludedInProject] DEFAULT ((0)),
[StateIdText] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DataSource] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HelpfulText] [nvarchar] (2000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DataDescription] [nvarchar] (2000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[States] ADD CONSTRAINT [PK_STATE] PRIMARY KEY CLUSTERED  ([StateId]) ON [PRIMARY]
GO
