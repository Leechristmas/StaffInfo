IF OBJECT_ID(N'dbo.tbl_ApiLog', 'U') IS NOT NULL
  DROP TABLE dbo.tbl_ApiLog
GO

IF OBJECT_ID(N'dbo.tbl_ApiLog', 'U') IS NULL
  CREATE TABLE dbo.tbl_ApiLog (
    ID INT IDENTITY
   ,Timestamp DATETIME NOT NULL
   ,LogLevel NVARCHAR(20) NOT NULL
   ,Message NVARCHAR(MAX) NULL
   ,Help NVARCHAR(MAX) NULL
   ,UserId UNIQUEIDENTIFIER NULL
   ,CONSTRAINT PK_tbl_ApiLog PRIMARY KEY CLUSTERED (ID)
  ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO