CREATE TABLE [dbo].[PasswordHistory]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[Password] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LastUsed] [datetime] NOT NULL,
[Username] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[UserId] [uniqueidentifier] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PasswordHistory] ADD CONSTRAINT [PK_PasswordHistory] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PasswordHistory] ADD CONSTRAINT [FK_PasswordHistory_aspnet_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
