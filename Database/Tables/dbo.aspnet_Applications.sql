CREATE TABLE [dbo].[aspnet_Applications]
(
[ApplicationName] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LoweredApplicationName] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ApplicationId] [uniqueidentifier] NOT NULL CONSTRAINT [DF__aspnet_Ap__Appli__6754599E] DEFAULT (newid()),
[Description] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[aspnet_Applications] ADD CONSTRAINT [PK__aspnet_Applicati__6477ECF3] PRIMARY KEY NONCLUSTERED  ([ApplicationId]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[aspnet_Applications] ADD CONSTRAINT [UQ__aspnet_Applicati__66603565] UNIQUE NONCLUSTERED  ([ApplicationName]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[aspnet_Applications] ADD CONSTRAINT [UQ__aspnet_Applicati__656C112C] UNIQUE NONCLUSTERED  ([LoweredApplicationName]) ON [PRIMARY]
GO
CREATE CLUSTERED INDEX [aspnet_Applications_Index] ON [dbo].[aspnet_Applications] ([LoweredApplicationName]) ON [PRIMARY]
GO
