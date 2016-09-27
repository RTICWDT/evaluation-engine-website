CREATE TABLE [dbo].[aspnet_Users]
(
[ApplicationId] [uniqueidentifier] NOT NULL,
[UserId] [uniqueidentifier] NOT NULL CONSTRAINT [DF__aspnet_Us__UserI__6B24EA82] DEFAULT (newid()),
[UserName] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LoweredUserName] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MobileAlias] [nvarchar] (16) COLLATE SQL_Latin1_General_CP1_CI_AS NULL CONSTRAINT [DF__aspnet_Us__Mobil__6C190EBB] DEFAULT (NULL),
[IsAnonymous] [bit] NOT NULL CONSTRAINT [DF__aspnet_Us__IsAno__6D0D32F4] DEFAULT ((0)),
[LastActivityDate] [datetime] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[aspnet_Users] ADD CONSTRAINT [PK__aspnet_Users__693CA210] PRIMARY KEY NONCLUSTERED  ([UserId]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [aspnet_Users_Index2] ON [dbo].[aspnet_Users] ([ApplicationId], [LastActivityDate]) ON [PRIMARY]
GO
CREATE UNIQUE CLUSTERED INDEX [aspnet_Users_Index] ON [dbo].[aspnet_Users] ([ApplicationId], [LoweredUserName]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[aspnet_Users] ADD CONSTRAINT [FK__aspnet_Us__Appli__6A30C649] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[aspnet_Applications] ([ApplicationId])
GO
ALTER TABLE [dbo].[aspnet_Users] ADD CONSTRAINT [FK_aspnet_Users_aspnet_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
