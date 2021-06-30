CREATE TABLE [dbo].[Author] (
	[AuthorId] [int] IDENTITY(1,1) NOT NULL CONSTRAINT PK_Author PRIMARY KEY,
	[FirstName] [nvarchar](256) NULL,
	[LastName] [nvarchar](256) NOT NULL,
	[Birthday] [datetime] NULL )
	
CREATE TABLE [dbo].[Post] (
	[PostId] [INT] IDENTITY(1,1) NOT NULL CONSTRAINT PK_Post PRIMARY KEY,
	[Title] [NVARCHAR](256) NOT NULL,
	[Body] [NVARCHAR](MAX) NOT NULL,
	[AuthorId] [INT] NOT NULL)

ALTER TABLE [dbo].[Post] 
ADD [CreationDateTime] [DATETIME] NOT NULL DEFAULT(GETDATE())
