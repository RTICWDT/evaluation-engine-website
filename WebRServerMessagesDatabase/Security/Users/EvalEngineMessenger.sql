IF NOT EXISTS (SELECT * FROM master.dbo.syslogins WHERE loginname = N'EvalEngineMessenger')
CREATE LOGIN [EvalEngineMessenger] WITH PASSWORD = 'p@ssw0rd'
GO
CREATE USER [EvalEngineMessenger] FOR LOGIN [EvalEngineMessenger]
GO
