CREATE TABLE [dbo].[User]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Email] NVARCHAR(320) NOT NULL UNIQUE, 
    [PasswordSalt] NVARCHAR(512) NOT NULL, 
    [PasswordHash] NVARCHAR(512) NOT NULL, 
    [CreatedDateTime] DATETIME2 NOT NULL, 
    [LastModifiedDateTime] DATETIME2 NOT NULL
)
