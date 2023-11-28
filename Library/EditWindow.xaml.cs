using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Library
{
    /// <summary>
    /// Логика взаимодействия для EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        private DataRow row;
        private Dictionary<string, Control> controls = new Dictionary<string, Control>();
        private string connectionString = "Data Source=ALIZZAVET\\ELIZAVETA;Initial Catalog=Библиотека;Integrated Security=True";

        public EditWindow(DataRow row, bool isNewRecord)
        {
            InitializeComponent();
            this.row = row;
            TextBox textBox;

            foreach (DataColumn column in row.Table.Columns)
            {
                if (column.DataType == typeof(DateTime))
                {
                    DatePicker datePicker = new DatePicker();
                    if (row[column] != DBNull.Value)
                    {
                        datePicker.SelectedDate = (DateTime)row[column];
                    }
                    stackPanel.Children.Insert(stackPanel.Children.Count - 2, datePicker);
                    controls.Add(column.ColumnName, datePicker);
                }
                else
                {
                    switch (column.ColumnName)
                    {
                        case "LibrarianID":
                            if (row.Table.TableName == "Librarians")
                            {
                                textBox = CreateTextBox(row, column);
                            }
                            else
                            {
                                ComboBox comboBoxLibrarians = CreateComboBox("comboBoxLibrarians", row, column);
                                FillLibrariansComboBox(comboBoxLibrarians);
                            }
                            break;
                        case "LibraryRoomID":
                            if (row.Table.TableName == "LibraryRooms")
                            {
                                textBox = CreateTextBox(row, column);
                            }
                            else
                            {
                                ComboBox comboBoxLibraryRooms = CreateComboBox("comboBoxLibraryRooms", row, column);
                                FillLibraryRoomsComboBox(comboBoxLibraryRooms);
                            }
                            break;
                        case "WorkID":
                            if (row.Table.TableName == "Works")
                            {
                                textBox = CreateTextBox(row, column);
                            }
                            else
                            {
                                ComboBox comboBoxWorks = CreateComboBox("comboBoxWorks", row, column);
                                FillWorksComboBox(comboBoxWorks);
                            }
                            break;
                        case "BookID":
                            if (row.Table.TableName == "Books")
                            {
                                textBox = CreateTextBox(row, column);
                            }
                            else
                            {
                                ComboBox comboBoxBooks = CreateComboBox("comboBoxBooks", row, column);
                                FillBooksComboBox(comboBoxBooks);
                            }
                            break;
                        case "AuthorID":
                            if (row.Table.TableName == "Authors")
                            {
                                textBox = CreateTextBox(row, column);
                            }
                            else
                            {
                                ComboBox comboBoxAuthors = CreateComboBox("comboBoxAuthors", row, column);
                                FillAuthorsComboBox(comboBoxAuthors);
                            }
                            break;
                        case "GenreID":
                            if (row.Table.TableName == "Genres")
                            {
                                textBox = CreateTextBox(row, column);
                            }
                            else
                            {
                                ComboBox comboBoxGenres = CreateComboBox("comboBoxGenres", row, column);
                                FillGenresComboBox(comboBoxGenres);
                            }
                            break;
                        case "PublisherID":
                            if (row.Table.TableName == "Publishers")
                            {
                                textBox = CreateTextBox(row, column);
                            }
                            else
                            {
                                ComboBox comboBoxPublishers = CreateComboBox("comboBoxPublishers", row, column);
                                FillPublishersComboBox(comboBoxPublishers);
                            }
                            break;
                        case "ActID":
                            if (row.Table.TableName == "Acts")
                            {
                                textBox = CreateTextBox(row, column);
                            }
                            else
                            {
                                ComboBox comboBoxActs = CreateComboBox("comboBoxActs", row, column);
                                FillActsComboBox(comboBoxActs);
                            }
                            break;
                        case "ShelfID":
                            if (row.Table.TableName == "Shelves")
                            {
                                textBox = CreateTextBox(row, column);
                            }
                            else
                            {
                                ComboBox comboBoxShelves = CreateComboBox("comboBoxShelves", row, column);
                                FillShelvesComboBox(comboBoxShelves);
                            }
                            break;
                        case "SectionID":
                            if (row.Table.TableName == "Sections")
                            {
                                textBox = CreateTextBox(row, column);
                            }
                            else
                            {
                                ComboBox comboBoxSections = CreateComboBox("comboBoxSections", row, column);
                                FillSectionsComboBox(comboBoxSections);
                            }
                            break;
                        case "SubscriptionID":
                            if (row.Table.TableName == "Subscriptions")
                            {
                                textBox = CreateTextBox(row, column);
                            }
                            else
                            {
                                ComboBox comboBoxSubscriptions = CreateComboBox("comboBoxSubscriptions", row, column);
                                FillSubscriptionsComboBox(comboBoxSubscriptions);
                            }
                            break;
                        case "IsAvailable":
                            
                                ComboBox comboBoxAvailability = CreateComboBox("comboBoxAvailability", row, column);
                                FillAvailabilityComboBox(comboBoxAvailability);
                            
                            break;
                        case "BookInventoryID":
                            if (row.Table.TableName == "BooksInventorisation")
                            {
                                textBox = CreateTextBox(row, column);
                            }
                            else
                            {
                                ComboBox comboBoxBookInventory = CreateComboBox("comboBoxBookInventory", row, column);
                                FillBookInventoryComboBox(comboBoxBookInventory);
                            }
                            break;
                        case "ActionType":
                            
                                ComboBox comboBoxActionType = CreateComboBox("comboBoxActionType", row, column);
                                FillActionTypeComboBox(comboBoxActionType);
                            
                            break;


                        default:
                            textBox = CreateTextBox(row, column);
                            break;
                    }
                }
            }
        }

        private ComboBox CreateComboBox(string name, DataRow row, DataColumn column)
        {
            ComboBox comboBox = new ComboBox
            {
                Name = name,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(10, 0, 0, 10),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 200
            };
            stackPanel.Children.Insert(stackPanel.Children.Count - 2, comboBox);
            controls.Add(column.ColumnName, comboBox);

            if (int.TryParse(row[column].ToString(), out int id))
            {
                comboBox.SelectedValue = id;
            }
            else
            {
                comboBox.SelectedValue = null;
            }

            return comboBox;
        }

        private TextBox CreateTextBox(DataRow row, DataColumn column)
        {
            TextBox textBox = new TextBox { Text = row[column].ToString() };
            textBox.ToolTip = column.ColumnName;
            if (column == row.Table.Columns[0])
            {
                textBox.IsEnabled = false;
            }
            stackPanel.Children.Insert(stackPanel.Children.Count - 2, textBox);
            controls.Add(column.ColumnName, textBox);

            return textBox;
        }


        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            foreach (DataColumn column in row.Table.Columns)
            {
                if (column.DataType == typeof(DateTime))
                {
                    DatePicker datePicker = (DatePicker)controls[column.ColumnName];
                    if (datePicker.SelectedDate.HasValue)
                    {
                        row[column] = datePicker.SelectedDate.Value;
                    }
                    else
                    {
                        row[column] = DBNull.Value;
                    }
                }
                else
                {
                    if (controls[column.ColumnName] is TextBox textBox)
                    {
                        if (string.IsNullOrWhiteSpace(textBox.Text))
                        {
                            MessageBox.Show($"Поле {column.ColumnName} не может быть пустым.");
                            return;
                        }
                        row[column] = textBox.Text;
                    }
                    else if (controls[column.ColumnName] is ComboBox comboBox)
                    {
                        if (comboBox.SelectedValue != null)
                        {
                            KeyValuePair<int, string> selectedPair = (KeyValuePair<int, string>)comboBox.SelectedValue;
                            row[column] = selectedPair.Key;
                        }
                        else
                        {
                            MessageBox.Show($"Поле {column.ColumnName} не может быть пустым.");
                            return;
                        }
                    }
                }
            }
            this.DialogResult = true;
        }


        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void FillLibrariansComboBox(ComboBox comboBoxLibrarians)
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

        private void FillLibraryRoomsComboBox(ComboBox comboBoxLibraryRooms)
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

        private void FillWorksComboBox(ComboBox comboBoxWorks)
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

        private void FillBooksComboBox(ComboBox comboBoxBooks)
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

        private void FillGenresComboBox(ComboBox comboBoxGenres)
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

        private void FillAuthorsComboBox(ComboBox comboBoxAuthors)
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

        private void FillPublishersComboBox(ComboBox comboBoxPublishers)
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

        private void FillActsComboBox(ComboBox comboBoxActs)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT ActID FROM Acts";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                comboBoxActs.Items.Add(new KeyValuePair<int, string>((int)reader["ActID"], reader["ActID"].ToString()));
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

        private void FillShelvesComboBox(ComboBox comboBoxShelves)
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

        private void FillSectionsComboBox(ComboBox comboBoxSections)
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


        private void FillSubscriptionsComboBox(ComboBox comboBoxSubscriptions)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT SubscriptionID, LastName + ' ' + FirstName + ' ' + MiddleName AS ReaderName FROM Subscriptions";
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

        private void FillAvailabilityComboBox(ComboBox comboBoxAvailability)
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

        private void FillBookInventoryComboBox(ComboBox comboBoxBookInventory)
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

        private void FillActionTypeComboBox(ComboBox comboBoxActionType)
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