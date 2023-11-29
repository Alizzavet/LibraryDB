/* СОЗДАНИЕ ТАБЛИЦ */ -----------------------------------------------------------------------------------------------


/* УДАЛЕНИЕ ТАБЛИЦ */ -----------------------------------------------------------------------------------------------
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
DROP TABLE BooksInventorisation;
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
    UserID INT, FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE
);

CREATE TABLE Administrators (
    AdministratorID INT PRIMARY KEY,
    UserID INT, FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE,
    LibrarianID INT, FOREIGN KEY (LibrarianID) REFERENCES Librarians(LibrarianID)
);

CREATE TABLE LibraryEvents (
	EventID INT PRIMARY KEY,
	EventType NVARCHAR(50) NOT NULL,
	EventName NVARCHAR(50) NOT NULL,
	EventDate DATE NOT NULL,
	LibrarianID INT, FOREIGN KEY (LibrarianID) REFERENCES Librarians(LibrarianID) ON DELETE CASCADE
);

--------------------------------------------

drop procedure EditLibraryEvents;
CREATE PROCEDURE DeleteFromLibraryEvents
    @EventID INT
AS
BEGIN
    -- Удаляем запись из таблицы LibraryEvents
    DELETE FROM LibraryEvents WHERE EventID = @EventID;
END
----------------------------------------------

CREATE TABLE Subscriptions (
	SubscriptionID INT PRIMARY KEY,
	LastName NVARCHAR(50) NOT NULL,
    FirstName NVARCHAR(50) NOT NULL,
    MiddleName NVARCHAR(50),
    SubscriptionPasport NVARCHAR(50),
	VisitorPhoneNumber VARCHAR(20),
	SubscriptionsType VARCHAR(20)
);

----------------------------
CREATE PROCEDURE DeleteFromSubscriptions
    @SubscriptionID INT
AS
BEGIN
    -- Удаляем запись из таблицы Subscriptions
    DELETE FROM Subscriptions WHERE SubscriptionID = @SubscriptionID;
END
----------------------------------------

CREATE TABLE LibraryRooms (
	LibraryRoomID INT PRIMARY KEY,
	LibraryRoomName NVARCHAR(50)
);

-------------------------
CREATE PROCEDURE DeleteFromLibraryRooms
    @LibraryRoomID INT
AS
BEGIN
    -- Удаляем все связанные записи из таблицы Sections
    DELETE FROM Sections WHERE LibraryRoomID = @LibraryRoomID;

    -- Теперь мы можем безопасно удалить запись из таблицы LibraryRooms
    DELETE FROM LibraryRooms WHERE LibraryRoomID = @LibraryRoomID;
END


-------------------------------------------

CREATE TABLE Sections (
	SectionID INT PRIMARY KEY,
	SectionNumber INT,
	LibraryRoomID INT, FOREIGN KEY (LibraryRoomID) REFERENCES LibraryRooms(LibraryRoomID) ON DELETE CASCADE
);


-----------------------------------------------
CREATE PROCEDURE GetSectionsData
AS
BEGIN
    SELECT 
        Sections.SectionID, 
        Sections.SectionNumber,
        Sections.LibraryRoomID, 
        LibraryRooms.LibraryRoomName 
    FROM 
        Sections 
    INNER JOIN 
        LibraryRooms ON Sections.LibraryRoomID = LibraryRooms.LibraryRoomID;
END


CREATE PROCEDURE DeleteFromSections
    @SectionID INT
AS
BEGIN
    -- Удаляем все связанные записи из таблицы Shelves
    DELETE FROM Shelves WHERE SectionID = @SectionID;

    -- Теперь мы можем безопасно удалить запись из таблицы Sections
    DELETE FROM Sections WHERE SectionID = @SectionID;
END


---------------------------------------------

CREATE TABLE Shelves (
	ShelfID INT PRIMARY KEY,
	ShelfNumber INT,
	SectionID INT, FOREIGN KEY (SectionID) REFERENCES Sections(SectionID) ON DELETE CASCADE
);


