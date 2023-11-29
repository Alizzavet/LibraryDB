using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Data.SqlClient;

namespace Library
{
    public class ComboBoxHelper
    {
        private static string connectionString = "Data Source=ALIZZAVET\\ELIZAVETA;Initial Catalog=Библиотека;Integrated Security=True";

        public void FillLibrariansComboBox(ComboBox comboBoxLibrarians)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Users.FirstName + ' ' + Users.LastName AS LibrarianName, Librarians.LibrarianID FROM Users INNER JOIN Librarians ON Users.UserID = Librarians.UserID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                comboBoxLibrarians.Items.Add(new KeyValuePair<int, string>((int)reader["LibrarianID"], reader["LibrarianName"].ToString()));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при заполнении списка библиотекарей: {ex.Message}");
            }
        }

        public void FillLibraryRoomsComboBox(ComboBox comboBoxLibraryRooms)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT LibraryRoomName, LibraryRoomID FROM LibraryRooms";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                comboBoxLibraryRooms.Items.Add(new KeyValuePair<int, string>((int)reader["LibraryRoomID"], reader["LibraryRoomName"].ToString()));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при заполнении списка залов: {ex.Message}");
            }
        }

        public void FillWorksComboBox(ComboBox comboBoxWorks)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT WorkID, WorkName FROM Works";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                comboBoxWorks.Items.Add(new KeyValuePair<int, string>((int)reader["WorkID"], reader["WorkName"].ToString()));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при заполнении списка произведений: {ex.Message}");
            }
        }

        public void FillBooksComboBox(ComboBox comboBoxBooks)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT BookID, BookName FROM Books";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                comboBoxBooks.Items.Add(new KeyValuePair<int, string>((int)reader["BookID"], reader["BookName"].ToString()));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при заполнении списка книг: {ex.Message}");
            }
        }

        public void FillGenresComboBox(ComboBox comboBoxGenres)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT GenreName, GenreID FROM Genres";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                comboBoxGenres.Items.Add(new KeyValuePair<int, string>((int)reader["GenreID"], reader["GenreName"].ToString()));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при заполнении списка жанров: {ex.Message}");
            }
        }

        public void FillAuthorsComboBox(ComboBox comboBoxAuthors)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT LastName + ' ' + FirstName + ' ' + MiddleName AS AuthorName, AuthorID FROM Authors";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                comboBoxAuthors.Items.Add(new KeyValuePair<int, string>((int)reader["AuthorID"], reader["AuthorName"].ToString()));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при заполнении списка авторов: {ex.Message}");
            }
        }

        public void FillPublishersComboBox(ComboBox comboBoxPublishers)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT PublisherName, PublisherID FROM Publishers";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                comboBoxPublishers.Items.Add(new KeyValuePair<int, string>((int)reader["PublisherID"], reader["PublisherName"].ToString()));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при заполнении списка издателей: {ex.Message}");
            }
        }

        public void FillActsComboBox(ComboBox comboBoxActs)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                SELECT 
                    Acts.ActID, 
                    Users.LastName + ' ' + Users.FirstName + ' ' + ISNULL(Users.MiddleName, '') AS LibrarianName,
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
                    Subscriptions ON Acts.SubscriptionID = Subscriptions.SubscriptionID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string actInfo = $"ActID: {reader["ActID"]}, Librarian: {reader["LibrarianName"]}, Reader: {reader["ReaderName"]}, ActionType: {reader["ActionType"]}, EventDate: {reader["EventDate"]}";
                                comboBoxActs.Items.Add(new KeyValuePair<int, string>((int)reader["ActID"], actInfo));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при заполнении списка актов: {ex.Message}");
            }
        }


        public void FillShelvesComboBox(ComboBox comboBoxShelves)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Shelves.ShelfID, Shelves.ShelfNumber, Sections.SectionNumber, LibraryRooms.LibraryRoomName " +
                                   "FROM Shelves " +
                                   "INNER JOIN Sections ON Shelves.SectionID = Sections.SectionID " +
                                   "INNER JOIN LibraryRooms ON Sections.LibraryRoomID = LibraryRooms.LibraryRoomID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string displayText = $"Номер полки: {reader["ShelfNumber"]}, Номер секции: {reader["SectionNumber"]} (Название зала: {reader["LibraryRoomName"]})";
                                comboBoxShelves.Items.Add(new KeyValuePair<int, string>((int)reader["ShelfID"], displayText));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при заполнении списка полок: {ex.Message}");
            }
        }

        public void FillSectionsComboBox(ComboBox comboBoxSections)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Sections.SectionID, Sections.SectionNumber, LibraryRooms.LibraryRoomName " +
                                   "FROM Sections " +
                                   "INNER JOIN LibraryRooms ON Sections.LibraryRoomID = LibraryRooms.LibraryRoomID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string displayText = $"Номер секции: {reader["SectionNumber"]} (Название зала: {reader["LibraryRoomName"]})";
                                comboBoxSections.Items.Add(new KeyValuePair<int, string>((int)reader["SectionID"], displayText));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при заполнении списка секций: {ex.Message}");
            }
        }


        public void FillSubscriptionsComboBox(ComboBox comboBoxSubscriptions)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT SubscriptionID, LastName + ' ' + FirstName + ' ' + ISNULL(MiddleName, '') AS ReaderName FROM Subscriptions";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                comboBoxSubscriptions.Items.Add(new KeyValuePair<int, string>((int)reader["SubscriptionID"], reader["ReaderName"].ToString()));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при заполнении списка подписок: {ex.Message}");
            }
        }


        public void FillAvailabilityComboBox(ComboBox comboBoxAvailability)
        {
            try
            {
                comboBoxAvailability.Items.Add(new KeyValuePair<int, string>(1, "Доступна"));
                comboBoxAvailability.Items.Add(new KeyValuePair<int, string>(0, "Списана"));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при заполнении списка доступности: {ex.Message}");
            }
        }

        public void FillBookInventoryComboBox(ComboBox comboBoxBookInventory)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT BooksInventorisation.BookInventoryID, Books.BookName, BooksInventorisation.CopyNumber " +
                                   "FROM BooksInventorisation " +
                                   "INNER JOIN Books ON BooksInventorisation.BookID = Books.BookID " +
                                   "WHERE BooksInventorisation.IsAvailable = 1";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string displayText = $"ID: {reader["BookInventoryID"]}, Книга: {reader["BookName"]}, Номер копии: {reader["CopyNumber"]}";
                                comboBoxBookInventory.Items.Add(new KeyValuePair<int, string>((int)reader["BookInventoryID"], displayText));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при заполнении списка инвентаризации книг: {ex.Message}");
            }
        }

        public void FillActionTypeComboBox(ComboBox comboBoxActionType)
        {
            try
            {
                comboBoxActionType.Items.Add(new KeyValuePair<int, string>(1, "Взял(а)"));
                comboBoxActionType.Items.Add(new KeyValuePair<int, string>(0, "Вернул(а)"));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при заполнении списка типов действий: {ex.Message}");
            }
        }
    }
}
