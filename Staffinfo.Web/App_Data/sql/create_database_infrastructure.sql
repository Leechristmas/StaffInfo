USE StaffinfoTestDB;

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

ALTER TABLE dbo.tbl_Employee
  DROP CONSTRAINT fk_Employee_Passport,
       CONSTRAINT fk_Employee_Address;

GO

DROP TABLE dbo.tbl_MESAchievement;
DROP TABLE dbo.tbl_WorkTerm;
DROP TABLE dbo.tbl_Location;
DROP TABLE dbo.tbl_Passport;
DROP TABLE dbo.tbl_Address;
DROP TABLE dbo.tbl_Employee;
DROP TABLE dbo.tbl_Rank;
DROP TABLE dbo.tbl_Post;
DROP TABLE dbo.tbl_Service;
DROP TABLE dbo.tbl_MilitaryService;

GO

DROP FUNCTION dbo.fn_GetActualRankID;
DROP FUNCTION dbo.fn_GetActualPostID;

GO

-----------------------------
--TABLES---------------------
-----------------------------

CREATE TABLE dbo.tbl_Rank(
  ID INT IDENTITY(1,1) PRIMARY KEY,
  RankName NVARCHAR(60) NOT NULL,
  RankWeight INT NOT NULL DEFAULT 0,
  Term INT NOT NULL --months
  );
GO
CREATE TABLE dbo.tbl_Location(
  ID INT IDENTITY(1,1) PRIMARY KEY,
  LocationName NVARCHAR(120) NOT NULL,
  Address NVARCHAR(200),
  Description NVARCHAR(500)
  );
GO

CREATE TABLE dbo.tbl_Service(
  ID INT IDENTITY(1,1) PRIMARY KEY,
  ServiceName NVARCHAR(60) NOT NULL,
  ServiceShortName NVARCHAR(10),
  ServiceGroupId INT NOT NULL,
  Description NVARCHAR(500)
  );
GO

CREATE TABLE dbo.tbl_Post(
  ID INT IDENTITY(1,1) PRIMARY KEY,
  PostName NVARCHAR(60) NOT NULL,
  ServiceID INT NOT NULL,
  PostWeight INT NOT NULL DEFAULT 0
);

ALTER TABLE dbo.tbl_Post
  ADD CONSTRAINT fk_Post_Service
  FOREIGN KEY (ServiceID) REFERENCES dbo.tbl_Service;
GO

CREATE TABLE dbo.tbl_Address(
  ID INT IDENTITY(1,1) PRIMARY KEY,
  City NVARCHAR(60) NOT NULL,
  Street NVARCHAR(60) NOT NULL,
  House NVARCHAR(10) NOT NULL,
  Flat NVARCHAR(5)
);
GO

CREATE TABLE dbo.tbl_Passport(
  ID INT IDENTITY(1,1) PRIMARY KEY,
  PassportNumber NVARCHAR(9) NOT NULL,
  PassportOrganization NVARCHAR(120) NOT NULL
  );
GO

CREATE TABLE dbo.tbl_Employee(
  ID INT IDENTITY(1,1) PRIMARY KEY,
  EmployeeFirstname NVARCHAR(60) NOT NULL,
  EmployeeLastname NVARCHAR(60) NOT NULL,
  EmployeeMiddlename NVARCHAR(60) NOT NULL,
  BirthDate DATETIME NOT NULL,
  PassportID INT NOT NULL,
  AddressID INT NOT NULL,
  ActualRankID INT,
  ActualPostID INT,
  IsPensioner BIT NOT NULL DEFAULT 0,
  EmployeePhoto VARBINARY(MAX),
  PhotoMimeType NVARCHAR(10)
  --TODO
);

ALTER TABLE dbo.tbl_Employee
  ADD CONSTRAINT fk_Employee_Address
      FOREIGN KEY (AddressID) REFERENCES tbl_Address,
      CONSTRAINT fk_Employee_Passport
      FOREIGN KEY (PassportID) REFERENCES tbl_Passport;
GO

--achievement list of ministry of emergency situations service of the employee
CREATE TABLE dbo.tbl_MESAchievement(
  ID INT IDENTITY(1,1) PRIMARY KEY,
  EmployeeID INT NOT NULL,
  LocationID INT NOT NULL,
  StartDate DATETIME NOT NULL,
  FinishDate DATETIME NOT NULL,
  PostID INT NOT NULL,
  RankID INT NOT NULL,
  Description NVARCHAR(500)
);

