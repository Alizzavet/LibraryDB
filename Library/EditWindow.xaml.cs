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

        public EditWindow(DataRow row)
        {
            InitializeComponent();
            this.row = row;

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
                    if (column.ColumnName == "LibrarianID")
                    {
                        ComboBox comboBoxLibrarians = new ComboBox
                        {
                            Name = "comboBoxLibrarians",
                            HorizontalAlignment = HorizontalAlignment.Left,
                            Margin = new Thickness(10, 0, 0, 10),
                            VerticalAlignment = VerticalAlignment.Top,
                            Width = 200
                        };
                        stackPanel.Children.Insert(stackPanel.Children.Count - 2, comboBoxLibrarians);
                        FillLibrariansComboBox(comboBoxLibrarians);
                        controls.Add(column.ColumnName, comboBoxLibrarians);

                        if (int.TryParse(row[column].ToString(), out int librarianID))
                        {
                            comboBoxLibrarians.SelectedValue = librarianID;
                        }
                        else
                        {
                            comboBoxLibrarians.SelectedValue = null;
                        }
                    }
                    else
                    {
                        TextBox textBox = new TextBox { Text = row[column].ToString() };
                        textBox.ToolTip = column.ColumnName;
                        if (column == row.Table.Columns[0])
                        {
                            textBox.IsEnabled = false;
                        }
                        stackPanel.Children.Insert(stackPanel.Children.Count - 2, textBox);
                        controls.Add(column.ColumnName, textBox);
                    }
                }
            }
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
    }
}
