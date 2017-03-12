------------------------------------------------
---DROP/CREATE TABLES
------------------------------------------------
IF OBJECT_ID(N'dbo.tbl_MESAchievement', 'U') IS NOT NULL
  DROP TABLE dbo.tbl_MESAchievement;
GO


IF OBJECT_ID(N'dbo.tbl_WorkTerm', 'U') IS NOT NULL
  DROP TABLE dbo.tbl_WorkTerm;
GO


IF OBJECT_ID(N'dbo.tbl_Passport', 'U') IS NOT NULL
  DROP TABLE dbo.tbl_Passport;
GO


IF OBJECT_ID(N'dbo.tbl_Address', 'U') IS NOT NULL
  DROP TABLE dbo.tbl_Address;
GO

IF OBJECT_ID(N'dbo.tbl_GratitudesAndPunishment', 'U') IS NOT NULL
  DROP TABLE dbo.tbl_GratitudesAndPunishment;
GO


IF OBJECT_ID(N'dbo.tbl_Sertification', 'U') IS NOT NULL
  DROP TABLE dbo.tbl_Sertification;
GO


IF OBJECT_ID(N'dbo.tbl_OutFromOffice', 'U') IS NOT NULL
  DROP TABLE dbo.tbl_OutFromOffice;
GO


IF OBJECT_ID(N'dbo.tbl_Education', 'U') IS NOT NULL
  DROP TABLE dbo.tbl_Education;
GO


IF OBJECT_ID(N'dbo.tbl_EducationLevel', 'U') IS NOT NULL
  DROP TABLE dbo.tbl_EducationLevel;
GO


IF OBJECT_ID(N'dbo.tbl_Contract', 'U') IS NOT NULL
  DROP TABLE dbo.tbl_Contract;
GO

IF OBJECT_ID(N'dbo.tbl_Relative', 'U') IS NOT NULL
  DROP TABLE dbo.tbl_Relative;
GO


IF OBJECT_ID(N'dbo.tbl_Dismissed', 'U') IS NOT NULL
  DROP TABLE dbo.tbl_Dismissed;
GO


IF OBJECT_ID(N'dbo.tbl_Rank', 'U') IS NOT NULL
  DROP TABLE dbo.tbl_Rank;
GO


IF OBJECT_ID(N'dbo.tbl_Post', 'U') IS NOT NULL
  DROP TABLE dbo.tbl_Post;
GO


IF OBJECT_ID(N'dbo.tbl_Service', 'U') IS NOT NULL
  DROP TABLE dbo.tbl_Service;
GO


IF OBJECT_ID(N'dbo.tbl_MilitaryService', 'U') IS NOT NULL
  DROP TABLE dbo.tbl_MilitaryService;
GO


IF OBJECT_ID(N'dbo.tbl_Employee', 'U') IS NOT NULL
  DROP TABLE dbo.tbl_Employee;
GO


IF OBJECT_ID(N'dbo.tbl_Location', 'U') IS NOT NULL
  DROP TABLE dbo.tbl_Location;
GO


IF OBJECT_ID(N'dbo.tbl_Notification', 'U') IS NOT NULL
  DROP TABLE dbo.tbl_Notification;
GO

--CREATING TABLES
----------------------------
IF OBJECT_ID(N'dbo.tbl_Rank', 'U') IS NULL
CREATE TABLE dbo.tbl_Rank (
    ID INT IDENTITY (1, 1) PRIMARY KEY
   ,RankName NVARCHAR(60) NOT NULL
   ,RankWeight INT NOT NULL DEFAULT 0
   ,Term INT NOT NULL --months
  );
GO


IF OBJECT_ID(N'dbo.tbl_Location', 'U') IS NULL
CREATE TABLE dbo.tbl_Location (
    ID INT IDENTITY (1, 1) PRIMARY KEY
   ,LocationName NVARCHAR(120) NOT NULL
   ,Address NVARCHAR(200)
   ,Description NVARCHAR(500)
  );
GO


IF OBJECT_ID(N'dbo.tbl_Service', 'U') IS NULL
CREATE TABLE dbo.tbl_Service (
    ID INT IDENTITY (1, 1) PRIMARY KEY
   ,ServiceName NVARCHAR(60) NOT NULL
   ,ServiceShortName NVARCHAR(10)
   ,ServiceGroupId INT NOT NULL
   ,Description NVARCHAR(500)
  );
GO