-------------------------------------------------
CREATE PROCEDURE GetShelvesData
AS
BEGIN
    SELECT 
        Shelves.ShelfID, 
        Shelves.ShelfNumber,
        Sections.SectionID, 
        Sections.SectionNumber,
        Sections.LibraryRoomID, 
        LibraryRooms.LibraryRoomName 
    FROM 
        Shelves 
    INNER JOIN 
        Sections ON Shelves.SectionID = Sections.SectionID 
    INNER JOIN 
        LibraryRooms ON Sections.LibraryRoomID = LibraryRooms.LibraryRoomID;
END


CREATE PROCEDURE DeleteFromShelves
    @ShelfID INT
AS
BEGIN
    -- Здесь вы можете добавить код для удаления связанных записей из других таблиц, если они есть

    -- Удаляем запись из таблицы Shelves
    DELETE FROM Shelves WHERE ShelfID = @ShelfID;
END


-------------------------------------------------

CREATE TABLE Works (
	WorkID INT PRIMARY KEY,
	WorkName NVARCHAR(100)
);

-----------------------------------
CREATE PROCEDURE DeleteFromWorks
    @WorkID INT
AS
BEGIN
    -- Удаляем все связанные записи из таблицы Works_Books
    DELETE FROM Works_Books WHERE WorkID = @WorkID;

    -- Теперь мы можем безопасно удалить запись из таблицы Works
    DELETE FROM Works WHERE WorkID = @WorkID;
END

CREATE PROCEDURE GetWorksData
AS
BEGIN
    SELECT 
        Works.WorkID, 
        Works.WorkName
    FROM 
        Works;
END
------------------------------------

CREATE TABLE Books (
	BookID INT PRIMARY KEY,
	BookName NVARCHAR(100),
	PublisherDate DATE NOT NULL
);

----------------------------------
CREATE PROCEDURE DeleteFromBooks
    @BookID INT
AS
BEGIN
    -- Удаляем все связанные записи из таблицы Works_Books
    DELETE FROM Works_Books WHERE BookID = @BookID;

    -- Теперь мы можем безопасно удалить запись из таблицы Books
    DELETE FROM Books WHERE BookID = @BookID;
END

CREATE PROCEDURE GetBooksData
AS
BEGIN
    SELECT 
        Books.BookID, 
        Books.BookName,
        Books.PublisherDate
    FROM 
        Books;
END
------------------------------------

CREATE TABLE Works_Books (
	Works_BooksID INT PRIMARY KEY,
	WorkID INT, FOREIGN KEY (WorkID) REFERENCES Works(WorkID) ON DELETE CASCADE,
	BookID INT, FOREIGN KEY (BookID) REFERENCES Books(BookID) ON DELETE CASCADE
);

-------------------------------------
CREATE PROCEDURE DeleteFromWorks_Books
    @Works_BooksID INT
AS
BEGIN
    -- Удаляем запись из таблицы Works_Books
    DELETE FROM Works_Books WHERE Works_BooksID = @Works_BooksID;
END

CREATE PROCEDURE GetWorks_BooksData
AS
BEGIN
    SELECT 
        Works_Books.Works_BooksID, 
        Works_Books.WorkID, 
        Works.WorkName,
        Works_Books.BookID,
        Books.BookName
    FROM 
        Works_Books 
    INNER JOIN 
        Works ON Works_Books.WorkID = Works.WorkID
    INNER JOIN
        Books ON Works_Books.BookID = Books.BookID;
END
-----------------------------------

CREATE TABLE Books_Shelves (
	Books_ShelvesID INT PRIMARY KEY,
	BookInventoryID INT, FOREIGN KEY (BookInventoryID) REFERENCES BooksInventorisation(BookInventoryID),
	ShelfID INT, FOREIGN KEY (ShelfID) REFERENCES Shelves(ShelfID)
);

----------------------------
CREATE PROCEDURE DeleteFromBooks_Shelves
    @Books_ShelvesID INT
