CREATE TABLE [dbo].[User]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Email] NVARCHAR(320) NOT NULL UNIQUE, 
    [FirstName] NVARCHAR(128) NOT NULL, 
    [LastName] NVARCHAR(128) NOT NULL,
    [PasswordSalt] NVARCHAR(512) NOT NULL, 
    [PasswordHash] NVARCHAR(512) NOT NULL, 
    [CreatedDateTimeUtc] DATETIME2 NOT NULL, 
    [LastModifiedDateTimeUtc] DATETIME2 NOT NULL
)