IF OBJECT_ID(N'dbo.tbl_Post', 'U') IS NULL
BEGIN
  CREATE TABLE dbo.tbl_Post (
    ID INT IDENTITY (1, 1) PRIMARY KEY
   ,PostName NVARCHAR(60) NOT NULL
   ,ServiceID INT NOT NULL
   ,PostWeight INT NOT NULL DEFAULT 0
  );

  ALTER TABLE dbo.tbl_Post
  ADD CONSTRAINT fk_Post_Service
  FOREIGN KEY (ServiceID) REFERENCES dbo.tbl_Service;
END
GO


IF OBJECT_ID(N'dbo.tbl_Address', 'U') IS NULL
CREATE TABLE dbo.tbl_Address (
    ID INT IDENTITY (1, 1) PRIMARY KEY
   ,City NVARCHAR(30) NOT NULL
   ,Area NVARCHAR(30) NOT NULL
   ,DetailedAddress NVARCHAR(70) NOT NULL
   ,ZipCode NVARCHAR(6) NOT NULL
  );
GO


IF OBJECT_ID(N'dbo.tbl_Passport', 'U') IS NULL
CREATE TABLE dbo.tbl_Passport (
    ID INT IDENTITY (1, 1) PRIMARY KEY
   ,PassportNumber NVARCHAR(9) NOT NULL
   ,PassportOrganization NVARCHAR(50) NOT NULL
   ,IdentityNumber NVARCHAR(14) NOT NULL
  );
GO


IF OBJECT_ID(N'dbo.tbl_Employee', 'U') IS NULL
--if retirementDAte is null, employee is not pensioner!
  CREATE TABLE dbo.tbl_Employee (
    ID INT IDENTITY (1, 1) PRIMARY KEY
   ,EmployeeFirstname NVARCHAR(30) NOT NULL
   ,EmployeeLastname NVARCHAR(30) NOT NULL
   ,EmployeeMiddlename NVARCHAR(30) NOT NULL
   ,BirthDate DATE NOT NULL
   ,PassportID INT NOT NULL
   ,AddressID INT NOT NULL
   ,ActualRankID INT DEFAULT NULL
   ,ActualPostID INT DEFAULT NULL
   ,RetirementDate DATE DEFAULT NULL
   ,EmployeePhoto VARBINARY(MAX)
   ,PhotoMimeType NVARCHAR(10)
   ,Description NVARCHAR(100)
   ,FirstPhoneNumber NVARCHAR(13)
   ,SecondPhoneNumber NVARCHAR(13)
   ,Gender NCHAR(1) NOT NULL--'W'/'M',
   ,PersonalNumber NVARCHAR(7) NOT NULL UNIQUE
  --TODO
  );
GO


IF OBJECT_ID(N'dbo.tbl_Relative', 'U') IS NULL
CREATE TABLE dbo.tbl_Relative (
    ID INT IDENTITY (1, 1) PRIMARY KEY
   ,EmployeeID INT NOT NULL REFERENCES tbl_Employee ON DELETE CASCADE
   ,Lastname NVARCHAR(30) NOT NULL
   ,Firstname NVARCHAR(30) NOT NULL
   ,Middlename NVARCHAR(30) NOT NULL
   ,BirthDate DATE NOT NULL
   ,Status NVARCHAR(15)
  );
GO


IF OBJECT_ID(N'dbo.tbl_Dismissed', 'U') IS NULL
--dismissed employees
  CREATE TABLE dbo.tbl_Dismissed (
    ID INT IDENTITY (1, 1) PRIMARY KEY
   ,DismissedLastname NVARCHAR(30) NOT NULL
   ,DismissedFirstname NVARCHAR(30) NOT NULL
   ,DismissedMiddlename NVARCHAR(30) NOT NULL
   ,BirthDate DATE NOT NULL
   ,DismissalDate DATE NOT NULL
   ,Clause NVARCHAR(10)
   ,ClauseDescription NVARCHAR(150)
  );
GO


IF OBJECT_ID(N'dbo.tbl_MESAchievement', 'U') IS NULL
BEGIN
  --achievement list of ministry of emergency situations service of the employee
  CREATE TABLE dbo.tbl_MESAchievement (
    ID INT IDENTITY (1, 1) PRIMARY KEY
   ,EmployeeID INT NOT NULL
   ,LocationID INT NOT NULL
   ,StartDate DATE NOT NULL
   ,FinishDate DATE DEFAULT NULL
   ,PostID INT NOT NULL
   ,RankID INT NOT NULL
   ,Description NVARCHAR(200)
  );

  ALTER TABLE dbo.tbl_MESAchievement
  ADD CONSTRAINT fk_MESAchievement_Employee
  FOREIGN KEY (EmployeeID) REFERENCES dbo.tbl_Employee ON DELETE CASCADE,
  CONSTRAINT fk_MESAchievement_Location
  FOREIGN KEY (LocationID) REFERENCES dbo.tbl_Location,
  CONSTRAINT fk_MESAchievement_Rank
  FOREIGN KEY (RankID) REFERENCES dbo.tbl_Rank,
  CONSTRAINT fk_MESAchievement_Post
  FOREIGN KEY (PostID) REFERENCES dbo.tbl_Post,
  CONSTRAINT unq_MESAchievement
  UNIQUE (EmployeeID, StartDate);
