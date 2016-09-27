IF NOT EXISTS (SELECT * FROM master.dbo.syslogins WHERE loginname = N'RTI\rsteele')
CREATE LOGIN [RTI\rsteele] FROM WINDOWS
GO
CREATE USER [RTI\rsteele] FOR LOGIN [RTI\rsteele]
GO
