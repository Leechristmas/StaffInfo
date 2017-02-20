--USE u0250751_StaffinfoTestDB;

ALTER TABLE dbo.tbl_Post
DROP CONSTRAINT fk_Post_Service;
GO

ALTER TABLE dbo.tbl_MESAchievement
DROP CONSTRAINT fk_MESAchievement_Employee,
CONSTRAINT fk_MESAchievement_Location,
CONSTRAINT fk_MESAchievement_Rank,
CONSTRAINT fk_MESAchievement_Post,
CONSTRAINT unq_MESAchievement;
GO

ALTER TABLE dbo.tbl_WorkTerm
DROP CONSTRAINT fk_WorkTerm_Employee,
CONSTRAINT fk_WorkTerm_Location,
CONSTRAINT unq_WorkTerm;
GO

ALTER TABLE dbo.tbl_MilitaryService
DROP CONSTRAINT fk_MilitaryService_Employee,
CONSTRAINT fk_MilitaryService_Location,
CONSTRAINT unq_MilitaryService;
GO

--ALTER TABLE dbo.tbl_Employee
--DROP CONSTRAINT fk_Employee_Passport,
--CONSTRAINT fk_Employee_Address;

GO

DROP TABLE dbo.tbl_MESAchievement;
DROP TABLE dbo.tbl_WorkTerm;
DROP TABLE dbo.tbl_Location;
DROP TABLE dbo.tbl_Passport;
DROP TABLE dbo.tbl_Address;
DROP TABLE dbo.tbl_GratitudesAndPunishment;
DROP TABLE dbo.tbl_Sertification;
DROP TABLE dbo.tbl_OutFromOffice;
DROP TABLE dbo.tbl_Education;
DROP TABLE dbo.tbl_Contract;
DROP TABLE dbo.tbl_Relative;
DROP TABLE dbo.tbl_Employee;
DROP TABLE dbo.tbl_Dismissed;
DROP TABLE dbo.tbl_Rank;
DROP TABLE dbo.tbl_Post;
DROP TABLE dbo.tbl_Service;
DROP TABLE dbo.tbl_MilitaryService;
DROP TABLE dbo.tbl_Notification;

GO

DROP FUNCTION dbo.fn_GetActualRankID;
DROP FUNCTION dbo.fn_GetActualPostID;
DROP FUNCTION dbo.fn_GetSeniorityByEmployeeID;
DROP FUNCTION dbo.fn_GetRankExpiryDate;

GO

DROP PROCEDURE dbo.pr_TransferEmployeeToDismissed;
DROP PROCEDURE dbo.pr_GetServicesStructure;
DROP PROCEDURE dbo.pr_GetSeniorityStatistic_NOT_USED;
DROP PROCEDURE dbo.pr_GetNotifications;
DROP PROCEDURE dbo.pr_DeleteNotification;
DROP PROCEDURE dbo.pr_AddNotification;

GO

-----------------------------
--TABLES---------------------
-----------------------------
CREATE TABLE dbo.tbl_Rank (
  ID INT IDENTITY (1, 1) PRIMARY KEY
 ,RankName NVARCHAR(60) NOT NULL
 ,RankWeight INT NOT NULL DEFAULT 0
 ,Term INT NOT NULL --months
);
GO
CREATE TABLE dbo.tbl_Location (
  ID INT IDENTITY (1, 1) PRIMARY KEY
 ,LocationName NVARCHAR(120) NOT NULL
 ,Address NVARCHAR(200)
 ,Description NVARCHAR(500)
);
GO

CREATE TABLE dbo.tbl_Service (
  ID INT IDENTITY (1, 1) PRIMARY KEY
 ,ServiceName NVARCHAR(60) NOT NULL
 ,ServiceShortName NVARCHAR(10)
 ,ServiceGroupId INT NOT NULL
 ,Description NVARCHAR(500)
);
GO

CREATE TABLE dbo.tbl_Post (
  ID INT IDENTITY (1, 1) PRIMARY KEY
 ,PostName NVARCHAR(60) NOT NULL
 ,ServiceID INT NOT NULL
 ,PostWeight INT NOT NULL DEFAULT 0
);

ALTER TABLE dbo.tbl_Post
ADD CONSTRAINT fk_Post_Service
FOREIGN KEY (ServiceID) REFERENCES dbo.tbl_Service;
GO