END
GO


IF OBJECT_ID(N'dbo.tbl_EducationLevel', 'U') IS NULL
CREATE TABLE dbo.tbl_EducationLevel (
    ID INT IDENTITY (1, 1)
   ,Transcript NVARCHAR(50) NOT NULL
   ,Weight SMALLINT NOT NULL
   ,Description NVARCHAR(50) NULL
   ,CONSTRAINT PK_tbl_EducationLevel_ID PRIMARY KEY CLUSTERED (ID)
  ) ON [PRIMARY]
GO


IF OBJECT_ID(N'dbo.tbl_Education', 'U') IS NULL
CREATE TABLE dbo.tbl_Education (
    ID INT IDENTITY (1, 1) PRIMARY KEY
   ,EmployeeID INT NOT NULL REFERENCES dbo.tbl_Employee ON DELETE CASCADE
   ,Institution NVARCHAR(100) NOT NULL
   ,Speciality NVARCHAR(100) NOT NULL
   ,LevelCode INT NOT NULL REFERENCES tbl_EducationLevel
   ,StartDate DATE NOT NULL
   ,FinishDate DATE NOT NULL
   ,Description NVARCHAR(200)
  );
GO


IF OBJECT_ID(N'dbo.tbl_Contract', 'U') IS NULL
CREATE TABLE dbo.tbl_Contract (
    ID INT IDENTITY (1, 1) PRIMARY KEY
   ,EmployeeID INT NOT NULL REFERENCES dbo.tbl_Employee ON DELETE CASCADE
   ,StartDate DATE NOT NULL
   ,FinishDate DATE NOT NULL
   ,Description NVARCHAR(200)
  );
GO


IF OBJECT_ID(N'dbo.tbl_WorkTerm', 'U') IS NULL
BEGIN
  CREATE TABLE dbo.tbl_WorkTerm (
    ID INT IDENTITY (1, 1) PRIMARY KEY
   ,EmployeeID INT NOT NULL
   ,LocationID INT NOT NULL
   ,Post NVARCHAR(30) NOT NULL
   ,StartDate DATE NOT NULL
   ,FinishDate DATE NOT NULL
   ,Description NVARCHAR(200)
  );

  ALTER TABLE dbo.tbl_WorkTerm
  ADD CONSTRAINT fk_WorkTerm_Employee
  FOREIGN KEY (EmployeeID) REFERENCES dbo.tbl_Employee ON DELETE CASCADE,
  CONSTRAINT fk_WorkTerm_Location
  FOREIGN KEY (LocationID) REFERENCES dbo.tbl_Location,
  CONSTRAINT unq_WorkTerm
  UNIQUE (EmployeeID, StartDate);
END
GO


IF OBJECT_ID(N'dbo.tbl_MilitaryService', 'U') IS NULL
BEGIN
  CREATE TABLE dbo.tbl_MilitaryService (
    ID INT IDENTITY (1, 1) PRIMARY KEY
   ,EmployeeID INT NOT NULL
   ,LocationID INT NOT NULL
   ,Rank NVARCHAR(30) NOT NULL
   ,StartDate DATE NOT NULL
   ,FinishDate DATE NOT NULL
   ,Description NVARCHAR(200)
  );

  ALTER TABLE dbo.tbl_MilitaryService
  ADD CONSTRAINT fk_MilitaryService_Employee
  FOREIGN KEY (EmployeeID) REFERENCES dbo.tbl_Employee ON DELETE CASCADE,
  CONSTRAINT fk_MilitaryService_Location
  FOREIGN KEY (LocationID) REFERENCES dbo.tbl_Location,
  CONSTRAINT unq_MilitaryService
  UNIQUE (EmployeeID, StartDate);
END
GO


IF OBJECT_ID(N'dbo.tbl_Notification', 'U') IS NULL
CREATE TABLE dbo.tbl_Notification (
    ID INT IDENTITY (1, 1) PRIMARY KEY
   ,Author NVARCHAR(100)
   ,Title NVARCHAR(20) NOT NULL
   ,Details NVARCHAR(200)
   ,DueDate DATE NOT NULL
  );
