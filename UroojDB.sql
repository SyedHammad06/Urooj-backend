create database urooj_database
use urooj_database

CREATE TABLE USERS(
	Id INT IDENTITY(100,1) PRIMARY KEY,
	UserId NVARCHAR(10) Unique,
	EncryptedPassword NVARCHAR(50),
	ModifiedDate Date,
	IsAdmin Bit,
	IsActive BIT,
	InfoId INT 
)

CREATE TABLE USERINFO(
	InfoId INT IDENTITY(100,1) PRIMARY KEY,
	FName NVarchar(50),
	MName NVarchar(50),
	LName NVarchar(50),
	UserAddress NVarchar(50),
	Phote VARBINARY(MAX),
	Adhaar NVarchar(15),
	City NVARCHAR(50),
	[State] NVARCHAR(50),
	Country NVARCHAR(50),
	PersonalEmail NVarchar(50) Unique,
	IsActive BIT
)

ALTER TABLE USERS
ADD CONSTRAINT FK_USERS_USERINFO
FOREIGN KEY (InfoId) REFERENCES USERINFO(InfoId);

CREATE TABLE BOOKS(
	BookId INT IDENTITY(1,1) PRIMARY KEY,
	BookName NVarchar(50),
	BookDescription NVarchar(300),
	BookContent NVarchar(MAX),
	[Subject] NVarchar(50),
	class NVarchar(10),
	HProgram NVarchar(10),
	IsActive BIT,
	BookUrl NVARCHAR(MAX),
	Modified Date,
	ModifiedBy NVarChAR(10)
)

CREATE TABLE STATIONERY(
	StationaryId INT IDENTITY(1,1) PRIMARY KEY,
	Title NVarchar(50),
	StationaryDescription NVarchar(50),
	StationaryPrice NVarchar(50),
	IsActice BIT,
	StationaryUrl NVARCHAR(MAX),
	Modified Date,
	ModifiedBy NVarChAR(10)
)


-- Alter the BOOKS table to add ModifiedBy
ALTER TABLE BOOKS
ADD ModifiedBy NVARCHAR(10);

-- Alter the STATIONERY table to add ModifiedBy
ALTER TABLE STATIONERY
ADD ModifiedBy NVARCHAR(10);
