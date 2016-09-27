CREATE TABLE [dbo].[Jobs]
(
[Id] [int] NOT NULL IDENTITY(1, 1),
[StatusId] [int] NULL,
[InterventionStartDate] [date] NULL,
[InterventionEndDate] [date] NULL,
[InterventionGradeLevels] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[InterventionAreas] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[OutcomeMeasures] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[SubgroupAnalyses] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CreatedOn] [datetime] NULL,
[GeneratedOn] [datetime] NULL,
[JobGUID] [uniqueidentifier] NOT NULL,
[State] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ServerNotes] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[OutcomeYears] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ErrorMessages] [nvarchar] (2000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DistrictMatch] [int] NOT NULL CONSTRAINT [DF_Jobs_DistrictMatch] DEFAULT ((0))
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO



CREATE trigger [dbo].[EmailAlertTrigger]
on [dbo].[Jobs]
after update
as 
declare @h as uniqueIdentifier
declare @message varchar(1000)
declare @id int, @jobguid uniqueidentifier, @statusid int, @name nvarchar(15)
set @message = ''
select @id = ALTERED.Id, @jobguid = ALTERED.JobGUID, @statusid = ALTERED.StatusId, @name = ALTERED.Name from (select d.Id as Id, d.JobGUID as JobGUID, d.StatusId as StatusId, s.Name as Name from inserted d with(nolock) join JobStatuses s on d.StatusId = s.Id where s.Name = 'DONE' OR s.Name = 'ERROR' EXCEPT select de.Id as Id, de.JobGUID as JobGUID, de.StatusId as StatusId, st.Name as Name from deleted de with(nolock) join JobStatuses st on de.StatusId = st.Id where st.Name = 'DONE' OR st.Name = 'ERROR' ) AS ALTERED

if (@jobguid is null)
begin
	Return
end

set @message = @message + '{ "JobGUID":'
set @message = @message + '"' + cast(@jobguid as varchar(256)) + '", '
set @message = @message + '"JobStatus":'
set @message = @message + cast(@statusid as varchar(2))
set @message = @message + '}'

begin dialog conversation @h
from service UserEmailAlertService to service 'UserEmailAlertService', 'current database'
on contract [UserEmailAlertContract]
with encryption = off;

send on conversation @h message type [UserEmailAlert] (@message)
print @h

if exists(select * from sys.conversation_endpoints where conversation_handle = @h and state = 'ER')
	begin
		RAISERROR('Service Broker in error state', 18, 127)
		rollback transaction
	end
else if exists(select * from sys.conversation_endpoints where conversation_handle = @h and state = 'CD')
	begin
		end conversation @h
	end
else 
	begin
		end conversation @h with cleanup;
		print 'Clean up completed'
	end



GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE trigger [dbo].[get_studyIds_trigger]
on [dbo].[Jobs]
after insert
as 
declare @h as uniqueIdentifier
declare @message varchar(1000)

declare @jobId uniqueidentifier, @jobStatus int
select @jobId = [JobGUID] from inserted with(nolock)

set @message = ''
set @message = @message + cast(@jobId as varchar(256))

begin dialog conversation @h
from service GetStudyIdsService to service 'GetStudyIdsService', 'current database'
on contract [GetStudyIdsContract]
with encryption = off;

send on conversation @h message type [GetStudyIds] (@message)
print @h


if exists(select * from sys.conversation_endpoints where conversation_handle = @h and state = 'ER')
	begin
		RAISERROR('Service Broker in error state', 18, 127)
		rollback transaction
	end
else
	begin
		end conversation @h with cleanup;
		print 'Clean up completed'
	end

GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO



CREATE trigger [dbo].[SendJobRequest]
on [dbo].[Jobs]
after update
as 
declare @h as uniqueIdentifier
declare @message varchar(1000)
declare @id int, @jobguid uniqueidentifier, @statusid int, @name nvarchar(15)
set @message = ''
select @id = ALTERED.Id, @jobguid = ALTERED.JobGUID, @statusid = ALTERED.StatusId, @name = ALTERED.Name from (select d.Id as Id, d.JobGUID as JobGUID, d.StatusId as StatusId, s.Name as Name from inserted d with(nolock) join JobStatuses s on d.StatusId = s.Id where s.Name = 'DONE' OR s.Name = 'ERROR' EXCEPT select de.Id as Id, de.JobGUID as JobGUID, de.StatusId as StatusId, st.Name as Name from deleted de with(nolock) join JobStatuses st on de.StatusId = st.Id where st.Name = 'READY' ) AS ALTERED

if (@jobguid is null)
begin
	Return
end

set @message = @message + '{ "JobGUID":'
set @message = @message + '"' + cast(@jobguid as varchar(256)) + '", '
set @message = @message + '"JobStatus":'
set @message = @message + cast(@statusid as varchar(2))
set @message = @message + '}'

begin dialog conversation @h
from service SendJobRequestService to service 'SendJobRequestService', 'current database'
on contract [SendJobRequestContract]
with encryption = off;

send on conversation @h message type [JobRequest] (@message)
print @h

if exists(select * from sys.conversation_endpoints where conversation_handle = @h and state = 'ER')
	begin
		RAISERROR('Service Broker in error state', 18, 127)
		rollback transaction
	end
else if exists(select * from sys.conversation_endpoints where conversation_handle = @h and state = 'CD')
	begin
		end conversation @h
	end
else 
	begin
		end conversation @h with cleanup;
		print 'Clean up completed'
	end



GO
ALTER TABLE [dbo].[Jobs] ADD CONSTRAINT [PK_Jobs] PRIMARY KEY CLUSTERED  ([Id]) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Jobs] ON [dbo].[Jobs] ([JobGUID]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Jobs] ADD CONSTRAINT [FK_Jobs_JobStatuses] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[JobStatuses] ([Id])
GO