GO


IF OBJECT_ID(N'dbo.tbl_GratitudesAndPunishment', 'U') IS NULL
CREATE TABLE dbo.tbl_GratitudesAndPunishment (
    ID INT IDENTITY (1, 1) PRIMARY KEY
   ,EmployeeID INT REFERENCES dbo.tbl_Employee ON DELETE CASCADE
   ,Title NVARCHAR(60) NOT NULL
   ,ItemType NCHAR(1) NOT NULL --'G' - gratitude/ 'V' - violation
   ,Date DATE NOT NULL
   ,Description NVARCHAR(200)
   ,AwardOrFine BIGINT  --kopecs   
  );
GO


IF OBJECT_ID(N'dbo.tbl_Sertification', 'U') IS NULL
BEGIN
  CREATE TABLE dbo.tbl_Sertification (
    ID INT IDENTITY (1, 1) PRIMARY KEY
   ,EmployeeID INT REFERENCES dbo.tbl_Employee ON DELETE CASCADE
   ,DueDate DATE NOT NULL
   ,Level NVARCHAR(15)
   ,Description NVARCHAR(255)
  );

  ALTER TABLE dbo.tbl_Sertification
  ADD CONSTRAINT unq_Sertification
  UNIQUE (EmployeeID, DueDate)
END
GO


IF OBJECT_ID(N'dbo.tbl_OutFromOffice', 'U') IS NULL
CREATE TABLE dbo.tbl_OutFromOffice (
    ID INT IDENTITY (1, 1) PRIMARY KEY
   ,EmployeeID INT REFERENCES dbo.tbl_Employee ON DELETE CASCADE
   ,StartDate DATE NOT NULL
   ,FinishDate DATE NOT NULL
   ,Cause NCHAR(1) NOT NULL  --больничные(S), отпуска(V), отгулы(D)
   ,Description NVARCHAR(255)
  );
GO


------------------------------
--PROCEDURES
------------------------------
--needed because of skipping a seniority field
IF OBJECT_ID(N'dbo.sp_InsertEmployee', 'P') IS NOT NULL
  EXEC sp_executesql N'DROP PROCEDURE dbo.sp_InsertEmployee'
GO

SET QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
IF OBJECT_ID(N'dbo.sp_InsertEmployee', 'P') IS NULL
  EXEC sp_executesql N'
--needed because of skipping a seniority field
CREATE PROCEDURE dbo.sp_InsertEmployee @lastname NVARCHAR(30)
, @firstname NVARCHAR(30)
, @middlename NVARCHAR(30)
, @birthdate DATE
, @passportId INT
, @addressId INT
, @actualRankId INT
, @actualPostId INT
, @retirementDate DATE
, @employeePhoto VARBINARY(MAX)
, @photoMimeType NVARCHAR(10)
, @description NVARCHAR(100)
, @firstPhoneNumber NVARCHAR(13)
, @secondPhoneNumber NVARCHAR(13)
, @gender NCHAR(1)
, @personalNumber NVARCHAR(7)
AS
BEGIN
  INSERT INTO dbo.tbl_Employee (EmployeeFirstname
    ,EmployeeLastname
    ,EmployeeMiddlename
    ,BirthDate
    ,PassportID
    ,AddressID
    ,ActualRankID
    ,ActualPostID
    ,RetirementDate
    ,EmployeePhoto
    ,PhotoMimeType
    ,Description
    ,FirstPhoneNumber
    ,SecondPhoneNumber
    ,Gender
    ,PersonalNumber)
    VALUES (@firstname
  ,@lastname
  ,@middlename
  ,@birthdate
  ,@passportId
  ,@addressId
  ,@actualRankId
  ,@actualPostId
  ,@retirementDate
  ,@employeePhoto
  ,@photoMimeType
  ,@description
  ,@firstPhoneNumber
  ,@secondPhoneNumber
  ,@gender
  ,@personalNumber);
  
  SELECT Scope_Identity() as Id;
  
END;

'
GO


IF OBJECT_ID(N'dbo.sp_TransferEmployeeToDismissed', 'P') IS NOT NULL
  EXEC sp_executesql N'DROP PROCEDURE dbo.sp_TransferEmployeeToDismissed'
GO

