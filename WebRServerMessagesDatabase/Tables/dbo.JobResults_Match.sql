CREATE TABLE [dbo].[JobResults_Match]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[JobGUID] [uniqueidentifier] NOT NULL,
[StudyId] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MatchedSet] [int] NOT NULL,
[TreatmentStatus] [int] NOT NULL,
[MainPropensityScore] [float] NOT NULL,
[InclusivePropensityScore] [float] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[JobResults_Match] ADD CONSTRAINT [PK_JobResults_Match] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[JobResults_Match] ADD CONSTRAINT [FK_JobResults_Match_Jobs] FOREIGN KEY ([JobGUID]) REFERENCES [dbo].[Jobs] ([JobGUID])
GO
