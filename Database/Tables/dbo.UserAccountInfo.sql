CREATE TABLE [dbo].[UserAccountInfo]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[ResetToken] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ResetTime] [datetime] NULL,
[ResetFlag] [bit] NULL,
[VerifyToken] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[VerifyTime] [datetime] NULL,
[VerifyFlag] [bit] NULL,
[UserId] [uniqueidentifier] NOT NULL,
[Salt] [nvarchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[UserAccountInfo] ADD CONSTRAINT [PK_UserAccountInfo] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[UserAccountInfo] ADD CONSTRAINT [FK_UserAccountInfo_UserAccountInfo] FOREIGN KEY ([UserId]) REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