SET QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
IF OBJECT_ID(N'dbo.sp_TransferEmployeeToDismissed', 'P') IS NULL
  EXEC sp_executesql N'
  CREATE PROCEDURE dbo.sp_TransferEmployeeToDismissed @EmployeeId INT,
  @DismissalDate DATE,
  @Clause NVARCHAR(10),
  @ClauseDescription NVARCHAR(150)
  AS
  BEGIN
    IF @DismissalDate IS NULL
    BEGIN
      RAISERROR (''@DismissalDate is null!'', 16, 2);
      RETURN;
    END

    IF (NOT EXISTS (SELECT
          *
        FROM dbo.tbl_Employee te
        WHERE te.ID = @EmployeeId)
      )
    BEGIN
      RAISERROR (''Employee does not exist!'', 16, 2);
      RETURN;
    END

    INSERT INTO dbo.tbl_Dismissed (DismissedLastname, DismissedFirstname, DismissedMiddlename, BirthDate, DismissalDate, Clause, ClauseDescription)
      SELECT
        te.EmployeeLastname
       ,te.EmployeeFirstname
       ,te.EmployeeMiddlename
       ,te.BirthDate
       ,@DismissalDate
       ,@Clause
       ,@ClauseDescription
      FROM dbo.tbl_Employee te
      WHERE te.ID = @EmployeeId;

    DELETE FROM dbo.tbl_Employee
    WHERE ID = @EmployeeId;

  END
'

GO

IF OBJECT_ID(N'dbo.sp_GetServicesStructure', 'P') IS NOT NULL
  EXEC sp_executesql N'DROP PROCEDURE dbo.sp_GetServicesStructure'
GO

SET QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
IF OBJECT_ID(N'dbo.sp_GetServicesStructure', 'P') IS NULL
  EXEC sp_executesql N'
  CREATE PROCEDURE dbo.sp_GetServicesStructure @ServiceId INT
  AS
  BEGIN
    IF @ServiceId IS NOT NULL
    BEGIN
      SELECT DISTINCT
        ts.ServiceName AS Name
       ,COUNT(*) AS count
      FROM tbl_Service ts
          ,tbl_MESAchievement tm
          ,tbl_Post tp
      WHERE tm.PostID = tp.ID
      AND tp.ServiceID = ts.ID
      AND tp.ServiceID = @ServiceId
      GROUP BY ts.ServiceName
    END
    ELSE
    BEGIN
      SELECT DISTINCT
        ts.ServiceName AS Name
       ,COUNT(*) AS count
      FROM tbl_Service ts
          ,tbl_MESAchievement tm
          ,tbl_Post tp
      WHERE tm.PostID = tp.ID
      AND tp.ServiceID = ts.ID
      GROUP BY ts.ServiceName
    END

  END

'

GO

IF OBJECT_ID(N'dbo.sp_GetSeniorityStatistic_NOT_USED', 'P') IS NOT NULL
  EXEC sp_executesql N'DROP PROCEDURE dbo.sp_GetSeniorityStatistic_NOT_USED'
GO

SET QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
IF OBJECT_ID(N'dbo.sp_GetSeniorityStatistic_NOT_USED', 'P') IS NULL
  EXEC sp_executesql N'
  CREATE PROCEDURE dbo.sp_GetSeniorityStatistic_NOT_USED @Scale INT, @Min INT, @Max INT
  AS
  BEGIN
    DECLARE @data TABLE (
      Name NVARCHAR(20)
     ,count INT
    );
    CREATE TABLE #seniority (
      Seniority INT
    );

    INSERT INTO #seniority (Seniority)
      SELECT
        dbo.fn_GetSeniorityByEmployeeID(te.ID, 0)
      FROM tbl_Employee te;


    DECLARE @step INT = 0;

    WHILE (@step / 365) <= @Max
    BEGIN
    INSERT INTO @data
      SELECT
        ''от '' + CAST((@step / 365) AS NVARCHAR(3)) + '' до '' + CAST(((@step / 365) + @Scale) AS NVARCHAR(3))
       ,COUNT(*)
      FROM #seniority s
      WHERE s.Seniority >= @step
      AND s.Seniority < @step + @Scale * 365;
    SET @step = @step + @Scale * 365
    END

    SELECT
      *
    FROM @data;

  END

'
GO

IF OBJECT_ID(N'dbo.sp_GetNotifications', 'P') IS NOT NULL
  EXEC sp_executesql N'DROP PROCEDURE dbo.sp_GetNotifications'
GO

