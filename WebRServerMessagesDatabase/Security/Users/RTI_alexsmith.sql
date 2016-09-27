IF NOT EXISTS (SELECT * FROM master.dbo.syslogins WHERE loginname = N'RTI\alexsmith')
CREATE LOGIN [RTI\alexsmith] FROM WINDOWS
GO
CREATE USER [RTI\alexsmith] FOR LOGIN [RTI\alexsmith]
GO
