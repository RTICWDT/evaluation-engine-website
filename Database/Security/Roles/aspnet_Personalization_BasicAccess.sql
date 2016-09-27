CREATE ROLE [aspnet_Personalization_BasicAccess]
AUTHORIZATION [dbo]
GO
EXEC sp_addrolemember N'aspnet_Personalization_BasicAccess', N'aspnet_Personalization_FullAccess'
GO