CREATE TABLE dbo.tbl_Address (
  ID INT IDENTITY (1, 1) PRIMARY KEY
 ,City NVARCHAR(30) NOT NULL
 ,Area NVARCHAR(30) NOT NULL
 ,DetailedAddress NVARCHAR(70) NOT NULL
 ,ZipCode NVARCHAR(6) NOT NULL
);
GO

CREATE TABLE dbo.tbl_Passport (
  ID INT IDENTITY (1, 1) PRIMARY KEY
 ,PassportNumber NVARCHAR(9) NOT NULL
 ,PassportOrganization NVARCHAR(50) NOT NULL
 ,IdentityNumber NVARCHAR(14) NOT NULL
);
GO

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

--ALTER TABLE dbo.tbl_Employee
--  ADD CONSTRAINT fk_Employee_Address
--      FOREIGN KEY (AddressID) REFERENCES tbl_Address,
--      CONSTRAINT fk_Employee_Passport
--      FOREIGN KEY (PassportID) REFERENCES tbl_Passport;
GO


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
GO

CREATE TABLE dbo.tbl_Education (
  ID INT IDENTITY (1, 1) PRIMARY KEY
 ,EmployeeID INT NOT NULL REFERENCES dbo.tbl_Employee ON DELETE CASCADE
 ,Institution NVARCHAR(100) NOT NULL
 ,Speciality NVARCHAR(100) NOT NULL
 ,StartDate DATE NOT NULL
 ,FinishDate DATE NOT NULL
 ,Description NVARCHAR(200)
);

CREATE TABLE dbo.tbl_Contract (
  ID INT IDENTITY (1, 1) PRIMARY KEY
 ,EmployeeID INT NOT NULL REFERENCES dbo.tbl_Employee ON DELETE CASCADE
 ,StartDate DATE NOT NULL
 ,FinishDate DATE NOT NULL
 ,Description NVARCHAR(200)
);

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
GO


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

GO

CREATE TABLE dbo.tbl_Notification (
  ID INT IDENTITY (1, 1) PRIMARY KEY
 ,Author NVARCHAR(100)
 ,Title NVARCHAR(20) NOT NULL
 ,Details NVARCHAR(200)
 ,DueDate DATE NOT NULL
);

GO

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

CREATE TABLE dbo.tbl_Sertification (
  ID INT IDENTITY (1, 1) PRIMARY KEY
 ,EmployeeID INT REFERENCES dbo.tbl_Employee ON DELETE CASCADE
 ,DueDate DATE NOT NULL
 ,Description NVARCHAR(255)
);

ALTER TABLE dbo.tbl_Sertification
ADD CONSTRAINT unq_Sertification
UNIQUE (EmployeeID, DueDate)

GO

CREATE TABLE dbo.tbl_OutFromOffice (
  ID INT IDENTITY (1, 1) PRIMARY KEY
 ,EmployeeID INT REFERENCES dbo.tbl_Employee ON DELETE CASCADE
 ,StartDate DATE NOT NULL
 ,FinishDate DATE NOT NULL
 ,Cause NCHAR(1) NOT NULL  --больничные(S), отпуска(V), отгулы(D)
 ,Description NVARCHAR(255)
)

GO

------------------------------
--PROCEDURES
------------------------------
CREATE PROCEDURE dbo.pr_TransferEmployeeToDismissed @EmployeeId INT,
@DismissalDate DATE,
@Clause NVARCHAR(10),
@ClauseDescription NVARCHAR(150)
AS
BEGIN
  IF @DismissalDate IS NULL
  BEGIN
    RAISERROR ('@DismissalDate is null!', 16, 2);
    RETURN;
  END

  IF (NOT EXISTS (SELECT
        *
      FROM dbo.tbl_Employee te
      WHERE te.ID = @EmployeeId)
    )
  BEGIN
    RAISERROR ('Employee does not exist!', 16, 2);
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

GO

CREATE PROCEDURE dbo.pr_GetServicesStructure @ServiceId INT
AS
BEGIN
  IF @ServiceId IS NOT NULL
  BEGIN
    SELECT DISTINCT
      ts.ServiceName AS Name
     ,COUNT(*) AS Count
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
     ,COUNT(*) AS Count
    FROM tbl_Service ts
        ,tbl_MESAchievement tm
        ,tbl_Post tp
    WHERE tm.PostID = tp.ID
    AND tp.ServiceID = ts.ID
    GROUP BY ts.ServiceName
  END

END

GO

