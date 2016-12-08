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
DROP TABLE dbo.tbl_Employee;
DROP TABLE dbo.tbl_Dismissed;
DROP TABLE dbo.tbl_Rank;
DROP TABLE dbo.tbl_Post;
DROP TABLE dbo.tbl_Service;
DROP TABLE dbo.tbl_MilitaryService;

GO

DROP FUNCTION dbo.fn_GetActualRankID;
DROP FUNCTION dbo.fn_GetActualPostID;
DROP FUNCTION dbo.fn_GetExpirienceByEmployeeID;

GO

DROP PROCEDURE dbo.pr_TransferEmployeeToDismissed;

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
);
GO

--if retirementDAte is null, employee is not pensioner!
CREATE TABLE dbo.tbl_Employee (
  ID INT IDENTITY (1, 1) PRIMARY KEY
 ,EmployeeFirstname NVARCHAR(30) NOT NULL
 ,EmployeeLastname NVARCHAR(30) NOT NULL
 ,EmployeeMiddlename NVARCHAR(30) NOT NULL
 ,BirthDate DATETIME NOT NULL
 ,PassportID INT NOT NULL
 ,AddressID INT NOT NULL
 ,ActualRankID INT DEFAULT NULL
 ,ActualPostID INT DEFAULT NULL
 ,RetirementDate DATETIME DEFAULT NULL
 ,EmployeePhoto VARBINARY(MAX)
 ,PhotoMimeType NVARCHAR(10)
 ,Description NVARCHAR(100)
 ,FirstPhoneNumber NVARCHAR(13)
 ,SecondPhoneNumber NVARCHAR(13)
--TODO
);

--ALTER TABLE dbo.tbl_Employee
--  ADD CONSTRAINT fk_Employee_Address
--      FOREIGN KEY (AddressID) REFERENCES tbl_Address,
--      CONSTRAINT fk_Employee_Passport
--      FOREIGN KEY (PassportID) REFERENCES tbl_Passport;
GO

--dismissed employees
CREATE TABLE dbo.tbl_Dismissed (
  ID INT IDENTITY (1, 1) PRIMARY KEY
 ,DismissedLastname NVARCHAR(30) NOT NULL
 ,DismissedFirstname NVARCHAR(30) NOT NULL
 ,DismissedMiddlename NVARCHAR(30) NOT NULL
 ,BirthDate DATETIME NOT NULL
 ,DismissalDate DATETIME NOT NULL
 ,Clause NVARCHAR(10)
 ,ClauseDescription NVARCHAR(150)
);

--achievement list of ministry of emergency situations service of the employee
CREATE TABLE dbo.tbl_MESAchievement (
  ID INT IDENTITY (1, 1) PRIMARY KEY
 ,EmployeeID INT NOT NULL
 ,LocationID INT NOT NULL
 ,StartDate DATETIME NOT NULL
 ,FinishDate DATETIME
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

CREATE TABLE dbo.tbl_WorkTerm (
  ID INT IDENTITY (1, 1) PRIMARY KEY
 ,EmployeeID INT NOT NULL
 ,LocationID INT NOT NULL
 ,Post NVARCHAR(30) NOT NULL
 ,StartDate DATETIME NOT NULL
 ,FinishDate DATETIME NOT NULL
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
 ,StartDate DATETIME NOT NULL
 ,FinishDate DATETIME NOT NULL
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

------------------------------
--PROCEDURES
------------------------------
CREATE PROCEDURE dbo.pr_TransferEmployeeToDismissed @EmployeeId INT,
@DismissalDate DATETIME,
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
CREATE FUNCTION dbo.fn_GetExpirienceByEmployeeID (@EmployeeID INT, @Type INT)
RETURNS INT
AS
BEGIN
  DECLARE @TotalDays INT = 0;

  IF(@Type = 1 OR @Type = 0)
    BEGIN
    	SELECT @TotalDays = @TotalDays + DATEDIFF(DAY, tm.StartDate, tm.FinishDate)
        FROM tbl_MESAchievement tm
      WHERE tm.EmployeeID = @EmployeeID;
    END
  
  IF(@Type = 2 OR @Type = 0)
    BEGIN
    	SELECT @TotalDays = @TotalDays + DATEDIFF(DAY, tms.StartDate, tms.FinishDate)
        FROM tbl_MilitaryService tms
      WHERE tms.EmployeeID = @EmployeeID;
    END

  RETURN @TotalDays;
END

GO
------------------------------
--TRIGGERS
------------------------------
--Updating the actual rank and post for employees
CREATE TRIGGER MESAchievemntInsertTrigger
ON tbl_MESAchievement
AFTER INSERT, DELETE, UPDATE
AS
BEGIN
  UPDATE tbl_Employee
  SET ActualRankID = dbo.fn_GetActualRankID(i.EmployeeID)
     ,ActualPostID = dbo.fn_GetActualPostID(i.EmployeeID)
  FROM (SELECT DISTINCT
      EmployeeID
    FROM INSERTED) i;

  UPDATE tbl_Employee
  SET ActualRankID = dbo.fn_GetActualRankID(d.EmployeeID)
     ,ActualPostID = dbo.fn_GetActualPostID(d.EmployeeID)
  FROM (SELECT DISTINCT
      EmployeeID
    FROM DELETED) d;

END;

GO

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

CREATE TRIGGER MESAchievementInsertUpdateDeleteTrigger
ON tbl_MESAchievement
AFTER INSERT, DELETE, UPDATE
AS
BEGIN
  UPDATE dbo.tbl_Employee
  SET ActualRankID = dbo.fn_GetActualRankID(ID),
      ActualPostID = dbo.fn_GetActualPostID(ID)
END

GO
