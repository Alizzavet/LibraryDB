/* �������� ������ */ -----------------------------------------------------------------------------------------------


/* �������� ������ */ -----------------------------------------------------------------------------------------------
/*
DROP TABLE Acts_Books;
DROP TABLE Books_Publishers;
DROP TABLE Acts;
DROP TABLE Books;
DROP TABLE Works_Books;
DROP TABLE Books_Shelves;
DROP TABLE Works_Authors;
DROP TABLE Works_Genres;
DROP TABLE LibraryEvents;
DROP TABLE Publishers;
DROP TABLE Genres;
DROP TABLE Authors;
DROP TABLE Works;
DROP TABLE Shelves;
DROP TABLE Sections;
DROP TABLE LibraryRooms;
DROP TABLE Administrators;
DROP TABLE Librarians;
DROP TABLE Users;
DROP TABLE Subscriptions;
*/ 
CREATE TABLE Users (
    UserID INT PRIMARY KEY,
    LastName NVARCHAR(50) NOT NULL,
    FirstName NVARCHAR(50) NOT NULL,
    MiddleName NVARCHAR(50),
	UserLogin NVARCHAR(50),
    UserPassword NVARCHAR(50)
);

CREATE TABLE Librarians (
    LibrarianID INT PRIMARY KEY,
    UserID INT, FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

CREATE TABLE Administrators (
    AdministratorID INT PRIMARY KEY,
    UserID INT, FOREIGN KEY (UserID) REFERENCES Users(UserID),
    LibrarianID INT, FOREIGN KEY (LibrarianID) REFERENCES Librarians(LibrarianID) 
);

CREATE TABLE LibraryEvents (
	EventID INT PRIMARY KEY,
	EventType NVARCHAR(50) NOT NULL,
	EventName NVARCHAR(50) NOT NULL,
	EventDate DATE NOT NULL,
	LibrarianID INT, FOREIGN KEY (LibrarianID) REFERENCES Librarians(LibrarianID) ON DELETE CASCADE
);

CREATE TABLE Subscriptions (
	SubscriptionID INT PRIMARY KEY,
	LastName NVARCHAR(50) NOT NULL,
    FirstName NVARCHAR(50) NOT NULL,
    MiddleName NVARCHAR(50),
    SubscriptionPasport NVARCHAR(50),
	VisitorPhoneNumber VARCHAR(20),
	SubscriptionsType VARCHAR(20)
);

CREATE TABLE LibraryRooms (
	LibraryRoomID INT PRIMARY KEY,
	LibraryRoomName NVARCHAR(50)
);


----------------------------------------------------------------------------- ��������� ��� ������ -----------------
CREATE PROCEDURE GetSectionsWithRoomNames
AS
SELECT Sections.SectionID, LibraryRooms.LibraryRoomName 
FROM Sections 
INNER JOIN LibraryRooms ON Sections.LibraryRoomID = LibraryRooms.LibraryRoomID
GO

----------------------------------------------------------------------------------------------------------------------

CREATE TABLE Sections (
	SectionID INT PRIMARY KEY,
	LibraryRoomID INT, FOREIGN KEY (LibraryRoomID) REFERENCES LibraryRooms(LibraryRoomID)
);


--------------------------------------------------------------- ��������� ��� ����� ----------------------------------
CREATE PROCEDURE GetShelvesWithSectionAndRoomNames
AS
SELECT Shelves.ShelfID, Sections.SectionID, LibraryRooms.LibraryRoomName 
FROM Shelves 
INNER JOIN Sections ON Shelves.SectionID = Sections.SectionID 
INNER JOIN LibraryRooms ON Sections.LibraryRoomID = LibraryRooms.LibraryRoomID
GO

---------------------------------------------------------------------------------------------------------------------

CREATE TABLE Shelves (
	ShelfID INT PRIMARY KEY,
	SectionID INT, FOREIGN KEY (SectionID) REFERENCES Sections(SectionID)
);

CREATE TABLE Works (
	WorkID INT PRIMARY KEY,
	WorkName NVARCHAR(100)
);

CREATE TABLE Books (
	BookID INT PRIMARY KEY,
	BookName NVARCHAR(100),
	PublisherDate DATE NOT NULL
);

CREATE TABLE Works_Books (
	Works_BooksID INT PRIMARY KEY,
	WorkID INT, FOREIGN KEY (WorkID) REFERENCES Works(WorkID) ON DELETE CASCADE,
	BookID INT, FOREIGN KEY (BookID) REFERENCES Books(BookID) ON DELETE CASCADE
);


CREATE TABLE Books_Shelves (
	Books_ShelvesID INT PRIMARY KEY,
	BookID INT, FOREIGN KEY (BookID) REFERENCES Books(BookID),
	ShelfID INT, FOREIGN KEY (ShelfID) REFERENCES Shelves(ShelfID)
);

CREATE TABLE Authors (
	AuthorID INT PRIMARY KEY,
	LastName NVARCHAR(50) NOT NULL,
    FirstName NVARCHAR(50) NOT NULL,
    MiddleName NVARCHAR(50),
	Pseudonym NVARCHAR(50)
);

CREATE TABLE Works_Authors (
	Works_AuthorsID INT PRIMARY KEY,
	WorkID INT, FOREIGN KEY (WorkID) REFERENCES Works(WorkID) ON DELETE CASCADE,
	AuthorID INT, FOREIGN KEY (AuthorID) REFERENCES Authors(AuthorID) ON DELETE CASCADE,
);

CREATE TABLE Genres (
	GenreID INT PRIMARY KEY,
	GenreName NVARCHAR(50) NOT NULL
);

CREATE TABLE Works_Genres (
	Works_GenresID INT PRIMARY KEY,
	WorkID INT, FOREIGN KEY (WorkID) REFERENCES Works(WorkID) ON DELETE CASCADE,
	GenreID INT, FOREIGN KEY (GenreID) REFERENCES Genres(GenreID) ON DELETE CASCADE
);

CREATE TABLE Publishers (
	PublisherID INT PRIMARY KEY,
	PublisherName NVARCHAR(50) NOT NULL
);

CREATE TABLE Books_Publishers (
	Books_PublishersID INT PRIMARY KEY,
	BookID INT, FOREIGN KEY (BookID) REFERENCES Books(BookID) ON DELETE CASCADE,
	PublisherID INT, FOREIGN KEY (PublisherID) REFERENCES Publishers(PublisherID) ON DELETE CASCADE
);

CREATE TABLE Acts (
	ActID INT PRIMARY KEY,
	LibrarianID INT, FOREIGN KEY (LibrarianID) REFERENCES Librarians(LibrarianID) ON DELETE CASCADE,
	SubscriptionID INT, FOREIGN KEY (SubscriptionID) REFERENCES Subscriptions(SubscriptionID) ON DELETE CASCADE,
	ActionType NVARCHAR(20) NOT NULL,
	EventDate DATE NOT NULL
);

CREATE TABLE Acts_Books (
	Acts_BooksID INT PRIMARY KEY,
	BookID INT, FOREIGN KEY (BookID) REFERENCES Books(BookID) ON DELETE CASCADE,
	ActID INT, FOREIGN KEY (ActID) REFERENCES Acts(ActID) ON DELETE CASCADE
);


/* ���������� ������ */ -----------------------------------------------------------------------------------------------

INSERT INTO Users (UserID, LastName, FirstName, MiddleName, UserPassword, UserLogin)
VALUES 
  (1, '������', '�����', NULL, '35203003', 'IvanovAdmin'),
  (2, '������', '����', NULL, 'securepass', 'PetrovLibra'),
  (3, '��������', '�����', '�������������', '123456', 'SidorovaLibra'),
  (4, '���������', '���������', '����������', 'l6w2ynvpebof_hIq54fg', 'FilanenkoAdmin'),
  (5, '���������', '�����', '������������', 'leomsupremacy', 'StepulionokAdmin');

-- ������ ���������� ������� Librarians
INSERT INTO Librarians (LibrarianID, UserID)
VALUES 
  (1, 1),
  (2, 2),
  (3, 3),
  (4, 5);

-- ������ ���������� ������� Administrators
INSERT INTO Administrators (AdministratorID, UserID, LibrarianID)
VALUES 
  (1, 1, 1),
  (2, 4, NULL),
  (3, 5, 4);

-- ������ ���������� ������� LibraryEvents
INSERT INTO LibraryEvents (EventID, EventType, EventName, EventDate, LibrarianID)
VALUES 
  (1, 'Meeting', '������������ ��������', '2023-09-28', 1),
  (2, 'Presentation', '����������� ����� ����������� ����� ��������', '2023-10-05', 2),
  (3, 'Presentation', '����� ����� ������', '2023-10-12', 3),
  (4, 'Presentation', '����������� �������� ���������� ����', '2023-11-01', 2),
  (5, 'Quest', '���� ������', '2023-12-25', 1),
  (6, 'Meeting', '��������� ����� � 3', '2024-01-13', 3);

-- ������ ���������� ������� Subscriptions
INSERT INTO Subscriptions (SubscriptionID, LastName, FirstName, MiddleName, SubscriptionPasport, VisitorPhoneNumber, SubscriptionsType)
VALUES 
  (1, '��������', '�������', '���������', 'AB132123456', '+375290493566', '��������'),
  (2, '��������', '�����', NULL, 'CD786749012', NULL, '�������'),
  (3, '�������', '���������', '��������', 'CD789422812', '+375333351995', '�������'),
  (4, '����������', '�������', '���������', 'CD786239012', '+375333361732', '��������'),
  (5, '���������', '�����', '�������������', 'CD786749012', '+375443356732', '��������'),
  (6, '��������', '�����', '����������', 'AB7823449012', NULL, '�������');

-- ������ ���������� ������� LibraryRooms
INSERT INTO LibraryRooms (LibraryRoomID, LibraryRoomName)
VALUES 
  (1, '��������� ���'),
  (2, '��� ��� ������ �� ���');

-- ������ ���������� ������� Sections
INSERT INTO Sections (SectionID, LibraryRoomID)
VALUES 
  (1, 1),
  (2, 1),
  (3, 2),
  (4, 2),
  (5, 2);

-- ������ ���������� ������� Shelves
INSERT INTO Shelves (ShelfID, SectionID)
VALUES 
  (1, 1),
  (2, 1),
  (3, 2),
  (4, 2),
  (5, 4),
  (6, 5);

-- ������ ���������� ������� Works
INSERT INTO Works (WorkID, WorkName)
VALUES 
  (1, '���� ���� "����� � ���"'),
  (2, '����� "�����"'),
  (3, '������� "������� ������"'),
  (4, '����� "������ � ���������"'),
  (5, '������� "���������� ��������� � ����"'),
  (6, '���� ���� "����-�������"'),
  (7, '���� ���� "��������� �����"'),
  (8, '������������� "������ ������"'),
  (9, '������������� "����� ������"');

-- ������ ���������� ������� Books
INSERT INTO Books (BookID, BookName, PublisherDate)
VALUES 
  (1, '����� � ���. ��� 1', '2010-05-15'),
  (2, '����� � ���. ��� 2', '2010-06-20'),
  (3, '�����', '1985-02-15'),
  (4, '����-�������. ����� �����. ��� 1', '2010-10-25'),
  (5, '����-�������. ����� � ���. ��� 2', '2011-02-25'),
  (6, '���������� ���������', '2015-06-07'),
  (7, '���������� �����', '2018-03-30'),
  (8, '��������� �����. ��� 1', '2016-06-03'),
  (9, '��������� �����. ��� 2', '2016-10-10'),
  (10, '������ � ���������', '2001-10-25'),
  (11, '���������� ��������� � ����', '2023-09-24');

-- ������ ���������� ������� Works_Books
INSERT INTO Works_Books (Works_BooksID, WorkID, BookID)
VALUES 
  (1, 1, 1),
  (2, 1, 2),
  (3, 3, 6),
  (4, 4, 6),
  (5, 4, 10),
  (6, 5, 11),
  (7, 6, 4),
  (8, 6, 5),
  (9, 7, 8),
  (10, 7, 8),
  (11, 8, 7),
  (12, 9, 7),
  (13, 2, 3);

-- ������ ���������� ������� Authors
INSERT INTO Authors (AuthorID, LastName, FirstName, MiddleName, Pseudonym)
VALUES 
  (1, '�������', '���', '����������', NULL),
  (2, '���������', '������', '�������', NULL),
  (3, '��������', '������', '�����������', NULL),
  (4, '������', '����', NULL, NULL),
  (5, '������', '���� ������� ����', NULL, NULL),
  (6, '�������', '��������', NULL, NULL),
  (7, '������', '������', '�������������', NULL),
  (8, '�������', '�������', NULL, '������ ����');

-- ������ ���������� ������� Works_Authors
INSERT INTO Works_Authors (Works_AuthorsID, WorkID, AuthorID)
VALUES 
  (1, 1, 1),
  (2, 2, 2),
  (3, 3, 3),
  (4, 4, 3),
  (5, 5, 8),
  (6, 6, 4),
  (7, 7, 5),
  (8, 8, 6),
  (9, 9, 7);

-- ������ ���������� ������� Genres
INSERT INTO Genres (GenreID, GenreName)
VALUES 
  (1, '���������'),
  (2, '�������'),
  (3, '���������'),
  (4, '��������'),
  (5, '������'),
  (6, '������'),
  (7, '�����'),
  (8, '�������� �����'),
  (9, '����'),
  (10, '�����������'),
  (11, '�����');

-- ������ ���������� ������� Works_Genres
INSERT INTO Works_Genres (Works_GenresID, WorkID, GenreID)
VALUES 
  (1, 1, 11),
  (2, 1, 4),
  (3, 2, 1),
  (4, 2, 5),
  (5, 3, 9),
  (6, 4, 8),
  (7, 4, 2),
  (8, 4, 6),
  (9, 4, 9),
  (10, 5, 1),
  (11, 5, 7),
  (12, 5, 3),
  (13, 6, 2),
  (14, 6, 10),
  (15, 6, 11),
  (16, 7, 2),
  (17, 7, 1),
  (18, 7, 10),
  (19, 8, 1),
  (20, 8, 5),
  (21, 9, 5);

-- ������ ���������� ������� Publishers
INSERT INTO Publishers (PublisherID, PublisherName)
VALUES 
  (1, '������������ "������"'),
  (2, '������������ "�����"'),
  (3, '������������ "����-����� �����"'),
  (4, '������������ "���"');

-- ������ ���������� ������� Books_Publishers
INSERT INTO Books_Publishers (Books_PublishersID, BookID, PublisherID)
VALUES 
  (1, 1, 1),
  (2, 2, 1),
  (3, 1, 4),
  (4, 2, 4),
  (5, 3, 1),
  (6, 4, 3),
  (7, 5, 3),
  (8, 6, 4),
  (9, 7, 1),
  (10, 8, 2),
  (11, 9, 2),
  (12, 8, 3),
  (13, 9, 3),
  (14, 10, 1),
  (15, 11, 2);

INSERT INTO Acts (ActID, LibrarianID, SubscriptionID, ActionType, EventDate)
VALUES 
  (1, 1, 1, 'Issue', '2023-09-29'),
  (2, 2, 2, 'Issue', '2023-09-29'),
  (3, 3, 1, 'Return', '2023-09-30'),
  (4, 1, 2, 'Return', '2023-10-10'),
  (5, 3, 4, 'Issue', '2023-10-10'),
  (6, 3, 4, 'Return', '2023-10-10'),
  (7, 1, 5, 'Issue', '2023-10-11'),
  (8, 2, 6, 'Issue', '2023-10-12'),
  (9, 2, 6, 'Issue', '2023-10-13');

INSERT INTO Acts_Books (Acts_BooksID, ActID, BookID)
VALUES 
  (1, 1, 3),
  (2, 2, 4),
  (3, 3, 3),
  (4, 4, 4),
  (5, 5, 7),
  (6, 5, 11),
  (7, 6, 11),
  (8, 7, 8),
  (9, 8, 3),
  (10, 9, 1);


  ------------------------------------------------------------------------------------------------------
  /* ������� */

  --- ������� ���� �������, � ������� 2 � ����� ������������ ----
  SELECT Authors.FirstName, Authors.LastName, COUNT(Works_Authors.WorkID) AS NumberOfWorks
FROM Authors
JOIN Works_Authors ON Authors.AuthorID = Works_Authors.AuthorID
GROUP BY Authors.FirstName, Authors.LastName
HAVING COUNT(Works_Authors.WorkID) >= 2;

 --- ������� ���� ���������� �����������, ������� ��� �� ������ ������� �� ����� ���������� �� �����, ������� ����� ---
 SELECT Subscriptions.LastName, Subscriptions.FirstName, Subscriptions.MiddleName, Subscriptions.VisitorPhoneNumber
FROM Subscriptions
JOIN Acts ON Subscriptions.SubscriptionID = Acts.SubscriptionID
JOIN Acts_Books ON Acts.ActID = Acts_Books.ActID
WHERE Acts_Books.BookID IN (
    SELECT DISTINCT Acts_Books.BookID
    FROM Acts_Books
    JOIN Acts ON Acts_Books.ActID = Acts.ActID
    WHERE Acts.ActionType = 'Issue'
)
AND NOT EXISTS (
    SELECT 1
    FROM Acts AS ReturnActs
    JOIN Acts_Books AS ReturnActs_Books ON ReturnActs.ActID = ReturnActs_Books.ActID
    WHERE ReturnActs.ActionType = 'Return'
    AND ReturnActs_Books.BookID = Acts_Books.BookID
    AND ReturnActs.SubscriptionID = Acts.SubscriptionID
);

 ---��� ����� ����� �������---
SELECT Books.BookName, Genres.GenreName
FROM Books
JOIN Works_Books ON Books.BookID = Works_Books.BookID
JOIN Works_Genres ON Works_Books.WorkID = Works_Genres.WorkID
JOIN Genres ON Works_Genres.GenreID = Genres.GenreID
WHERE Genres.GenreName = '�������';

 ---������� ���������������, ������� ��������� ����� �������������� � ������� �� ���---
 SELECT Users.FirstName, Users.LastName, Users.MiddleName
FROM Users
JOIN Administrators ON Users.UserID = Administrators.UserID
WHERE Administrators.LibrarianID IS NOT NULL;

---��� ������������, �������������� � �������---
SELECT Works.WorkName, Books.BookName, Books.PublisherDate
FROM Works
JOIN Works_Books ON Works.WorkID = Works_Books.WorkID
JOIN Books ON Works_Books.BookID = Books.BookID
WHERE MONTH(Books.PublisherDate) = 10;

--- ������� ������������ (� ������) �� ���������� �� ������ � ����� ---
SELECT Works.WorkName, Books.BookName, Books.PublisherDate
FROM Works
JOIN Works_Books ON Works.WorkID = Works_Books.WorkID
JOIN Books ON Works_Books.BookID = Books.BookID
ORDER BY Books.PublisherDate ASC;



---------------------------------------------------------------------------------------
/*���������������� ������� */

/*������� ��� ���������� �������� ������������ �� ���� ��������.
������� ��� �������� ���������� ����, ������ ������ �������������.
������� ��� ������ ����� �� ��������, ������ � �.�.
������� ��� ������ ������ ��������� �������� (��� ���� ������ ����� ����).
*/

CREATE FUNCTION CalculateAge (@DateOfBirth DATE)
RETURNS INT
AS
BEGIN
    DECLARE @CurrentDate DATE = GETDATE();
    RETURN DATEDIFF(YEAR, @DateOfBirth, @CurrentDate);
END;

SELECT dbo.CalculateAge('1990-05-15') AS Age;

------------------------------------------------------------------------------------

CREATE FUNCTION CountBooksCheckedOutByUser (@UserID INT)
RETURNS INT
AS
BEGIN
    DECLARE @Count INT;
    SELECT @Count = COUNT(*) 
    FROM Acts_Books ab
    JOIN Acts a ON ab.ActID = a.ActID
    WHERE a.SubscriptionID = @UserID AND a.ActionType = 'Issue';
    RETURN @Count;
END;

SELECT dbo.CountBooksCheckedOutByUser(1) AS TotalCheckedOutBooks;

-------------------------------------------------------------------------------


CREATE FUNCTION SearchBook (
    @BookName NVARCHAR(100), 
    @AuthorName NVARCHAR(100), 
    @GenreName NVARCHAR(50)
)
RETURNS TABLE
AS
RETURN (
    SELECT 
        b.BookID, 
        b.BookName, 
        a.LastName AS AuthorLastName, 
        a.FirstName AS AuthorFirstName, 
        g.GenreName,
        COUNT(*) AS Matching  -- ��������� ������� ��� �������� ����
    FROM 
        Books b
    JOIN 
        Works_Books wb ON b.BookID = wb.BookID
    JOIN 
        Works_Authors wa ON wb.WorkID = wa.WorkID
    JOIN 
        Authors a ON wa.AuthorID = a.AuthorID
    JOIN 
        Works_Genres wg ON b.BookID = wg.WorkID
    JOIN 
        Genres g ON wg.GenreID = g.GenreID
    WHERE 
        (ISNULL(@BookName, '') = '' OR b.BookName LIKE '%' + @BookName + '%') AND
        (ISNULL(@AuthorName, '') = '' OR (a.FirstName + ' ' + a.LastName) LIKE '%' + @AuthorName + '%') AND
        (ISNULL(@GenreName, '') = '' OR g.GenreName LIKE '%' + @GenreName + '%')
    GROUP BY 
        b.BookID, b.BookName, a.LastName, a.FirstName, g.GenreName  -- ���������� ����������
);

SELECT *
FROM dbo.SearchBook(' ', ' ', '�������');

SELECT *
FROM dbo.SearchBook('', '��������', ' ');

DROP FUNCTION IF EXISTS dbo.SearchBook;

--------------------------------------------------------------------------------------------------------------

CREATE FUNCTION MostActiveReader()
RETURNS TABLE
AS
RETURN (
    SELECT TOP 1 u.LastName AS ReaderLastName, u.FirstName AS ReaderFirstName,
           COUNT(ab.BookID) AS TotalBooksCheckedOut
    FROM Users u
    JOIN Subscriptions s ON u.UserID = s.SubscriptionID
    JOIN Acts a ON s.SubscriptionID = a.SubscriptionID
    JOIN Acts_Books ab ON a.ActID = ab.ActID
    WHERE a.ActionType = 'Issue'
    GROUP BY u.LastName, u.FirstName
    ORDER BY TotalBooksCheckedOut DESC
);

SELECT * FROM MostActiveReader();


---------------------------------------------------------------------------------------------------------------

/* ��������� */

---- ���������� ������ ������������--------

CREATE PROCEDURE AddUser
    @LastName NVARCHAR(50),
    @FirstName NVARCHAR(50),
    @MiddleName NVARCHAR(50),
    @UserPassword NVARCHAR(50)
AS
BEGIN
    INSERT INTO Users (LastName, FirstName, MiddleName, UserPassword)
    VALUES (@LastName, @FirstName, @MiddleName, @UserPassword);
END;


-----�������� ������������ �� ��������������----

CREATE PROCEDURE DeleteUser
    @UserID INT
AS
BEGIN
    DELETE FROM Users WHERE UserID = @UserID;
END;

-----���������� ������ ������������-----------

CREATE PROCEDURE UpdateUser
    @UserID INT,
    @LastName NVARCHAR(50),
    @FirstName NVARCHAR(50),
    @MiddleName NVARCHAR(50),
    @UserPassword NVARCHAR(50)
AS
BEGIN
    UPDATE Users
    SET LastName = @LastName,
        FirstName = @FirstName,
        MiddleName = @MiddleName,
        UserPassword = @UserPassword
    WHERE UserID = @UserID;
END;

-----��������� ������ ���� �������������------

CREATE PROCEDURE GetAllUsers
AS
BEGIN
    SELECT * FROM Users;
END;



--------����� ��������------------------


-- ���������� ������ ������������
EXEC AddUser '������', '�������', '�������������', 'wasd';

-- �������� ������������ � UserID = 3
EXEC DeleteUser 3;

-- ���������� ������ ������������ � UserID = 2
EXEC UpdateUser 2, '������', '����', '��������', 'newpassword1';

-- ��������� ������ ���� �������������
EXEC GetAllUsers;


/* �������� */

---------������� ��� ���������� ���� �� ����� ����� ���������� ��� �������� ----------

ALTER TABLE Shelves
ADD BookCount INT;

CREATE TRIGGER UpdateShelfBookCount
ON Books_Shelves
AFTER INSERT, DELETE
AS
BEGIN
    UPDATE Shelves
    SET BookCount = (
        SELECT COUNT(*)
        FROM Books_Shelves
        WHERE Books_Shelves.ShelfID = Shelves.ShelfID
    )
    FROM Shelves
    INNER JOIN Sections ON Shelves.SectionID = Sections.SectionID
    INNER JOIN LibraryRooms ON Sections.LibraryRoomID = LibraryRooms.LibraryRoomID
    WHERE LibraryRooms.LibraryRoomName = '��������� ���';
END;

---������� ����� � ��������� ���----
INSERT INTO Books_Shelves (Books_ShelvesID, BookID, ShelfID)
VALUES (16, 10, 2);

---  �������� �� ���������� --- 
SELECT * FROM Shelves WHERE ShelfID = 2;




-------- �������� �� ����� ���� ������ ��������, ���� �� �� ���� ���� �� ����  -----

DROP TRIGGER trg_CheckBorrowLimit;

CREATE TRIGGER trg_CheckBorrowLimit
ON Acts_Books
AFTER INSERT
AS
BEGIN
    DECLARE @ActID INT;
    SELECT @ActID = ActID FROM inserted;

    DECLARE @SubscriptionID INT;
    SELECT @SubscriptionID = SubscriptionID FROM Acts WHERE ActID = @ActID;

    IF (SELECT COUNT(*) FROM Acts WHERE SubscriptionID = @SubscriptionID AND ActionType = 'Issue' AND EventDate > (SELECT ISNULL(MAX(EventDate), '1900-01-01') FROM Acts WHERE ActionType = 'Return' AND SubscriptionID = @SubscriptionID)) > 2
    BEGIN
        THROW 51000, '�������� ��� ���� 2 ����� � �� ����� ����� ������, ���� �� ������ ���� �� ���� �� ���.', 1;
    END
END;


--- �������� ���� ������������ �����, �� �� ��� ���� 2 � �� ������ ---

INSERT INTO Acts (ActID, LibrarianID, SubscriptionID, ActionType, EventDate)
VALUES (11, 2, 6, 'Issue', '2023-10-25');

INSERT INTO Acts_Books (Acts_BooksID, BookID, ActID)
VALUES (12, 5, 11);



--------������ �� �������� ������������ ��� ���������� ������ � ����������------

CREATE TRIGGER CheckSubscriptionFields
ON Subscriptions
INSTEAD OF INSERT
AS
BEGIN
    IF EXISTS (
        SELECT 1
        FROM inserted
        WHERE (VisitorPhoneNumber IS NULL OR VisitorPhoneNumber = '')
           OR (SubscriptionsType NOT IN ('��������', '�������', '��������'))
           OR (SubscriptionPasport IS NULL OR SubscriptionPasport = '')
    )
    BEGIN
        THROW 51000, '������� ��� ���������� � ������ �������� ����� ����������� ������ ������������.', 1;
    END
    ELSE
    BEGIN
        INSERT INTO Subscriptions (SubscriptionID, LastName, FirstName, MiddleName, SubscriptionPasport, VisitorPhoneNumber, SubscriptionsType)
        SELECT SubscriptionID, LastName, FirstName, MiddleName, SubscriptionPasport, VisitorPhoneNumber, SubscriptionsType
        FROM inserted;
    END
END;


-- ������ ������� � �������������� ������� ����������
INSERT INTO Subscriptions (SubscriptionID, LastName, FirstName, MiddleName, SubscriptionPasport, VisitorPhoneNumber, SubscriptionsType)
VALUES (7, '������', '����', NULL, '', '+375290493566', '��������');