CREATE PROCEDURE dbo.pr_GetSeniorityStatistic_NOT_USED @Scale INT, @Min INT, @Max INT
AS
BEGIN
  DECLARE @data TABLE (
    Name NVARCHAR(20)
   ,Count INT
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
        'от ' + CAST((@step / 365) AS NVARCHAR(3)) + ' до ' + CAST(((@step / 365) + @Scale) AS NVARCHAR(3))
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

GO

CREATE PROCEDURE dbo.pr_GetNotifications @IncludeCustomNotifications BIT = 0,
@IncludeSertification BIT = 0,  --when corresponding table will be added
@IncludeBirthDates BIT = 0,
@IncludeRanks BIT = 0
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
         ,N'День Рождения'
         ,N'День рождения сотрудника ' + te.EmployeeLastname + ' ' + te.EmployeeFirstname + ' ' + te.EmployeeMiddlename + ' (' + CONVERT(NVARCHAR, te.BirthDate, 104) + ')'
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
         ,N'Аттестация'
         ,N'Аттестация сотрудника ' + te.EmployeeLastname + ' ' + te.EmployeeFirstname + ' ' + te.EmployeeMiddlename
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
         ,N'Выслуга звания'
         ,N'Выслуга звания "' + tr.RankName + '" сотрудника ' + te.EmployeeLastname + ' ' + te.EmployeeFirstname + ' ' + te.EmployeeMiddlename
         ,dbo.fn_GetRankExpiryDate(te.ID)
        FROM dbo.tbl_Employee te
            ,dbo.tbl_Rank tr
        WHERE te.ActualRankID IS NOT NULL
        AND te.ActualRankID = tr.ID
        AND te.RetirementDate IS NULL;



  END

  SELECT
    *
  FROM #query q;

END;

GO

CREATE PROCEDURE dbo.pr_DeleteNotification @NotificationId INT
AS
BEGIN
  IF EXISTS (SELECT
        *
      FROM dbo.tbl_Notification n
      WHERE n.ID = @NotificationId)
    DELETE FROM dbo.tbl_Notification
    WHERE ID = @NotificationId;
  ELSE
    RAISERROR ('Notification with specified id does not exist is null!', 16, 2);
END

GO

CREATE PROCEDURE dbo.pr_AddNotification @Author NVARCHAR(100),
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

GO
------------------------------
--FUNCTIONS
------------------------------
--Returns the actual rank id for the specified user
CREATE FUNCTION dbo.fn_GetActualRankID (@EmployeeID INT)
RETURNS INT
AS
BEGIN
  DECLARE @RankID INT;

  SELECT
    @RankID = RankID
  FROM dbo.tbl_MESAchievement tml
  WHERE tml.EmployeeID = @EmployeeID
  AND tml.StartDate = (SELECT
      MAX(StartDate)
    FROM dbo.tbl_MESAchievement tml1
    WHERE tml1.EmployeeID = @EmployeeID);

  RETURN @RankID;
END;

GO

CREATE FUNCTION dbo.fn_GetRankExpiryDate (@EmployeeID INT)
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
  WHERE tml.EmployeeID = @EmployeeID
  AND tml.StartDate = (SELECT
      MAX(StartDate)
    FROM dbo.tbl_MESAchievement tml1
    WHERE tml1.EmployeeID = @EmployeeID);

  SELECT
    @RankSeniority = tr.Term
  FROM tbl_Rank tr
  WHERE tr.ID = @RankID;

  SET @Date = DATEADD(MONTH, @RankSeniority, @Date);

  RETURN @Date;
END

GO

--Returns the actual post id for the specified user
CREATE FUNCTION dbo.fn_GetActualPostID (@EmployeeID INT)
RETURNS INT
AS
BEGIN
  DECLARE @PostID INT;

  SELECT
    @PostID = PostID
  FROM dbo.tbl_MESAchievement tml
  WHERE tml.EmployeeID = @EmployeeID
  AND tml.StartDate = (SELECT
      MAX(StartDate)
    FROM dbo.tbl_MESAchievement tml1
    WHERE tml1.EmployeeID = @EmployeeID);

  RETURN @PostID;
END

GO

--Type: 
--1-MES; 2-Military; 0-Common
CREATE FUNCTION dbo.fn_GetSeniorityByEmployeeID (@EmployeeID INT, @Type INT)
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
    WHERE tm.EmployeeID = @EmployeeID;
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
    WHERE tms.EmployeeID = @EmployeeID;
  END

  RETURN @TotalDays;
END

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
ALTER TABLE dbo.tbl_Employee
ADD Seniority AS dbo.fn_GetSeniorityByEmployeeID(ID, 0);
GO