AS
BEGIN
    -- Удаляем запись из таблицы Books_Shelves
    DELETE FROM Books_Shelves WHERE Books_ShelvesID = @Books_ShelvesID;
END

CREATE PROCEDURE GetBooks_ShelvesData
AS
BEGIN
    SELECT 
        Books_Shelves.Books_ShelvesID, 
        Books_Shelves.BookInventoryID, 
        Books.BookName,
        Shelves.ShelfID,
        Shelves.ShelfNumber,
        Sections.SectionNumber,
        LibraryRooms.LibraryRoomName
    FROM 
        Books_Shelves 
    INNER JOIN 
        BooksInventorisation ON Books_Shelves.BookInventoryID = BooksInventorisation.BookInventoryID
    INNER JOIN
        Books ON BooksInventorisation.BookID = Books.BookID
    INNER JOIN
        Shelves ON Books_Shelves.ShelfID = Shelves.ShelfID
    INNER JOIN
        Sections ON Shelves.SectionID = Sections.SectionID
    INNER JOIN
        LibraryRooms ON Sections.LibraryRoomID = LibraryRooms.LibraryRoomID;
END

-----------------------------------

CREATE TABLE Authors (
	AuthorID INT PRIMARY KEY,
	LastName NVARCHAR(50) NOT NULL,
    FirstName NVARCHAR(50) NOT NULL,
    MiddleName NVARCHAR(50),
	Pseudonym NVARCHAR(50)
);

------------------------------------
CREATE PROCEDURE DeleteFromAuthors
    @AuthorID INT
AS
BEGIN
    -- Удаляем все связанные записи из таблицы Works_Authors
    DELETE FROM Works_Authors WHERE AuthorID = @AuthorID;

    -- Теперь мы можем безопасно удалить запись из таблицы Authors
    DELETE FROM Authors WHERE AuthorID = @AuthorID;
END
----------------------------------------

CREATE TABLE Works_Authors (
	Works_AuthorsID INT PRIMARY KEY,
	WorkID INT, FOREIGN KEY (WorkID) REFERENCES Works(WorkID) ON DELETE CASCADE,
	AuthorID INT, FOREIGN KEY (AuthorID) REFERENCES Authors(AuthorID) ON DELETE CASCADE,
);

------------------------------------
CREATE PROCEDURE DeleteFromWorks_Authors
    @Works_AuthorsID INT
AS
BEGIN
    -- Удаляем запись из таблицы Works_Authors
    DELETE FROM Works_Authors WHERE Works_AuthorsID = @Works_AuthorsID;
END

CREATE PROCEDURE GetWorks_AuthorsData
AS
BEGIN
    SELECT 
        Works_Authors.Works_AuthorsID, 
        Works_Authors.WorkID, 
        Works.WorkName,
        Works_Authors.AuthorID,
        Authors.LastName,
        Authors.FirstName,
        Authors.MiddleName,
        Authors.Pseudonym
    FROM 
        Works_Authors 
    INNER JOIN 
        Works ON Works_Authors.WorkID = Works.WorkID
    INNER JOIN
        Authors ON Works_Authors.AuthorID = Authors.AuthorID;
END
---------------------------------------

CREATE TABLE Genres (
	GenreID INT PRIMARY KEY,
	GenreName NVARCHAR(50) NOT NULL
);

----------------------------
CREATE PROCEDURE DeleteFromGenres
    @GenreID INT
AS
BEGIN
    -- Удаляем все связанные записи из таблицы Works_Genres
    DELETE FROM Works_Genres WHERE GenreID = @GenreID;

    -- Теперь мы можем безопасно удалить запись из таблицы Genres
    DELETE FROM Genres WHERE GenreID = @GenreID;
END
----------------------

CREATE TABLE Works_Genres (
	Works_GenresID INT PRIMARY KEY,
	WorkID INT, FOREIGN KEY (WorkID) REFERENCES Works(WorkID) ON DELETE CASCADE,
	GenreID INT, FOREIGN KEY (GenreID) REFERENCES Genres(GenreID) ON DELETE CASCADE
);

