CREATE TABLE [dbo].[YearsOfInterest]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Label] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Value] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[StateAbbv] [varchar] (3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DefaultStartDate] [date] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[YearsOfInterest] ADD CONSTRAINT [PK_YearsOfInterest] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
