CREATE ROLE [aspnet_Membership_FullAccess]
AUTHORIZATION [dbo]
EXEC sp_addrolemember N'aspnet_Membership_FullAccess', N'EvalEngineUser'

GO
