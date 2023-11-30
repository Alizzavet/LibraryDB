using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
    /// Логика взаимодействия для DemonstrationWindow.xaml
    /// </summary>
    public partial class DemonstrationWindow : Window
    {
        private static string connectionString = "Data Source=ALIZZAVET\\ELIZAVETA;Initial Catalog=Библиотека;Integrated Security=True";
        public DemonstrationWindow()
        {
            InitializeComponent();
            FillLibrariansComboBox(LibrarianComboBox);
        }

        private void ShowYearButton_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("GetBooksByYear", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Year", YearTextBox.Text);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                ResultDataGrid.ItemsSource = dataTable.DefaultView;
            }
        }

        private void ShowGenreButton_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("GetBooksByGenre", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Genre", GenreTextBox.Text);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                ResultDataGrid.ItemsSource = dataTable.DefaultView;
            }
        }

        private void ShowReadersButton_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("GetReadersByLibrarianAndPeriod", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@LibrarianID", ((KeyValuePair<int, string>)LibrarianComboBox.SelectedItem).Key);
                command.Parameters.AddWithValue("@StartDate", StartDatePicker.SelectedDate.Value);
                command.Parameters.AddWithValue("@EndDate", EndDatePicker.SelectedDate.Value);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                ResultDataGrid.ItemsSource = dataTable.DefaultView;
            }
        }

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

        private void ShowWorkloadButton_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("GetLibrarianWorkload", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@StartDate", FirstDatePicker.SelectedDate.Value);
                command.Parameters.AddWithValue("@EndDate", SecondDatePicker.SelectedDate.Value);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                ResultDataGrid.ItemsSource = dataTable.DefaultView;
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Введите год" || textBox.Text == "Введите жанр")
            {
                textBox.Text = string.Empty;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                if (textBox.Name == "YearTextBox")
                {
                    textBox.Text = "Введите год";
                }
                else if (textBox.Name == "GenreTextBox")
                {
                    textBox.Text = "Введите жанр";
                }
            }
        }


    }

}