----------------------------------
CREATE PROCEDURE DeleteFromWorks_Genres
    @Works_GenresID INT
AS
BEGIN
    -- Удаляем запись из таблицы Works_Genres
    DELETE FROM Works_Genres WHERE Works_GenresID = @Works_GenresID;
END

CREATE PROCEDURE GetWorks_GenresData
AS
BEGIN
    SELECT 
        Works_Genres.Works_GenresID, 
        Works_Genres.WorkID, 
        Works.WorkName,
        Works_Genres.GenreID,
        Genres.GenreName
    FROM 
        Works_Genres 
    INNER JOIN 
        Works ON Works_Genres.WorkID = Works.WorkID
    INNER JOIN
        Genres ON Works_Genres.GenreID = Genres.GenreID;
END
-----------------------------------

CREATE TABLE Publishers (
	PublisherID INT PRIMARY KEY,
	PublisherName NVARCHAR(50) NOT NULL
);

----------------------------------------
CREATE PROCEDURE DeleteFromPublishers
    @PublisherID INT
AS
BEGIN
    -- Удаляем запись из таблицы Publishers
    DELETE FROM Publishers WHERE PublisherID = @PublisherID;
END
-----------------------------------------

CREATE TABLE Books_Publishers (
	Books_PublishersID INT PRIMARY KEY,
	BookID INT, FOREIGN KEY (BookID) REFERENCES Books(BookID) ON DELETE CASCADE,
	PublisherID INT, FOREIGN KEY (PublisherID) REFERENCES Publishers(PublisherID) ON DELETE CASCADE
);

-------------------------------
CREATE PROCEDURE DeleteFromBooks_Publishers
    @Books_PublishersID INT
AS
BEGIN
    -- Удаляем запись из таблицы Books_Publishers
    DELETE FROM Books_Publishers WHERE Books_PublishersID = @Books_PublishersID;
END

CREATE PROCEDURE GetBooks_PublishersData
AS
BEGIN
    SELECT 
        Books_Publishers.Books_PublishersID, 
        Books_Publishers.BookID, 
        Books.BookName,
        Books_Publishers.PublisherID,
        Publishers.PublisherName
    FROM 
        Books_Publishers 
    INNER JOIN 
        Books ON Books_Publishers.BookID = Books.BookID
    INNER JOIN
        Publishers ON Books_Publishers.PublisherID = Publishers.PublisherID;
END
-------------------------------------

CREATE TABLE Acts (
	ActID INT PRIMARY KEY,
	LibrarianID INT, FOREIGN KEY (LibrarianID) REFERENCES Librarians(LibrarianID) ON DELETE CASCADE,
	SubscriptionID INT, FOREIGN KEY (SubscriptionID) REFERENCES Subscriptions(SubscriptionID) ON DELETE CASCADE,
	ActionType NVARCHAR(20) NOT NULL,
	EventDate DATE NOT NULL
);

----------------------------------
CREATE PROCEDURE DeleteFromActs
    @ActID INT
AS
BEGIN
    -- Удаляем все связанные записи из таблицы Acts_Books
    DELETE FROM Acts_Books WHERE ActID = @ActID;

    -- Теперь мы можем безопасно удалить запись из таблицы Acts
    DELETE FROM Acts WHERE ActID = @ActID;
END

CREATE PROCEDURE GetActsData
AS
BEGIN
    SELECT 
        Acts.ActID, 
        Acts.LibrarianID,
        Users.LastName + ' ' + Users.FirstName + ' ' + ISNULL(Users.MiddleName, '') AS LibrarianName,
        Acts.SubscriptionID,
        Subscriptions.LastName + ' ' + Subscriptions.FirstName + ' ' + ISNULL(Subscriptions.MiddleName, '') AS ReaderName,
        Acts.ActionType,
        Acts.EventDate
    FROM 
        Acts 
    INNER JOIN 
        Librarians ON Acts.LibrarianID = Librarians.LibrarianID
    INNER JOIN
        Users ON Librarians.UserID = Users.UserID
    INNER JOIN
        Subscriptions ON Acts.SubscriptionID = Subscriptions.SubscriptionID;
