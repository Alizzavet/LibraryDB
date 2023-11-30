using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Library
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static bool IsAdmin;
        public MainWindow()
        {
            InitializeComponent();
            IsAdmin = false;
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Data Source=ALIZZAVET\\ELIZAVETA;Initial Catalog=Библиотека;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT FirstName, LastName, MiddleName FROM Users INNER JOIN Administrators ON Users.UserID = Administrators.UserID WHERE UserLogin = @UserLogin AND UserPassword = @UserPassword";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserLogin", txtUsername.Text);
                    command.Parameters.AddWithValue("@UserPassword", txtPassword.Password);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            MessageBox.Show($"Добро пожаловать, {reader["FirstName"]} {reader["MiddleName"]} {reader["LastName"]} ! Вы вошли как администратор.");
                            AdminWindow adminWindow = new AdminWindow();
                            adminWindow.Show();
                            IsAdmin = true;
                            this.Close();
                            return;
                        }
                    }
                }

                query = "SELECT FirstName, LastName, MiddleName FROM Users INNER JOIN Librarians ON Users.UserID = Librarians.UserID WHERE UserLogin = @UserLogin AND UserPassword = @UserPassword";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserLogin", txtUsername.Text);
                    command.Parameters.AddWithValue("@UserPassword", txtPassword.Password);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            MessageBox.Show($"Добро пожаловать, {reader["FirstName"]} {reader["MiddleName"]} {reader["LastName"]}! Вы вошли как библиотекарь.");
                            LibraryWindow libraryWindow = new LibraryWindow();
                            libraryWindow.Show();
                            IsAdmin = false;
                            this.Close();
                            return;
                        }
                    }
                }
            }

            MessageBox.Show("Неверный логин и пароль");
        }
    }

}  