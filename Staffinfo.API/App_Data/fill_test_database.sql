INSERT INTO dbo.tbl_Rank(RankName, RankWeight, Term)
  VALUES ('������� �������', 1, 1),
          ('�������', 2, 2),
          ('������� �������', 3, 3),
          ('��������', 4, 3),
          ('���������', 5, 5),
          ('������� ���������', 6, 1),
          ('���������', 7, 2),
          ('������� ���������', 8, 3),
          ('�������', 9, 3),
          ('�����', 10, 4),
          ('������������', 11, 5);

GO

INSERT INTO dbo.tbl_Service (ServiceName, ServiceShortName, ServiceGroupId)
  VALUES ('������� ������', '��', 100),
        ('������ ������ � ����������������', '����',200),
        ('����� ������������ ����������', '���',300),
        ('����������� ������', '��', 400),
        ('���������-������������ ������', '���', 500),
        ('����������������� ������', '���',600),
        ('������ ���������� � ������������ ������', '����',700),
        ('�������-������������ ������', '���',800),
        ('��������� ���������� �����', NULL, 801),
        ('�������������� ���������', NULL, 802),
        ('��������� ����� � ����������', NULL, 803),
        ('�������-������������ ���������', NULL, 804);

GO

INSERT INTO dbo.tbl_Post (PostName, ServiceID, PostWeight)
  VALUES ('���. ������', 1, 1),	--������� ������
        ('���. �� ���', 1, 2),
        ('���. �� ���', 1, 2),
        ('���. �� ����', 1, 2),
        ('���. ���. �����', 1, 3),
        ('������� �������', 1, 4),
        ('��������� ��. ����', 2, 1),	--������ ������ � ����������������
        ('��������� ��. ����������������', 2, 2),
        ('���. ���. �����', 3, 1),	--����� ������������ ����������
        ('������� �������', 3, 2),
        ('���������', 3, 3),
        ('���������', 4, 1),	--����������� ������
        ('���� ������ ���. ������', 4, 2),
        ('���������-��������', 4, 3),
        ('��������-�������', 4, 4),
        ('���������', 5, 1),	--���������-������������ ������
        ('�������� ���������', 5, 2),
        ('���������-�������', 5, 3),
        ('��������-�������', 5, 4),
        ('���������', 6, 1),	--����������������� ������
        ('�������-������������', 6, 2),
        ('�������� ���������', 6, 3),
        ('������� ���������-������������', 6, 4),
        ('��������-������������', 6, 5),
        ('���������', 7, 1),	--������ ���. � ������������ ������
        ('�������', 7, 2),
        ('�������� ���������', 7, 3),
        ('������� ���������-�����-��������.', 7, 4),
        ('���������', 8, 1),	--�������-������������ ������
        ('��������-���������', 9, 1), --��������� ���������� �����
        ('������� ����������-�������', 10, 1),	--�������������� ���������
        ('���������-�������', 10, 2),
        ('�������', 11, 1),	--��������� ����� � ����������
        ('������ �����', 11, 2),
        ('��������-�������', 11, 3),
        ('�������� ���������', 12, 1),	--�������-������������ ���������
        ('������� ���������-��������', 12, 2),
        ('���������-��������', 12, 3),
        ('������� ��������-���������', 12, 4),
        ('��������-���������', 12, 5)
GO

INSERT INTO dbo.tbl_Address(City, Area, DetailedAddress, ZipCode) 
  VALUES ('������', '����������', '��. �����������, �.23, ��. 2', '222333'),
         ('������', '����������', '��. �������, �.21, ��.5', '215345'),
         ('��������', '����������', '��. ��������, �.3, ��.93', '241562'),
         ('���������', '����������', '��. ��������, �.11, ��.51', '235561'),
         ('������', '����������', '��. ��������, �.5, ��.4', '213312'),
         ('������', '����������', '��. ������, �.77, ��.89', '238093'),
         ('������', '����������', '��. �������, �.23, ��.75', '289754'),
         ('����', '����������', '��. ��������, �.34, ��.55', '235086'),
         ('�������', '����������', '��. �������� ������, �.43, ��.13', '241043'),
         ('����', '����������', '��. �������, �.41, ��.141', '453988'),
         ('������', '����������', '��. ����������, �.42, ��.34', '209283'),
         ('������', '����������', '��. �������, �.55, ��.4', '298774'),
         ('������', '����������', '��. ������, �.1, ��.15', '294855'),
         ('������', '����������', '��. ��������������, �.32, ��.54', '215432');

GO