END



------------------------------
CREATE TABLE BooksInventorisation (
	BookInventoryID INT PRIMARY KEY,
	BookID INT, FOREIGN KEY (BookID) REFERENCES Books(BookID) ON DELETE CASCADE,
	IsAvailable BIT NOT NULL,
	CopyNumber INT NOT NULL
);

-----------------------------
CREATE PROCEDURE DeleteFromBooksInventorisation
    @BookInventoryID INT
AS
BEGIN
    -- Удаляем запись из таблицы BookInventory
    DELETE FROM BooksInventorisation WHERE BookInventoryID = @BookInventoryID;
END

CREATE PROCEDURE GetBooksInventorisationData
AS
BEGIN
    SELECT 
        BooksInventorisation.BookInventoryID, 
        BooksInventorisation.BookID, 
        Books.BookName,
        BooksInventorisation.CopyNumber,
        CASE 
            WHEN BooksInventorisation.IsAvailable = 1 THEN 'В наличии'
            ELSE 'Списана'
        END AS Availability
    FROM 
        BooksInventorisation 
    INNER JOIN 
        Books ON BooksInventorisation.BookID = Books.BookID;
END
----------------------------------------

CREATE TABLE Acts_Books (
	Acts_BooksID INT PRIMARY KEY,
	BookInventoryID INT, FOREIGN KEY (BookInventoryID) REFERENCES BooksInventorisation(BookInventoryID) ON DELETE CASCADE,
	ActID INT, FOREIGN KEY (ActID) REFERENCES Acts(ActID) ON DELETE CASCADE
);

--------------------------------
CREATE PROCEDURE DeleteFromActs_Books
    @Acts_BooksID INT
AS
BEGIN
    -- Удаляем запись из таблицы Acts_Books
    DELETE FROM Acts_Books WHERE Acts_BooksID = @Acts_BooksID;
END

CREATE PROCEDURE GetActs_BooksData
AS
BEGIN
    SELECT 
        Acts_Books.Acts_BooksID, 
        Acts_Books.BookInventoryID, 
        Books.BookName,
        Acts_Books.ActID,
        Acts.ActionType,
        BooksInventorisation.CopyNumber,
        CASE 
            WHEN BooksInventorisation.IsAvailable = 1 THEN 'В наличии'
            ELSE 'Списана'
        END AS Availability
    FROM 
        Acts_Books 
    INNER JOIN 
        BooksInventorisation ON Acts_Books.BookInventoryID = BooksInventorisation.BookInventoryID
    INNER JOIN
        Books ON BooksInventorisation.BookID = Books.BookID
    INNER JOIN
        Acts ON Acts_Books.ActID = Acts.ActID;
END

------------------------------------

/* ЗАПОЛНЕНИЕ ТАБЛИЦ */ -----------------------------------------------------------------------------------------------

INSERT INTO Users (UserID, LastName, FirstName, MiddleName, UserPassword, UserLogin)
VALUES 
  (1, 'Иванов', 'Антон', NULL, '35203003', 'IvanovAdmin'),
  (2, 'Петров', 'Петр', NULL, 'securepass', 'PetrovLibra'),
  (3, 'Сидорова', 'Елена', 'Александровна', '123456', 'SidorovaLibra'),
  (4, 'Филаненко', 'Александр', 'Евгеньевич', 'l6w2ynvpebof_hIq54fg', 'FilanenkoAdmin'),
  (5, 'Степулёнок', 'Елена', 'Владимировна', 'leomsupremacy', 'StepulionokAdmin');

