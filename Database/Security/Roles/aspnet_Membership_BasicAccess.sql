CREATE ROLE [aspnet_Membership_BasicAccess]
AUTHORIZATION [dbo]
GO
EXEC sp_addrolemember N'aspnet_Membership_BasicAccess', N'aspnet_Membership_FullAccess'
GO
