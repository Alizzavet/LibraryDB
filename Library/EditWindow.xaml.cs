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


    }
}