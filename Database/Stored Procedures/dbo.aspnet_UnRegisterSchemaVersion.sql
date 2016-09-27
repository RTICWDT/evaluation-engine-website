SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE [dbo].aspnet_UnRegisterSchemaVersion
    @Feature                   nvarchar(128),
    @CompatibleSchemaVersion   nvarchar(128)
AS
BEGIN
    DELETE FROM dbo.aspnet_SchemaVersions
        WHERE   Feature = LOWER(@Feature) AND @CompatibleSchemaVersion = CompatibleSchemaVersion
END
GO
GRANT EXECUTE ON  [dbo].[aspnet_UnRegisterSchemaVersion] TO [aspnet_Membership_BasicAccess]
GRANT EXECUTE ON  [dbo].[aspnet_UnRegisterSchemaVersion] TO [aspnet_Membership_ReportingAccess]
GRANT EXECUTE ON  [dbo].[aspnet_UnRegisterSchemaVersion] TO [aspnet_Personalization_BasicAccess]
GRANT EXECUTE ON  [dbo].[aspnet_UnRegisterSchemaVersion] TO [aspnet_Personalization_ReportingAccess]
GRANT EXECUTE ON  [dbo].[aspnet_UnRegisterSchemaVersion] TO [aspnet_Profile_BasicAccess]
GRANT EXECUTE ON  [dbo].[aspnet_UnRegisterSchemaVersion] TO [aspnet_Profile_ReportingAccess]
GRANT EXECUTE ON  [dbo].[aspnet_UnRegisterSchemaVersion] TO [aspnet_Roles_BasicAccess]
GRANT EXECUTE ON  [dbo].[aspnet_UnRegisterSchemaVersion] TO [aspnet_Roles_ReportingAccess]
GRANT EXECUTE ON  [dbo].[aspnet_UnRegisterSchemaVersion] TO [aspnet_WebEvent_FullAccess]
GO
