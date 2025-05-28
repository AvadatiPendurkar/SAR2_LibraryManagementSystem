create database SAR2_DB

use SAR2_DB

---create table book


 create TABLE Books (
    bookId INT identity(1,1) PRIMARY KEY,
    bookName VARCHAR(255) NOT NULL,
    authorName varchar(255),
    isbn VARCHAR(20) UNIQUE,
    genre varchar(200),
    quantity INT 	
);

select * from Books



---create table Users

CREATE TABLE Users (
    userId INT identity(1,1) PRIMARY KEY,
    firstName VARCHAR(100) NOT NULL,
    lastName VARCHAR(100) NOT NULL,
    email  VARCHAR(max) NOT NULL,
	pass varchar(20) not null,
	mobileNo varchar(20) not null
);

alter table Users add role varchar(50)
select * from Users
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

alter procedure sp_addUser1
@firstName varchar(200),
@lastName varchar(200),
@email varchar(max),
@pass varchar(20),
@mobileNo varchar(20),
@role varchar(50)
as 
begin
insert into Users(firstName, lastName, email, pass, mobileNo,role) values (@firstName, @lastName, @email, @pass, @mobileNo,@role);
end

exec sp_addUser1 @firstName='abc1',@lastName='demo1',@email='demo1@gmail.com',@pass='demo1',@mobileNo='9999999999'


---userlogin
alter PROCEDURE sp_LoginUser
    @email varchar(max),
    @pass varchar(20),
	@role varchar(50)
AS
BEGIN
    SELECT UserID, firstName, lastName, email, mobileNo,role
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

alter PROCEDURE sp_getAllUsers
AS
BEGIN
    SELECT userId, firstName, lastName, email, pass, mobileNo,role FROM Users
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

alter PROCEDURE sp_updateBook
    @bookId INT,
    @bookName NVARCHAR(100),
    @authorName NVARCHAR(100),
    @isbn NVARCHAR(50),
    @genre NVARCHAR(50),
    @quantity INT,
	@bookImage varbinary(max)
AS
BEGIN
    UPDATE Books
    SET
        bookName = @bookName,
        authorName = @authorName,
        isbn = @isbn,
        genre = @genre,
        quantity = @quantity,
		bookImage=@bookImage
    WHERE bookId = @bookId;
END

alter PROCEDURE sp_addBooks
    @bookName NVARCHAR(100),
    @authorName NVARCHAR(100),
    @isbn NVARCHAR(50),
    @genre NVARCHAR(50),
    @quantity INT,
	@bookImage varbinary(max)
AS
BEGIN
    INSERT INTO Books (bookName, authorName, isbn, genre, quantity,bookImage)
    VALUES (@bookName, @authorName, @isbn, @genre, @quantity,@bookImage);
END

exec sp_addBooks @bookName='shamchi Aai', @authorName='sane guruji', @isbn='ddtfthj3f5', @genre='jeevangatha', @quantity='12',@bookImage='a.jpg';

alter PROCEDURE sp_viewAllBooks
AS
BEGIN
    SELECT 
        bookId,
        bookName,
        authorName,
        isbn,
        genre,
        quantity,
		bookImage
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

select * from Books
select * from IssueBook
----------------------Issue book stored proc
alter proc sp_issueBook
@userId INT,
@bookId int,
@issueDate datetime,
@dueDate datetime,
@bookQty int ,
@status varchar(50)
as
begin
	INSERT INTO IssueBook (userId, bookId, issueDate, dueDate, bookQty, status)
	VALUES (@userId, @bookId, @issueDate, DATEADD(MONTH,1,GETDATE()), @bookQty, @status)
end

create PROCEDURE sp_updateIssueBook1
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


INSERT INTO Books (bookName, authorName, isbn, genre, quantity, bookImage)
VALUES
('To Kill a Mockingbird', 'Harper Lee', '9780061120084', 'Classic', 15, 
CONVERT(varbinary(max), 'iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z/C/HgAGgwJ/lK3Q6wAAAABJRU5ErkJggg==')),

('1984', 'George Orwell', '9780451524935', 'Dystopian', 20, 
CONVERT(varbinary(max), 'iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z/C/HgAGgwJ/lK3Q6wAAAABJRU5ErkJggg==')),

('The Great Gatsby', 'F. Scott Fitzgerald', '9780743273565', 'Classic', 12, 
CONVERT(varbinary(max), 'iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z/C/HgAGgwJ/lK3Q6wAAAABJRU5ErkJggg==')),

