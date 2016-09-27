IF NOT EXISTS (SELECT * FROM master.dbo.syslogins WHERE loginname = N'EvalEngineUser')
CREATE LOGIN [EvalEngineUser] WITH PASSWORD = 'p@ssw0rd'
GO
CREATE USER [EvalEngineUser] FOR LOGIN [EvalEngineUser]
GO