INSERT INTO dbo.tbl_Passport(PassportNumber, PassportOrganization)
  VALUES ('HB7865463', '���������� ����'),
	 ('HB2495463', '���������� ����'),
	 ('HB7852456', '���������� ����'),
	 ('HB3893042', '���������� ����'),
	 ('HB1354675', '���������� ����'),
	 ('HB7353223', '���������� ����'),
	 ('HB8288383', '���������� ����'),
	 ('HB7737228', '���������� ����'),
	 ('HB8984857', '���������� ����'),
	 ('HB1786232', '���������� ����'),
	 ('HB4788524', '���������� ����'),
	 ('HB3664234', '���������� ����'),
	 ('HB8304983', '���������� ����'),
	 ('HB8263567', '���������� ����');


GO

INSERT INTO dbo.tbl_Employee(EmployeeFirstname, EmployeeLastname, EmployeeMiddlename, BirthDate, PassportID, AddressID, RetirementDate, Description)
  VALUES ('����', '�����', '����������', '1974-10-01', 1, 1, NULL, NULL),
	('����', '���������', '����������', '1975-02-04', 2, 2, NULL, NULL),
	('������', '������', '����������', '1969-01-04', 3, 3, NULL, NULL),
	('�������', '������', '��������', '1974-04-06', 4, 4, NULL, NULL),
	('�����', '������', '��������', '1981-04-03', 5, 5, NULL, NULL),
	('������', '����', '��������', '1983-04-06', 6, 6, NULL, NULL),
	('����', '�������', '��������', '1981-03-11', 7, 7, NULL, NULL),
	('�����', '������', '����������', '1984-11-09', 8, 8, NULL, NULL),
	('�����', '������', '���������', '1983-04-06', 9, 9, NULL, NULL),
	('�����', '��������', '����������', '1989-04-03', 10, 10, NULL, NULL),
	('�������', '��������', '����������', '1991-04-08', 11, 11, NULL, NULL),
	('����', '������', '����������', '1987-05-01', 12, 12, NULL, NULL),
	('�������', '������', '����������', '1980-12-06', 13, 13, NULL, NULL),
	('��', '������', '��������', '1979-09-03', 14, 14, NULL, NULL);

GO

INSERT INTO dbo.tbl_Location(LocationName, Address, Description)
VALUES ('���������� ����', '��������� 59', '��'),
	('���������� �� 15', '��������� 59', '��'),
	('���������� �� 7', '��������� 59', '��'),
	('��������� �� 3', '��������� 59', '��'),
	('����������� �� 11', '��������� 59', '��'),
	('���������� �� 1', '��������� 59', '��');

GO

INSERT INTO dbo.tbl_MESAchievement (EmployeeID, LocationID, StartDate, FinishDate, PostID, RankID, Description)
 VALUES (1, 1, '2000-04-05', NULL, 1, 1, 'xz'),
	(2, 2, '2000-04-05', '2010-05-06', 8, 2, 'xz'),
	(3, 3, '2000-04-05', '2001-05-06', 3, 3, 'xz'),
	(4, 4, '2000-04-05', '2007-05-06', 17, 4, 'xz');

GO

INSERT INTO dbo.tbl_Notification (Author, Title, Details, DueDate)
  VALUES ('qwerty', '����������� 1', '������ �������� �����������', '2016-12-12'),
      ('qwerty', '����������� 2', '������ �������� �����������', '2016-12-11'),
      ('qwerty', '����������� 2', '������ �������� �����������', '2016-11-12');

GO

INSERT INTO dbo.tbl_OutFromOffice (EmployeeID, startdate, FinishDate, Cause, Description)
  VALUES (1, '2000-04-05', '2010-05-06', 'S', 'aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa'),
  (2, '2000-04-05', '2010-05-06', 'S', ''),
  (3, '2000-04-05', '2010-05-06', 'S', ''),
  (1, '2000-04-05', '2010-05-06', 'D', '');

GO

INSERT INTO dbo.tbl_GratitudesAndPunishment(Title, EmployeeID, ItemType, Date, Description, AwardOrFine)
  VALUES ('�� �������� ������', 4, 'G', '2016-12-03', '������', 50000),
      ('�� �������� ������', 2, 'V', '2016-02-03', '������', 50000),
      ('�� �������� ������', 2, 'G', '2016-12-03', '������', 50000),
      ('�� �������� ������', 3, 'V', '2016-12-03', '������', 50000),
      ('�� �������� ������',2, 'G', '2016-12-03', '������', 50000);

GO

INSERT INTO dbo.tbl_Sertification(EmployeeID, DueDate, Description)
  VALUES (1, '2010-12-10', 'description1'),
      (1, '2010-11-10', 'description2');

GO