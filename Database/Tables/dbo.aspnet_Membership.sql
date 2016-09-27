CREATE TABLE [dbo].[aspnet_Membership]
(
[ApplicationId] [uniqueidentifier] NOT NULL,
[UserId] [uniqueidentifier] NOT NULL,
[Password] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PasswordFormat] [int] NOT NULL CONSTRAINT [DF__aspnet_Me__Passw__7C4F7684] DEFAULT ((0)),
[PasswordSalt] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MobilePIN] [nvarchar] (16) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Email] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[LoweredEmail] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PasswordQuestion] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PasswordAnswer] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[IsApproved] [bit] NOT NULL,
[IsLockedOut] [bit] NOT NULL,
[CreateDate] [datetime] NOT NULL,
[LastLoginDate] [datetime] NOT NULL,
[LastPasswordChangedDate] [datetime] NOT NULL,
[LastLockoutDate] [datetime] NOT NULL,
[FailedPasswordAttemptCount] [int] NOT NULL,
[FailedPasswordAttemptWindowStart] [datetime] NOT NULL,
[FailedPasswordAnswerAttemptCount] [int] NOT NULL,
[FailedPasswordAnswerAttemptWindowStart] [datetime] NOT NULL,
[Comment] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[aspnet_Membership] ADD CONSTRAINT [PK__aspnet_Membershi__797309D9] PRIMARY KEY NONCLUSTERED  ([UserId]) ON [PRIMARY]
GO
CREATE CLUSTERED INDEX [aspnet_Membership_index] ON [dbo].[aspnet_Membership] ([ApplicationId], [LoweredEmail]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[aspnet_Membership] ADD CONSTRAINT [FK__aspnet_Me__Appli__7A672E12] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[aspnet_Applications] ([ApplicationId])
GO
ALTER TABLE [dbo].[aspnet_Membership] ADD CONSTRAINT [FK__aspnet_Me__UserI__7B5B524B] FOREIGN KEY ([UserId]) REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
