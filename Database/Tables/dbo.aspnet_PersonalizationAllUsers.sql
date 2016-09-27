CREATE TABLE [dbo].[aspnet_PersonalizationAllUsers]
(
[PathId] [uniqueidentifier] NOT NULL,
[PageSettings] [image] NOT NULL,
[LastUpdatedDate] [datetime] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[aspnet_PersonalizationAllUsers] ADD CONSTRAINT [PK__aspnet_Personali__30C33EC3] PRIMARY KEY CLUSTERED  ([PathId]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[aspnet_PersonalizationAllUsers] ADD CONSTRAINT [FK__aspnet_Pe__PathI__31B762FC] FOREIGN KEY ([PathId]) REFERENCES [dbo].[aspnet_Paths] ([PathId])
GO
