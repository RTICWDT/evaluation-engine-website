CREATE ROLE [aspnet_Personalization_ReportingAccess]
AUTHORIZATION [dbo]
GO
EXEC sp_addrolemember N'aspnet_Personalization_ReportingAccess', N'aspnet_Personalization_FullAccess'
GO
