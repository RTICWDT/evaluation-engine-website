CREATE TABLE [dbo].[JobResults]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[YAML] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Image] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[JobGUID] [uniqueidentifier] NOT NULL,
[Title] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[IntroText] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[TreatmentCount] [int] NULL,
[ControlCount] [int] NULL,
[BalanceTable] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[SchoolModelBoxplot] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[InclusiveMainBoxplot] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[MainBoxplot] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Histogram] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[InterventionPlot] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ControlPlot] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ActivePlot] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[WithinDistrictMatchesPct] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[BalanceMainPval] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[BalanceInclusivePval] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[SupplementalInformation] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Messages] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[TreatmentExcludedCount] [int] NULL,
[BalancePlotSensitivity] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[TimeUser] [float] NULL,
[TimeSystem] [float] NULL,
[TimeElapsed] [float] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[JobResults] ADD CONSTRAINT [PK_JobResults] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[JobResults] ADD CONSTRAINT [FK_JobResults_Jobs] FOREIGN KEY ([JobGUID]) REFERENCES [dbo].[Jobs] ([JobGUID])
GO