-- Пример заполнения таблицы Librarians
INSERT INTO Librarians (LibrarianID, UserID)
VALUES 
  (1, 1),
  (2, 2),
  (3, 3),
  (4, 5);

-- Пример заполнения таблицы Administrators
INSERT INTO Administrators (AdministratorID, UserID, LibrarianID)
VALUES 
  (1, 1, 1),
  (2, 4, NULL),
  (3, 5, 4);

-- Пример заполнения таблицы LibraryEvents
INSERT INTO LibraryEvents (EventID, EventType, EventName, EventDate, LibrarianID)
VALUES 
  (1, 'Meeting', 'Библиотечное собрание', '2023-09-28', 1),
  (2, 'Presentation', 'Презентация новых поступлений жанра детектив', '2023-10-05', 2),
  (3, 'Presentation', 'Дебют юного автора', '2023-10-12', 3),
  (4, 'Presentation', 'Презентация выставки раритетных книг', '2023-11-01', 2),
  (5, 'Quest', 'Алые паруса', '2023-12-25', 1),
  (6, 'Meeting', 'Посещение школы № 3', '2024-01-13', 3);

-- Пример заполнения таблицы Subscriptions
INSERT INTO Subscriptions (SubscriptionID, LastName, FirstName, MiddleName, SubscriptionPasport, VisitorPhoneNumber, SubscriptionsType)
VALUES 
  (1, 'Кузнецов', 'Дмитрий', 'Андреевич', 'AB132123456', '+375290493566', 'Взрослый'),
  (2, 'Смирнова', 'Мария', NULL, 'CD786749012', NULL, 'Детский'),
  (3, 'Юрчишко', 'Елизавета', 'Игоревна', 'CD789422812', '+375333351995', 'Детский'),
  (4, 'Афонасьева', 'Татьяна', 'Эдуадовна', 'CD786239012', '+375333361732', 'Взрослый'),
  (5, 'Боталенко', 'Артем', 'Станиславович', 'CD786749012', '+375443356732', 'Льготный'),
  (6, 'Абрамова', 'Мария', 'Евгеньевна', 'AB7823449012', NULL, 'Детский');

-- Пример заполнения таблицы LibraryRooms
INSERT INTO LibraryRooms (LibraryRoomID, LibraryRoomName)
VALUES 
  (1, 'Читальный зал'),
  (2, 'Зал для выбора на дом');


-- Пример заполнения таблицы Works
INSERT INTO Works (WorkID, WorkName)
VALUES 
  (1, 'Цикл книг "Война и мир"'),
  (2, 'Поэма "Демон"'),
  (3, 'Повесть "Собачье сердце"'),
  (4, 'Роман "Мастер и Маргарита"'),
  (5, 'Повесть "Повелитель Александр с горы"'),
  (6, 'Цикл книг "Коты-воители"'),
  (7, 'Цикл книг "Властелин Колец"'),
  (8, 'Стихотворение "Тайная любовь"'),
  (9, 'Стихотворение "Белая береза"');

-- Пример заполнения таблицы Books
INSERT INTO Books (BookID, BookName, PublisherDate)
VALUES 
  (1, 'Война и мир. Том 1', '2010-05-15'),
  (2, 'Война и мир. Том 2', '2010-06-20'),
  (3, 'Демон', '1985-02-15'),
  (4, 'Коты-воители. Стань диким. Том 1', '2010-10-25'),
  (5, 'Коты-воители. Огонь и лед. Том 2', '2011-02-25'),
  (6, 'Творчество Булгакова', '2015-06-07'),
  (7, 'Знаменитые стихи', '2018-03-30'),
  (8, 'Властелин колец. Том 1', '2016-06-03'),
  (9, 'Властелин колец. Том 2', '2016-10-10'),
  (10, 'Мастер и Маргарита', '2001-10-25'),
  (11, 'Повелитель Александр с горы', '2023-09-24');

-- Пример заполнения таблицы Works_Books
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

