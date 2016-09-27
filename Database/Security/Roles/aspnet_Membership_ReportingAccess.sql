CREATE ROLE [aspnet_Membership_ReportingAccess]
AUTHORIZATION [dbo]
GO
EXEC sp_addrolemember N'aspnet_Membership_ReportingAccess', N'aspnet_Membership_FullAccess'
GO
