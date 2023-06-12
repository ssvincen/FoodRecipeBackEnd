﻿CREATE TABLE [dbo].[Recipe]
(
	[Id] INT NOT NULL,
	[FK_AuthorId] BIGINT NOT NULL,
	[ImageName] NVARCHAR(250),
	[Name] NVARCHAR(250) NOT NULL, 
	Instructions NVARCHAR(MAX),
	[DateAdd] DATETIME DEFAULT GETDATE(),
	CONSTRAINT [PK_Recipe] PRIMARY KEY CLUSTERED (Id ASC),
	CONSTRAINT [FK_AuthorId] FOREIGN KEY ([FK_AuthorId]) REFERENCES [dbo].[Users](Id)
)