-- Пример заполнения таблицы Authors
INSERT INTO Authors (AuthorID, LastName, FirstName, MiddleName, Pseudonym)
VALUES 
  (1, 'Толстой', 'Лев', 'Николаевич', NULL),
  (2, 'Лермонтов', 'Михаил', 'Юрьевич', NULL),
  (3, 'Булгаков', 'Михаил', 'Афанасьевич', NULL),
  (4, 'Хантер', 'Эрин', NULL, NULL),
  (5, 'Толкин', 'Джон Рональд Руэл', NULL, NULL),
  (6, 'Клеменс', 'Брентано', NULL, NULL),
  (7, 'Есенин', 'Сергей', 'Александрович', NULL),
  (8, 'Эллаиза', 'Кокстон', NULL, 'Аврора Найт');

-- Пример заполнения таблицы Works_Authors
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

-- Пример заполнения таблицы Genres
INSERT INTO Genres (GenreID, GenreName)
VALUES 
  (1, 'Романтика'),
  (2, 'Фэнтези'),
  (3, 'Мелодрама'),
  (4, 'Классика'),
  (5, 'Лирика'),
  (6, 'Сатира'),
  (7, 'Ужасы'),
  (8, 'Любовный роман'),
  (9, 'Фарс'),
  (10, 'Приключения'),
  (11, 'Драма');

-- Пример заполнения таблицы Works_Genres
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

-- Пример заполнения таблицы Publishers
INSERT INTO Publishers (PublisherID, PublisherName)
VALUES 
  (1, 'Издательство "Азбука"'),
  (2, 'Издательство "Эксмо"'),
  (3, 'Издательство "Олма-Медиа групп"'),
  (4, 'Издательство "АСТ"');

-- Пример заполнения таблицы Books_Publishers
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
  /* ЗАПРОСЫ */

  --- Выбрать всех авторов, у которых 2 и более произведения ----
  SELECT Authors.FirstName, Authors.LastName, COUNT(Works_Authors.WorkID) AS NumberOfWorks
FROM Authors
JOIN Works_Authors ON Authors.AuthorID = Works_Authors.AuthorID
GROUP BY Authors.FirstName, Authors.LastName
HAVING COUNT(Works_Authors.WorkID) >= 2;

 --- Выбрать всех владельцев абонементов, которые еще не успели вернуть по актам библиотеки те книги, которые взяли ---
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

 ---Все книги жанра фэнтези---
SELECT Books.BookName, Genres.GenreName
FROM Books
JOIN Works_Books ON Books.BookID = Works_Books.BookID
JOIN Works_Genres ON Works_Books.WorkID = Works_Genres.WorkID
JOIN Genres ON Works_Genres.GenreID = Genres.GenreID
WHERE Genres.GenreName = 'Фэнтези';

 ---Выбрать администраторов, которые назначены также библиотекарями и вывести их фио---
 SELECT Users.FirstName, Users.LastName, Users.MiddleName
FROM Users
JOIN Administrators ON Users.UserID = Administrators.UserID
WHERE Administrators.LibrarianID IS NOT NULL;

---Все произведения, опубликованные в октябре---
SELECT Works.WorkName, Books.BookName, Books.PublisherDate
FROM Works
JOIN Works_Books ON Works.WorkID = Works_Books.WorkID
JOIN Books ON Works_Books.BookID = Books.BookID
WHERE MONTH(Books.PublisherDate) = 10;

--- Выборка произведений (в книгах) по сортировке от старых к новым ---
SELECT Works.WorkName, Books.BookName, Books.PublisherDate
FROM Works
JOIN Works_Books ON Works.WorkID = Works_Books.WorkID
JOIN Books ON Works_Books.BookID = Books.BookID
ORDER BY Books.PublisherDate ASC;



---------------------------------------------------------------------------------------
/*ПОЛЬЗОВАТЕЛЬСКИЕ ФУНКЦИИ */

