CREATE TABLE [dbo].[Analyses]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[UseIdentifier] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[IdentifierList] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[IdentifierFile] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[StudyName] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[StudyDescription] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AnalysisName] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[AnalysisDescription] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[InterventionStartDate] [datetime] NULL,
[InterventionEndDate] [datetime] NULL,
[InterventionGradeLevels] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[InterventionAreas] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[OutcomeAreas] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[SubgroupAnalyses] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[GeneratedOn] [datetime] NULL,
[StatusId] [int] NULL,
[CreatedOn] [datetime] NULL,
[JobGUID] [uniqueidentifier] NULL,
[UserName] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[State] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[YearsOfInterest] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[UserEmail] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DistrictMatch] [int] NOT NULL CONSTRAINT [DF_Analyses_DistrictMatch] DEFAULT ((0))
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Analyses] ADD CONSTRAINT [PK_Analyses] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
