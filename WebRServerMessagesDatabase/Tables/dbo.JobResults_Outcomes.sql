CREATE TABLE [dbo].[JobResults_Outcomes]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[JobGUID] [uniqueidentifier] NOT NULL,
[OutcomeYAML] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[OutcomeName] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Rank] [int] NOT NULL,
[DataNote_ScaleScore] [varchar] (8000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[TableNote_ScaleScore] [varchar] (8000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Chart_ScaleScore] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Chart_PercentProficient] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[YAML_ScaleScore] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[YAML_PercentProficient] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Title] [nvarchar] (4000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DataNote_PercentProficient] [nvarchar] (4000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[TableNote_PercentProficient] [nvarchar] (4000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[OutcomeYear] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[OutcomeHeader] [nvarchar] (2000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[SummaryText_ScaleScore] [nvarchar] (4000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[SummaryTest_PercentProficient] [nvarchar] (4000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Title_PercentProficiency] [nvarchar] (4000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Title_ScaleScore] [nvarchar] (4000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[TestName] [nvarchar] (4000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[OutcomeNote] [nvarchar] (4000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ChartValues_ScaleScore_Intervention] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ChartValues_ScaleScore_Control] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ChartValues_PercentProficient_Intervention] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ChartValues_PercentProficient_Control] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ErrorMessage] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Messages] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Success] [bit] NULL,
[PerGrade_Averages_YAML] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ChartValues_Main_Difference] [float] NULL,
[ChartValues_Proficiency_Difference] [float] NULL,
[OrdinalAverages] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[SubgroupNote] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[JobResults_Outcomes] ADD CONSTRAINT [PK_JobResults_Outcomes] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[JobResults_Outcomes] ADD CONSTRAINT [FK_JobResults_Outcomes_Jobs] FOREIGN KEY ([JobGUID]) REFERENCES [dbo].[Jobs] ([JobGUID])
GO