/*Функция для вычисления возраста пользователя по дате рождения.
Функция для подсчета количества книг, взятых каждым пользователем.
Функция для поиска книги по названию, автору и т.д.
Функция для вывода самого активного читателя (кто взял больше всего книг).
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
        COUNT(*) AS Matching  -- Добавлено условие для подсчёта книг
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
        b.BookID, b.BookName, a.LastName, a.FirstName, g.GenreName  -- Группируем результаты
);

SELECT *
FROM dbo.SearchBook(' ', ' ', 'Фэнтези');

SELECT *
FROM dbo.SearchBook('', 'Булгаков', ' ');

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

/* ПРОЦЕДУРЫ */


---- Добавление нового пользователя--------

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


-----Удаление пользователя по идентификатору----

CREATE PROCEDURE DeleteUser
    @UserID INT
AS
BEGIN
    DELETE FROM Users WHERE UserID = @UserID;
END;

-----Обновление данных пользователя-----------

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

-----Получение списка всех пользователей------

CREATE PROCEDURE GetAllUsers
AS
BEGIN
    SELECT * FROM Users;
END;



--------ВЫЗОВ ПРОЦЕДУР------------------


-- Добавление нового пользователя
EXEC AddUser 'Алешин', 'Валерий', 'Александрович', 'wasd';

-- Удаление пользователя с UserID = 3
EXEC DeleteUser 3;

-- Обновление данных пользователя с UserID = 2
EXEC UpdateUser 2, 'Иванов', 'Петр', 'Иванович', 'newpassword1';

-- Получение списка всех пользователей
EXEC GetAllUsers;


/* ТРИГГЕРЫ */

---------Триггер для количества книг на полке после добавления или удаления ----------

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
    WHERE LibraryRooms.LibraryRoomName = 'Читальный зал';
END;

---Добавим книгу в читальный зал----
INSERT INTO Books_Shelves (Books_ShelvesID, BookID, ShelfID)
VALUES (16, 10, 2);

---  Проверим их количество --- 
SELECT * FROM Shelves WHERE ShelfID = 2;




-------- Проверка на лимит книг выдаче читателю, если он не сдал хотя бы одну  -----

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
        THROW 51000, 'Читатель уже взял 2 книги и не может взять больше, пока не вернет хотя бы одну из них.', 1;
    END
END;


--- Пытаемся дать пользователю книгу, но он уже взял 2 и не вернул ---

INSERT INTO Acts (ActID, LibrarianID, SubscriptionID, ActionType, EventDate)
VALUES (11, 2, 6, 'Issue', '2023-10-25');

INSERT INTO Acts_Books (Acts_BooksID, BookID, ActID)
VALUES (12, 5, 11);



--------Запрет на создание пользователя без паспортных данных и абонемента------

CREATE TRIGGER CheckSubscriptionFields
ON Subscriptions
INSTEAD OF INSERT
AS
BEGIN
    IF EXISTS (
        SELECT 1
        FROM inserted
        WHERE (VisitorPhoneNumber IS NULL OR VisitorPhoneNumber = '')
           OR (SubscriptionsType NOT IN ('Взрослый', 'Детский', 'Льготный'))
           OR (SubscriptionPasport IS NULL OR SubscriptionPasport = '')
    )
    BEGIN
        THROW 51000, 'Введите тип абонемента и данные паспорта перед добавлением нового пользователя.', 1;
    END
    ELSE
    BEGIN
        INSERT INTO Subscriptions (SubscriptionID, LastName, FirstName, MiddleName, SubscriptionPasport, VisitorPhoneNumber, SubscriptionsType)
        SELECT SubscriptionID, LastName, FirstName, MiddleName, SubscriptionPasport, VisitorPhoneNumber, SubscriptionsType
        FROM inserted;
    END
END;


-- Пример вставки с недостаточными данными абонемента
INSERT INTO Subscriptions (SubscriptionID, LastName, FirstName, MiddleName, SubscriptionPasport, VisitorPhoneNumber, SubscriptionsType)
VALUES (7, 'Иванов', 'Петр', NULL, '', '+375290493566', 'Взрослый');