ALTER TABLE dbo.tbl_MESAchievement
  ADD CONSTRAINT fk_MESAchievement_Employee
        FOREIGN KEY (EmployeeID) REFERENCES dbo.tbl_Employee,
      CONSTRAINT fk_MESAchievement_Location
        FOREIGN KEY (LocationID) REFERENCES dbo.tbl_Location,
      CONSTRAINT fk_MESAchievement_Rank
        FOREIGN KEY (RankID) REFERENCES dbo.tbl_Rank,
      CONSTRAINT fk_MESAchievement_Post
        FOREIGN KEY (PostID) REFERENCES dbo.tbl_Post,
      CONSTRAINT unq_MESAchievement
        UNIQUE (EmployeeID, StartDate);
GO

CREATE TABLE dbo.tbl_WorkTerm(
  ID INT IDENTITY(1,1) PRIMARY KEY,
  EmployeeID INT NOT NULL,
  LocationID INT NOT NULL,
  Post NVARCHAR(120) NOT NULL,
  StartDate DATETIME NOT NULL,
  FinishDate DATETIME NOT NULL,
  Description NVARCHAR(500)
);

ALTER TABLE dbo.tbl_WorkTerm
  ADD CONSTRAINT fk_WorkTerm_Employee
        FOREIGN KEY (EmployeeID) REFERENCES dbo.tbl_Employee,
      CONSTRAINT fk_WorkTerm_Location
        FOREIGN KEY (LocationID) REFERENCES dbo.tbl_Location,
      CONSTRAINT unq_WorkTerm
        UNIQUE (EmployeeID, StartDate);
GO


CREATE TABLE dbo.tbl_MilitaryService(
  ID INT IDENTITY(1,1) PRIMARY KEY,
  EmployeeID INT NOT NULL,
  LocationID INT NOT NULL,
  Rank NVARCHAR(120) NOT NULL,
  StartDate DATETIME NOT NULL,
  FinishDate DATETIME NOT NULL,
  Description NVARCHAR(500)
);

ALTER TABLE dbo.tbl_MilitaryService
  ADD CONSTRAINT fk_MilitaryService_Employee
        FOREIGN KEY (EmployeeID) REFERENCES dbo.tbl_Employee,
      CONSTRAINT fk_MilitaryService_Location
        FOREIGN KEY (LocationID) REFERENCES dbo.tbl_Location,
      CONSTRAINT unq_MilitaryService
        UNIQUE (EmployeeID, StartDate);

GO

------------------------------
--PROCEDURES
------------------------------


------------------------------
--FUNCTIONS
------------------------------
--Returns the actual rank id for the specified user
CREATE FUNCTION dbo.fn_GetActualRankID(@EmployeeID INT)
  RETURNS INT
AS
BEGIN	
  DECLARE @RankID INT;
  
  SELECT @RankID = RankID 
  FROM dbo.tbl_MESAchievement tml 
  WHERE tml.EmployeeID = @EmployeeID AND 
        tml.StartDate = (SELECT MAX(StartDate) FROM dbo.tbl_MESAchievement tml1 WHERE tml1.EmployeeID = @EmployeeID);
  
  RETURN @RankID;
END;

GO

--Returns the actual post id for the specified user
CREATE FUNCTION dbo.fn_GetActualPostID(@EmployeeID INT)
  RETURNS INT
AS
BEGIN	
  DECLARE @PostID INT;
  
  SELECT @PostID = PostID 
  FROM dbo.tbl_MESAchievement tml 
  WHERE tml.EmployeeID = @EmployeeID AND 
        tml.StartDate = (SELECT MAX(StartDate) FROM dbo.tbl_MESAchievement tml1 WHERE tml1.EmployeeID = @EmployeeID);
  
  RETURN @PostID;
END

GO
------------------------------
--TRIGGERS
------------------------------
--Inserting into Employee table
CREATE TRIGGER EmployeeInsertTrigger on tbl_Employee
  INSTEAD OF INSERT
AS
BEGIN
  INSERT INTO tbl_Employee (EmployeeFirstname, EmployeeLastname, EmployeeMiddlename, BirthDate, PassportID, AddressID, ActualRankID, ActualPostID, IsPensioner)
    SELECT EmployeeFirstname, EmployeeLastname, EmployeeMiddlename, BirthDate, PassportID, AddressID, dbo.fn_GetActualRankID(ID), dbo.fn_GetActualPostID(ID), IsPensioner
    FROM inserted
END;

GO

CREATE TRIGGER EmployeeDeleteTrigger ON tbl_Employee
  AFTER DELETE
AS
  BEGIN
    SELECT * FROM DELETED;
    SELECT * FROM INSERTED;
    --TODO
--  	DELETE ta FROM tbl_Address ta, INSERTED i WHERE ta.ID = i.AddressID;
--    DELETE tp FROM tbl_Passport tp , INSERTED i WHERE tp.ID = i.PassportID;
  END

--CREATE TRIGGER MESAchievementInsertTrigger ON tbl_MESAchievement
--  AFTER INSERT,DELETE,UPDATE
--AS
--BEGIN
--	
--END
--
--GO
