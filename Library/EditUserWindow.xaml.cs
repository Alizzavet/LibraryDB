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
    /// Логика взаимодействия для EditUserWindow.xaml
    /// </summary>
    public partial class EditUserWindow : Window
    {
        private string connectionString = "Data Source=ALIZZAVET\\ELIZAVETA;Initial Catalog=Библиотека;Integrated Security=True";
        public EditUserWindow()
        {
            InitializeComponent();
        }

        private void ViewUsersButton_Click(object sender, RoutedEventArgs e)
        {
            string sql = "EXEC GetUserRoles";
            SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, connectionString);
            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            dataGrid.ItemsSource = dataTable.DefaultView;
            dataGrid.Columns[0].Visibility = Visibility.Collapsed;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            EditUserDataWindow window = new EditUserDataWindow();
            window.ShowDialog();

            if (window.DialogResult.HasValue && window.DialogResult.Value)
            {
                // Получите данные из окна и добавьте пользователя
                string lastName = window.txtLastName.Text;
                string firstName = window.txtFirstName.Text;
                string middleName = window.txtMiddleName.Text;
                string userLogin = window.txtUserLogin.Text;
                string userPassword = window.txtUserPassword.Text;
                string role = window.rbnAdministrator.IsChecked.Value ? "Администратор" : "Библиотекарь";

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand("AddUser", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            command.Parameters.Add(new SqlParameter("@LastName", lastName));
                            command.Parameters.Add(new SqlParameter("@FirstName", firstName));
                            command.Parameters.Add(new SqlParameter("@MiddleName", middleName));
                            command.Parameters.Add(new SqlParameter("@UserLogin", userLogin));
                            command.Parameters.Add(new SqlParameter("@UserPassword", userPassword));
                            command.Parameters.Add(new SqlParameter("@Role", role));

                            connection.Open();
                            command.ExecuteNonQuery();
                        }
                    }

                    // Обновите DataGrid
                    ViewUsersButton_Click(null, null);
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 50000)
                    {
                        MessageBox.Show("Пожалуйста, предоставьте значения для LastName, FirstName, UserLogin и UserPassword.");
                    }
                    else
                    {
                        throw;
                    }
                }

            }

        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem is DataRowView row)
            {
                EditUserDataWindow window = new EditUserDataWindow();

                window.txtLastName.Text = row["LastName"].ToString();
                window.txtFirstName.Text = row["FirstName"].ToString();
                window.txtMiddleName.Text = row["MiddleName"].ToString();
                window.txtUserLogin.Text = row["UserLogin"].ToString();
                window.txtUserPassword.Text = row["UserPassword"].ToString();
                if (row["Role"].ToString() == "Администратор")
                {
                    window.rbnAdministrator.IsChecked = true;
                }
                else
                {
                    window.rbnLibrarian.IsChecked = true;
                }

                window.ShowDialog();

                if (window.DialogResult.HasValue && window.DialogResult.Value)
                {
                    // Получите данные из окна и обновите пользователя
                    string lastName = window.txtLastName.Text;
                    string firstName = window.txtFirstName.Text;
                    string middleName = window.txtMiddleName.Text;
                    string userLogin = window.txtUserLogin.Text;
                    string userPassword = window.txtUserPassword.Text;
                    string role = window.rbnAdministrator.IsChecked.Value ? "Администратор" : "Библиотекарь";
                    int userID = int.Parse(row["UserID"].ToString());

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand("EditUser", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            command.Parameters.Add(new SqlParameter("@UserID", userID));
                            command.Parameters.Add(new SqlParameter("@LastName", lastName));
                            command.Parameters.Add(new SqlParameter("@FirstName", firstName));
                            command.Parameters.Add(new SqlParameter("@MiddleName", middleName));
                            command.Parameters.Add(new SqlParameter("@UserLogin", userLogin));
                            command.Parameters.Add(new SqlParameter("@UserPassword", userPassword));
                            command.Parameters.Add(new SqlParameter("@Role", role));

                            connection.Open();
                            command.ExecuteNonQuery();
                        }
                    }

                    // Обновите DataGrid
                    ViewUsersButton_Click(null, null);
                }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem is DataRowView row)
            {
                // Удалите пользователя
                int userID = int.Parse(row["UserID"].ToString());

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand("DeleteUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("@UserID", userID));

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }

                // Обновите DataGrid
                ViewUsersButton_Click(null, null);
            }
        }

        private void BackToAdminWindow_Click(object sender, RoutedEventArgs e)
        {
            AdminWindow adminWindow = new AdminWindow();
            adminWindow.Show();
            this.Close();
        }
    }

}
