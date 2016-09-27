SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

  CREATE VIEW [dbo].[vw_aspnet_Roles]
  AS SELECT [dbo].[aspnet_Roles].[ApplicationId], [dbo].[aspnet_Roles].[RoleId], [dbo].[aspnet_Roles].[RoleName], [dbo].[aspnet_Roles].[LoweredRoleName], [dbo].[aspnet_Roles].[Description]
  FROM [dbo].[aspnet_Roles]
  
GO
GRANT SELECT ON  [dbo].[vw_aspnet_Roles] TO [aspnet_Roles_ReportingAccess]
GO