SET QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
IF OBJECT_ID(N'dbo.sp_GetNotifications', 'P') IS NULL
  EXEC sp_executesql N'
  CREATE PROCEDURE dbo.sp_GetNotifications @IncludeCustomNotifications BIT = 0,
  @IncludeSertification BIT = 0,  --when corresponding table will be added
  @IncludeBirthDates BIT = 0,
  @IncludeRanks BIT = 0,
  @IncludeContracts BIT = 0
  AS
  BEGIN
    CREATE TABLE #query (
      ID INT
     ,Author NVARCHAR(100)
     ,Title NVARCHAR(20) NOT NULL
     ,Details NVARCHAR(200)
     ,DueDate DATE NOT NULL
    );

    IF @IncludeCustomNotifications = 1
    BEGIN
      INSERT INTO #query (ID, Author, Title, Details, DueDate)
        SELECT
          tn.ID
         ,tn.Author
         ,tn.Title
         ,tn.Details
         ,tn.DueDate
        FROM dbo.tbl_Notification tn
    END

    IF @IncludeBirthDates = 1
    BEGIN
      INSERT INTO #query (ID, Author, Title, Details, DueDate)
        SELECT
          -1
         ,NULL
         ,N''День Рождения''
         ,N''День рождения сотрудника '' + te.EmployeeLastname + '' '' + te.EmployeeFirstname + '' '' + te.EmployeeMiddlename + '' ('' + CONVERT(NVARCHAR, te.BirthDate, 104) + '')''
         ,DATEADD(yy, DATEDIFF(yy, te.BirthDate, GETDATE()), te.BirthDate)
        FROM dbo.tbl_Employee te
        WHERE te.RetirementDate IS NULL;
    END

    IF @IncludeSertification = 1
    BEGIN
      INSERT INTO #query (ID, Author, Title, Details, DueDate)
        SELECT
          -1
         ,NULL
         ,N''Аттестация''
         ,N''Аттестация сотрудника '' + te.EmployeeLastname + '' '' + te.EmployeeFirstname + '' '' + te.EmployeeMiddlename
         ,ts.DueDate
        FROM dbo.tbl_Sertification ts
            ,dbo.tbl_Employee te
        WHERE ts.EmployeeID = te.ID
        AND te.RetirementDate IS NULL;
    END

    IF @IncludeRanks = 1
    BEGIN
      INSERT INTO #query (ID, Author, Title, Details, DueDate)
        SELECT
          -1
         ,NULL
         ,N''Выслуга звания''
         ,N''Выслуга звания "'' + tr.RankName + ''" сотрудника '' + te.EmployeeLastname + '' '' + te.EmployeeFirstname + '' '' + te.EmployeeMiddlename
         ,dbo.fn_GetRankExpiryDate(te.ID)
        FROM dbo.tbl_Employee te
            ,dbo.tbl_Rank tr
        WHERE te.ActualRankID IS NOT NULL
        AND te.ActualRankID = tr.ID
        AND te.RetirementDate IS NULL;

      IF @IncludeContracts = 1
      BEGIN
        INSERT INTO #query (ID, Author, Title, Details, DueDate)
          SELECT
            -1 AS ID
           ,NULL AS Author
           ,N''Истечение контракта'' AS Title
           ,N''Истечение контракта ('' + CONVERT(NVARCHAR, tc.StartDate, 103) + '' - '' + CONVERT(NVARCHAR, tc.FinishDate, 103) + '') сотрудника '' + te.EmployeeLastname + '' '' + te.EmployeeFirstname + '' '' + te.EmployeeMiddlename AS Details
           ,MAX(tc.FinishDate) AS DueDate
          FROM dbo.tbl_Employee te
              ,dbo.tbl_Contract tc
          WHERE tc.EmployeeID = te.ID
          AND te.RetirementDate IS NULL
          GROUP BY tc.StartDate
                  ,tc.FinishDate
                  ,te.EmployeeLastname
                  ,te.EmployeeFirstname
                  ,te.EmployeeMiddlename;
      END
    END

    SELECT
      *
    FROM #query q;

  END;

'
GO

IF OBJECT_ID(N'dbo.sp_DeleteNotification', 'P') IS NOT NULL
  EXEC sp_executesql N'DROP PROCEDURE dbo.sp_DeleteNotification'
GO

SET QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
IF OBJECT_ID(N'dbo.sp_DeleteNotification', 'P') IS NULL
  EXEC sp_executesql N'
  CREATE PROCEDURE dbo.sp_DeleteNotification @NotificationId INT
  AS
  BEGIN
    IF EXISTS (SELECT
          *
        FROM dbo.tbl_Notification n
        WHERE n.ID = @NotificationId)
      DELETE FROM dbo.tbl_Notification
      WHERE ID = @NotificationId;
    ELSE
      RAISERROR (''Notification with specified id does not exist is null!'', 16, 2);
  END

'
GO

IF OBJECT_ID(N'dbo.sp_AddNotification', 'P') IS NOT NULL
  EXEC sp_executesql N'DROP PROCEDURE dbo.sp_AddNotification'
