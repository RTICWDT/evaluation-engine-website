SET IDENTITY_INSERT [dbo].[JobStatuses] ON
INSERT INTO [dbo].[JobStatuses] ([Id], [Name]) VALUES (5, N'READY')
SET IDENTITY_INSERT [dbo].[JobStatuses] OFF
SET IDENTITY_INSERT [dbo].[JobStatuses] ON
INSERT INTO [dbo].[JobStatuses] ([Id], [Name]) VALUES (1, N'CREATED')
INSERT INTO [dbo].[JobStatuses] ([Id], [Name]) VALUES (2, N'RUNNING')
INSERT INTO [dbo].[JobStatuses] ([Id], [Name]) VALUES (3, N'DONE')
INSERT INTO [dbo].[JobStatuses] ([Id], [Name]) VALUES (4, N'ERROR')
SET IDENTITY_INSERT [dbo].[JobStatuses] OFF
