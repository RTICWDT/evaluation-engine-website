SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

  CREATE VIEW [dbo].[vw_aspnet_WebPartState_User]
  AS SELECT [dbo].[aspnet_PersonalizationPerUser].[PathId], [dbo].[aspnet_PersonalizationPerUser].[UserId], [DataSize]=DATALENGTH([dbo].[aspnet_PersonalizationPerUser].[PageSettings]), [dbo].[aspnet_PersonalizationPerUser].[LastUpdatedDate]
  FROM [dbo].[aspnet_PersonalizationPerUser]
  
GO
GRANT SELECT ON  [dbo].[vw_aspnet_WebPartState_User] TO [aspnet_Personalization_ReportingAccess]
GO
