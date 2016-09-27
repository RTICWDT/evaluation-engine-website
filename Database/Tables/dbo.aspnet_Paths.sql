CREATE TABLE [dbo].[aspnet_Paths]
(
[ApplicationId] [uniqueidentifier] NOT NULL,
[PathId] [uniqueidentifier] NOT NULL CONSTRAINT [DF__aspnet_Pa__PathI__2CF2ADDF] DEFAULT (newid()),
[Path] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LoweredPath] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[aspnet_Paths] ADD CONSTRAINT [PK__aspnet_Paths__2B0A656D] PRIMARY KEY NONCLUSTERED  ([PathId]) ON [PRIMARY]
GO
CREATE UNIQUE CLUSTERED INDEX [aspnet_Paths_index] ON [dbo].[aspnet_Paths] ([ApplicationId], [LoweredPath]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[aspnet_Paths] ADD CONSTRAINT [FK__aspnet_Pa__Appli__2BFE89A6] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[aspnet_Applications] ([ApplicationId])
GO
