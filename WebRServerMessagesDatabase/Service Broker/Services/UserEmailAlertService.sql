CREATE SERVICE [UserEmailAlertService]
AUTHORIZATION [dbo]
ON QUEUE [dbo].[UserEmailAlertQueue]
(
[UserEmailAlertContract]
)
GO