GO

SET QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
IF OBJECT_ID(N'dbo.sp_AddNotification', 'P') IS NULL
  EXEC sp_executesql N'
  CREATE PROCEDURE dbo.sp_AddNotification @Author NVARCHAR(100),
  @Title NVARCHAR(20),
  @Details NVARCHAR(200),
  @DueDate DATE
  AS
  BEGIN
    INSERT INTO tbl_Notification (Author, Title, Details, DueDate)
      VALUES (@Author, @Title, @Details, @DueDate);
    SELECT
      *
    FROM tbl_Notification tn
    WHERE tn.ID = (SELECT
        MAX(ID)
      FROM tbl_Notification tn1)
  END

'
GO
------------------------------
--FUNCTIONS
------------------------------
IF EXISTS (SELECT
      1
    FROM sys.objects
    WHERE object_id = OBJECT_ID(N'dbo.fn_GetActualRankID')
    AND type IN ('IF', 'FN', 'TF'))
  EXEC sp_executesql N'DROP FUNCTION dbo.fn_GetActualRankID'
GO

SET QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
IF NOT EXISTS (SELECT
      1
    FROM sys.objects
    WHERE object_id = OBJECT_ID(N'dbo.fn_GetActualRankID')
    AND type IN ('IF', 'FN', 'TF'))
  EXEC sp_executesql N'  ------------------------------
  --FUNCTIONS
  ------------------------------
  --Returns the actual rank id for the specified user
  CREATE FUNCTION dbo.fn_GetActualRankID (@EmployeeId INT)
  RETURNS INT
  AS
  BEGIN
    DECLARE @RankID INT;

    SELECT
      @RankID = RankID
    FROM dbo.tbl_MESAchievement tml
    WHERE tml.EmployeeID = @EmployeeId
    AND tml.StartDate = (SELECT
        MAX(StartDate)
      FROM dbo.tbl_MESAchievement tml1
      WHERE tml1.EmployeeID = @EmployeeId);

    RETURN @RankID;
  END;

'
GO

IF EXISTS (SELECT
      1
    FROM sys.objects
    WHERE object_id = OBJECT_ID(N'dbo.fn_GetRankExpiryDate')
    AND type IN ('IF', 'FN', 'TF'))
  EXEC sp_executesql N'DROP FUNCTION dbo.fn_GetRankExpiryDate'
GO

SET QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
IF NOT EXISTS (SELECT
      1
    FROM sys.objects
    WHERE object_id = OBJECT_ID(N'dbo.fn_GetRankExpiryDate')
    AND type IN ('IF', 'FN', 'TF'))
  EXEC sp_executesql N'
CREATE FUNCTION dbo.fn_GetRankExpiryDate (@EmployeeId INT)
RETURNS DATE
AS
BEGIN
  DECLARE @Date DATE;
  DECLARE @RankID INT;
  DECLARE @RankSeniority INT;

  SELECT
    @RankID = RankID
   ,@Date = tml.StartDate
  FROM dbo.tbl_MESAchievement tml
  WHERE tml.EmployeeID = @EmployeeId
  AND tml.StartDate = (SELECT
      MAX(StartDate)
    FROM dbo.tbl_MESAchievement tml1
    WHERE tml1.EmployeeID = @EmployeeId);

  SELECT
    @RankSeniority = tr.Term
  FROM tbl_Rank tr
  WHERE tr.ID = @RankID;

  SET @Date = DATEADD(MONTH, @RankSeniority, @Date);

  RETURN @Date;
END

'
GO

IF EXISTS (SELECT
      1
    FROM sys.objects
    WHERE object_id = OBJECT_ID(N'dbo.fn_GetActualPostID')
    AND type IN ('IF', 'FN', 'TF'))
  EXEC sp_executesql N'DROP FUNCTION dbo.fn_GetActualPostID'
GO

SET QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
IF NOT EXISTS (SELECT
      1
    FROM sys.objects
    WHERE object_id = OBJECT_ID(N'dbo.fn_GetActualPostID')
    AND type IN ('IF', 'FN', 'TF'))
  EXEC sp_executesql N'
--Returns the actual post id for the specified user
CREATE FUNCTION dbo.fn_GetActualPostID (@EmployeeId INT)
RETURNS INT
AS
BEGIN
  DECLARE @PostID INT;

  SELECT
    @PostID = PostID
  FROM dbo.tbl_MESAchievement tml
  WHERE tml.EmployeeID = @EmployeeId
  AND tml.StartDate = (SELECT
      MAX(StartDate)
    FROM dbo.tbl_MESAchievement tml1
    WHERE tml1.EmployeeID = @EmployeeId);

  RETURN @PostID;
