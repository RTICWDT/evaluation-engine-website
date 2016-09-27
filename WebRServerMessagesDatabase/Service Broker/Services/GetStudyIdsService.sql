CREATE SERVICE [GetStudyIdsService]
AUTHORIZATION [dbo]
ON QUEUE [dbo].[GetStudyIdsQueue]
(
[GetStudyIdsContract]
)
GO
