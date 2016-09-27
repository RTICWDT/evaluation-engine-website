SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
CREATE PROCEDURE dbo.aspnet_Personalization_GetApplicationId (
    @ApplicationName NVARCHAR(256),
    @ApplicationId UNIQUEIDENTIFIER OUT)
AS
BEGIN
    SELECT @ApplicationId = ApplicationId FROM dbo.aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
END
GO
GRANT EXECUTE ON  [dbo].[aspnet_Personalization_GetApplicationId] TO [aspnet_Personalization_BasicAccess]
GO
