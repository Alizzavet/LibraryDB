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
    /// Логика взаимодействия для EditActWindow.xaml
    /// </summary>
    public partial class EditActWindow : Window
    {
        private int? actId; // Используется для хранения ID акта при редактировании
        protected string connectionString = "Data Source=ALIZZAVET\\ELIZAVETA;Initial Catalog=Библиотека;Integrated Security=True";

        public EditActWindow(int? actId = null)
        {
            InitializeComponent();

            // Заполните все ComboBox данными
            FillLibrariansComboBox(comboBoxLibrarians);
            FillSubscriptionsComboBox(comboBoxSubscriptions);
            FillInventorisationComboBox(comboBoxInventorisation);
            FillActionTypeComboBox(comboBoxActionType);

            this.actId = actId;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxLibrarians.SelectedItem == null || comboBoxSubscriptions.SelectedItem == null ||
        comboBoxActionType.SelectedItem == null || datePickerEventDate.SelectedDate == null ||
        comboBoxInventorisation.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, заполните все поля перед сохранением.");
                return;
            }
            int librarianId = ((KeyValuePair<int, string>)comboBoxLibrarians.SelectedItem).Key;
            int subscriptionId = ((KeyValuePair<int, string>)comboBoxSubscriptions.SelectedItem).Key;
            int actionType = ((KeyValuePair<int, string>)comboBoxActionType.SelectedItem).Key;
            DateTime eventDate = datePickerEventDate.SelectedDate.Value;
            int bookInventoryId = ((KeyValuePair<int, string>)comboBoxInventorisation.SelectedItem).Key;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command;

                    // Проверка доступности книги перед добавлением нового акта
                    if (actionType == 1) // Взял(а)
                    {
                        command = new SqlCommand("SELECT COUNT(*) FROM Acts_Books ab INNER JOIN Acts a ON ab.ActID = a.ActID WHERE ab.BookInventoryID = @BookInventoryID AND a.ActionType = 1 AND NOT EXISTS (SELECT 1 FROM Acts_Books ab2 INNER JOIN Acts a2 ON ab2.ActID = a2.ActID WHERE ab2.BookInventoryID = ab.BookInventoryID AND a2.ActionType = 0)", connection);
                        command.Parameters.AddWithValue("@BookInventoryID", bookInventoryId);
                        int count = (int)command.ExecuteScalar();
                        if (count > 0)
                        {
                            MessageBox.Show("Книга уже взята и еще не вернули.");
                            return;
                        }
                    }
                    else if (actionType == 0) // Вернул(а)
                    {
                        command = new SqlCommand("SELECT COUNT(*) FROM Acts_Books ab INNER JOIN Acts a ON ab.ActID = a.ActID WHERE ab.BookInventoryID = @BookInventoryID AND a.ActionType = 1 AND NOT EXISTS (SELECT 1 FROM Acts_Books ab2 INNER JOIN Acts a2 ON ab2.ActID = a2.ActID WHERE ab2.BookInventoryID = ab.BookInventoryID AND a2.ActionType = 0)", connection);
                        command.Parameters.AddWithValue("@BookInventoryID", bookInventoryId);
                        int count = (int)command.ExecuteScalar();
                        if (count == 0)
                        {
                            MessageBox.Show("Книга еще не была взята.");
                            return;
                        }
                    }

                    if (actId == null)
                    {
                        // Добавить новый акт
                        command = new SqlCommand("AddAct", connection);
                    }
                    else
                    {
                        // Редактировать существующий акт
                        command = new SqlCommand("EditAct", connection);
                        command.Parameters.AddWithValue("@ActID", actId.Value);
                    }

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@LibrarianID", librarianId);
                    command.Parameters.AddWithValue("@SubscriptionID", subscriptionId);
                    command.Parameters.AddWithValue("@ActionType", actionType);
                    command.Parameters.AddWithValue("@EventDate", eventDate);
                    command.Parameters.AddWithValue("@BookInventoryID", bookInventoryId);

                    command.ExecuteNonQuery();
                }

                // Закрыть это окно
                this.Close();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 51000)
                {
                    MessageBox.Show($"{ex.Message}");
                }
                else
                {
                    throw;
                }
            }
            this.Close();
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

        public void FillInventorisationComboBox(ComboBox comboBoxInventorisation)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT BookInventoryID, BookName FROM BooksInventorisation INNER JOIN Books ON BooksInventorisation.BookID = Books.BookID WHERE BooksInventorisation.IsAvailable = 1";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                comboBoxInventorisation.Items.Add(new KeyValuePair<int, string>((int)reader["BookInventoryID"], reader["BookName"].ToString()));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при заполнении списка инвентаризации: {ex.Message}");
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
