create database SAR2_DB

use SAR2_DB

---create table book
 CREATE TABLE Books (
    bookId INT identity(1,1) PRIMARY KEY,
    bookName VARCHAR(255) NOT NULL,
    authorName varchar(255),
    isbn VARCHAR(20) UNIQUE,
    genre varchar(200),
    quantity INT,  
);

---create table Users

CREATE TABLE Users (
    userId INT identity(1,1) PRIMARY KEY,
    firstName VARCHAR(100) NOT NULL,
    lastName VARCHAR(100) NOT NULL,
    email  VARCHAR(max) NOT NULL,
	pass varchar(20) not null,
	mobileNo varchar(20) not null
);

----create table issueBook

CREATE TABLE IssueBook (
    issueId INT identity(1,1) PRIMARY KEY,
	userId INT FOREIGN KEY (userId) REFERENCES Users(userId),
	bookId int	FOREIGN KEY (bookId) REFERENCES  Books(bookId),
    issueDate datetime not null,
	dueDate datetime,
	returnDate datetime,
	bookQty int,
    status varchar(50),
)

---create table manageruser

create table Managers
 (
 mId int primary key identity(1,1),
 mfirstName varchar(200),
 mlastName varchar(200),
 email varchar(max),
 mobileNo varchar(20),
 pass varchar(20)
 )

 ---registerUser

create procedure sp_addUser1
@firstName varchar(200),
@lastName varchar(200),
@email varchar(max),
@pass varchar(20),
@mobileNo varchar(20)
as 
begin
insert into Users(firstName, lastName, email, pass, mobileNo) values (@firstName, @lastName, @email, @pass, @mobileNo);
end

exec sp_addUser1 @firstName='abc',@lastName='demo',@email='demo@gmail.com',@pass='demo',@mobileNo='9999999999'


---userlogin
CREATE PROCEDURE sp_LoginUser
    @email varchar(max),
    @pass varchar(20)
AS
BEGIN
    SELECT UserID, firstName, lastName, email, mobileNo
    FROM Users
    WHERE email = @email AND pass = @pass
END

--- update user
CREATE PROCEDURE sp_updateUser1
    @userId INT,
    @firstName NVARCHAR(50),
    @lastName NVARCHAR(50),
    @email NVARCHAR(100),
    @pass NVARCHAR(50),
    @mobileNo NVARCHAR(15)
AS
BEGIN
    UPDATE Users
    SET 
        firstName = @firstName,
        lastName = @lastName,
        email = @email,
        pass = @pass,
        mobileNo = @mobileNo
    WHERE userId = @userId
END
select * from Users

CREATE PROCEDURE sp_getAllUsers
AS
BEGIN
    SELECT userId, firstName, lastName, email, pass, mobileNo FROM Users
END

CREATE PROCEDURE sp_deleteUser1
    @userId INT
AS
BEGIN
    DELETE FROM Users WHERE userId = @userId
END


create PROCEDURE sp_getUserById
    @userId INT
AS
BEGIN
    SELECT userId, firstName, lastName, email, pass, mobileNo
    FROM Users
    WHERE userId = @userId
END

----------------------books stored proc

CREATE PROCEDURE sp_updateBook
    @bookId INT,
    @bookName NVARCHAR(100),
    @authorName NVARCHAR(100),
    @isbn NVARCHAR(50),
    @genre NVARCHAR(50),
    @quantity INT
AS
BEGIN
    UPDATE Books
    SET
        bookName = @bookName,
        authorName = @authorName,
        isbn = @isbn,
        genre = @genre,
        quantity = @quantity
    WHERE bookId = @bookId;
END

CREATE PROCEDURE sp_addBooks
    @bookName NVARCHAR(100),
    @authorName NVARCHAR(100),
    @isbn NVARCHAR(50),
    @genre NVARCHAR(50),
    @quantity INT
AS
BEGIN
    INSERT INTO Books (bookName, authorName, isbn, genre, quantity)
    VALUES (@bookName, @authorName, @isbn, @genre, @quantity);
END

CREATE PROCEDURE sp_viewAllBooks
AS
BEGIN
    SELECT 
        bookId,
        bookName,
        authorName,
        isbn,
        genre,
        quantity
    FROM Books;
END

CREATE PROCEDURE sp_deleteBook
    @bookId INT
AS
BEGIN
    DELETE FROM Books
    WHERE bookId = @bookId;
END

CREATE PROCEDURE sp_viewBookById
    @bookId INT
AS
BEGIN
    SELECT 
        bookId,
        bookName,
        authorName,
        isbn,
        genre,
        quantity
    FROM Books
    WHERE bookId = @bookId;
END

----------------------Issue book stored proc
alter proc sp_issueBook
@issueId INT,
@userId INT,
@bookId int,
@issueDate datetime,
@dueDate datetime,
@bookQty int ,
@status varchar(50)
as
begin
	INSERT INTO IssueBook (issueId, userId, bookId, issueDate, dueDate, bookQty, status)
	VALUES (@issueId, @userId, @bookId, @issueDate, DATEADD(MONTH,1,GETDATE()), @bookQty, @status)
end

CREATE PROCEDURE sp_updateIssueBook
    @issueId INT,
	@userId INT,
	@bookId int,
	@bookQty int
AS
BEGIN
    UPDATE IssueBook
    SET
		userId = @userId,
		bookId = @bookId,
        bookQty = @bookQty
    WHERE issueId = @issueId;
END

CREATE PROCEDURE sp_viewAllIssueBook
as
BEGIN
    SELECT issueId, userId, bookId, issueDate, dueDate, returnDate, bookQty, status
    FROM IssueBook;
END

CREATE PROCEDURE sp_deleteIssueBook
    @issueId INT
AS
BEGIN
    DELETE FROM IssueBook
    WHERE issueId = @issueId;
END

CREATE PROCEDURE sp_viewIssueBookById
    @issueId INT
AS
BEGIN
    SELECT 
        issueId, userId, bookId, issueDate, dueDate, returnDate, bookQty, status
    FROM IssueBook
    WHERE issueId = @issueId;
END

exec sp_issueBook @issueId=1, @userId=1, @bookId=1, @issueDate= GetdATE(), @dueDate=DATEADD(MONTH,1,GETDATE()), @bookQty=10, @status='active';


----------------------------Manager store Procedure

create procedure sp_AddManager
@mfirstName varchar(50),
@mlastName varchar(50),
@email varchar(50),
@pass varchar(50),
@mobileNo varchar(12)
as
begin
	insert into Managers(mfirstName,mlastName,email,pass,mobileNo)
	values(@mfirstName, @mlastName, @email, @pass, @mobileNo);
end

create procedure sp_updateManager
@mId int,
@mfirstName varchar(50),
@mlastName varchar(50),
@email varchar(50),
@pass varchar(50),
@mobileNo varchar(12)
 as
 begin
	update Managers
	 set
		mfirstName = @mfirstName,
		mlastName = @mlastName,
		email = @email,
		pass = @pass,
		mobileNo = @mobileNo
	where mId = @mId;
end

create procedure sp_viewAllManager
as 
begin 
	select * from Managers;
end

create procedure sp_viewManagerById
@mId int 
as 
begin
	select mId,mfirstName,mlastName,email,pass,mobileNo from Managers  
	where mId = @mId;
end 

create procedure sp_deleteManager
@mId int
as 
begin
	delete from Managers
	where mId = @mId;
end