('Pride and Prejudice', 'Jane Austen', '9781503290564', 'Romance', 18, 
CONVERT(varbinary(max), 'iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z/C/HgAGgwJ/lK3Q6wAAAABJRU5ErkJggg=='));

('The Catcher in the Rye', 'J.D. Salinger', '9780316769488', 'Coming-of-age', 10, 
CONVERT(varbinary(max), 'iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z/C/HgAGgwJ/lK3Q6wAAAABJRU5ErkJggg==')),

('To the Lighthouse', 'Virginia Woolf', '9780156907392', 'Modernist', 8, 
CONVERT(varbinary(max), 'iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z/C/HgAGgwJ/lK3Q6wAAAABJRU5ErkJggg==')),

('Brave New World', 'Aldous Huxley', '9780060850524', 'Science Fiction', 14, 
CONVERT(varbinary(max), 'iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z/C/HgAGgwJ/lK3Q6wAAAABJRU5ErkJggg==')),

('The Hobbit', 'J.R.R. Tolkien', '9780547928227', 'Fantasy', 25, 
CONVERT(varbinary(max), 'iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z/C/HgAGgwJ/lK3Q6wAAAABJRU5ErkJggg==')),

('Fahrenheit 451', 'Ray Bradbury', '9781451673319', 'Dystopian', 16, 
CONVERT(varbinary(max), 'iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z/C/HgAGgwJ/lK3Q6wAAAABJRU5ErkJggg==')),

('Moby-Dick', 'Herman Melville', '9781503280787', 'Adventure', 9, 
CONVERT(varbinary(max), 'iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z/C/HgAGgwJ/lK3Q6wAAAABJRU5ErkJggg==')),

('The Lord of the Rings', 'J.R.R. Tolkien', '9780544003415', 'Fantasy', 22, 
CONVERT(varbinary(max), 'iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z/C/HgAGgwJ/lK3Q6wAAAABJRU5ErkJggg==')),

('The Alchemist', 'Paulo Coelho', '9780062315007', 'Fantasy', 30, 
CONVERT(varbinary(max), 'iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z/C/HgAGgwJ/lK3Q6wAAAABJRU5ErkJggg==')),

('The Da Vinci Code', 'Dan Brown', '9780307474278', 'Mystery', 17, 
CONVERT(varbinary(max), 'iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z/C/HgAGgwJ/lK3Q6wAAAABJRU5ErkJggg==')),

('The Hunger Games', 'Suzanne Collins', '9780439023481', 'Dystopian', 28, 
CONVERT(varbinary(max), 'iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z/C/HgAGgwJ/lK3Q6wAAAABJRU5ErkJggg==')),

('The Girl with the Dragon Tattoo', 'Stieg Larsson', '9780307269751', 'Crime', 13, 
CONVERT(varbinary(max), 'iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z/C/HgAGgwJ/lK3Q6wAAAABJRU5ErkJggg==')),

('Gone Girl', 'Gillian Flynn', '9780307588364', 'Thriller', 19, 
CONVERT(varbinary(max), 'iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z/C/HgAGgwJ/lK3Q6wAAAABJRU5ErkJggg==')),

('The Book Thief', 'Markus Zusak', '9780375831003', 'Historical Fiction', 11, 
CONVERT(varbinary(max), 'iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z/C/HgAGgwJ/lK3Q6wAAAABJRU5ErkJggg==')),

('The Silent Patient', 'Alex Michaelides', '9781250301697', 'Psychological Thriller', 23, 
CONVERT(varbinary(max), 'iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z/C/HgAGgwJ/lK3Q6wAAAABJRU5ErkJggg==')),

('Educated', 'Tara Westover', '9780399590504', 'Memoir', 16, 
CONVERT(varbinary(max), 'iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z/C/HgAGgwJ/lK3Q6wAAAABJRU5ErkJggg==')),

('Sapiens', 'Yuval Noah Harari', '9780062316097', 'Nonfiction', 27, 
CONVERT(varbinary(max), 'iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z/C/HgAGgwJ/lK3Q6wAAAABJRU5ErkJggg==')),

('Atomic Habits', 'James Clear', '9780735211292', 'Self-help', 35, 
CONVERT(varbinary(max), 'iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z/C/HgAGgwJ/lK3Q6wAAAABJRU5ErkJggg=='));

select * from Books