END

'
GO

IF EXISTS (SELECT
      1
    FROM sys.objects
    WHERE object_id = OBJECT_ID(N'dbo.fn_GetSeniorityByEmployeeID')
    AND type IN ('IF', 'FN', 'TF'))
  EXEC sp_executesql N'DROP FUNCTION dbo.fn_GetSeniorityByEmployeeID'
GO

SET QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
IF NOT EXISTS (SELECT
      1
    FROM sys.objects
    WHERE object_id = OBJECT_ID(N'dbo.fn_GetSeniorityByEmployeeID')
    AND type IN ('IF', 'FN', 'TF'))
  EXEC sp_executesql N'
--Type: 
--1-MES; 2-Military; 0-Common
CREATE FUNCTION dbo.fn_GetSeniorityByEmployeeID (@EmployeeId INT, @Type INT)
RETURNS INT
AS
BEGIN
  DECLARE @TotalDays INT = 0;

  IF (@Type = 1
    OR @Type = 0)
  BEGIN
    SELECT
      @TotalDays = @TotalDays + DATEDIFF(DAY, tm.StartDate, CASE
        WHEN tm.FinishDate IS NULL THEN GETDATE()
        ELSE tm.FinishDate
      END)
    FROM tbl_MESAchievement tm
    WHERE tm.EmployeeID = @EmployeeId;
  END

  IF (@Type = 2
    OR @Type = 0)
  BEGIN
    SELECT
      @TotalDays = @TotalDays + DATEDIFF(DAY, tms.StartDate, CASE
        WHEN tms.FinishDate IS NULL THEN GETDATE()
        ELSE tms.FinishDate
      END)
    FROM tbl_MilitaryService tms
    WHERE tms.EmployeeID = @EmployeeId;
  END

  RETURN @TotalDays;
END

'
GO
------------------------------
--TRIGGERS
------------------------------
--cascade deleting addresses and passport data
CREATE TRIGGER EmployeeDeleteTrigger
ON tbl_Employee
AFTER DELETE
AS
BEGIN
  DELETE ta
    FROM tbl_Address ta, DELETED d
  WHERE ta.ID = d.AddressID;
  DELETE tp
    FROM tbl_Passport tp, DELETED d
  WHERE tp.ID = d.PassportID;
--TODO
END

GO

--Updating the actual rank and post for employees
CREATE TRIGGER MESAchievementInsertUpdateDeleteTrigger
ON tbl_MESAchievement
AFTER INSERT, DELETE, UPDATE
AS
BEGIN
  UPDATE dbo.tbl_Employee
  SET ActualRankID = dbo.fn_GetActualRankID(ID)
     ,ActualPostID = dbo.fn_GetActualPostID(ID)
END

GO

--forbid adding more than 1 null finish date for every employee
CREATE TRIGGER MESAchievementInserTrigger
ON tbl_MESAchievement
FOR INSERT, UPDATE
AS
BEGIN

  DECLARE @emplID INT
         ,@date DATE;

  DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR SELECT
    i.EmployeeID
   ,i.FinishDate
  FROM INSERTED i

  OPEN cur

  FETCH NEXT FROM cur INTO @emplID, @date

  WHILE @@fetch_status = 0
  BEGIN

  IF @date IS NULL
  BEGIN
    IF (SELECT
          COUNT(*)
        FROM INSERTED i
        WHERE i.EmployeeID = @emplID
        AND i.FinishDate IS NULL)
      > 1
    BEGIN
      ROLLBACK TRAN;
      RAISERROR ('Can not be inserted more than 1 assignment with null finish date!', 16, 2);
      RETURN;
    END

    IF (SELECT
          COUNT(*)
        FROM tbl_MESAchievement tm
        WHERE tm.EmployeeID = @emplID
        AND tm.FinishDate IS NULL)
      > 1
    BEGIN
      ROLLBACK TRAN;
      RAISERROR ('Actual assignment (finishDate is null) is already exists!', 16, 2);
      RETURN;
    END
  END

  FETCH NEXT FROM cur INTO @emplID, @date

  END

  CLOSE cur
  DEALLOCATE cur

END;

GO

------------------------------
--ADDITIONAL
------------------------------
IF OBJECT_ID(N'dbo.tbl_Employee', 'U') IS NOT NULL
  ALTER TABLE dbo.tbl_Employee
  ADD Seniority AS dbo.fn_GetSeniorityByEmployeeID(ID, 0);
GO