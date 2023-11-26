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
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        private DatabaseOperations dbOps = new DatabaseOperations();
        private SqlDataAdapter dataAdapter;
        private DataTable dataTable;
        private string currentTable;

        public AdminWindow()
        {
            InitializeComponent();
        }

        private void btnLibraryEvents_Click(object sender, RoutedEventArgs e)
        {
            currentTable = "LibraryEvents";
            dataAdapter = dbOps.FillDataGrid($"SELECT * FROM {currentTable}", out dataTable);
            dataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void btnSubscriptions_Click(object sender, RoutedEventArgs e)
        {
            currentTable = "Subscriptions";
            dataAdapter = dbOps.FillDataGrid($"SELECT * FROM {currentTable}", out dataTable);
            dataGrid.ItemsSource = dataTable.DefaultView;
        }

        // Добавьте здесь новые обработчики событий для новых таблиц администратора

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRow row = dbOps.AddRow(dataTable);
                EditWindow editWindow = new EditWindow(row);
                if (editWindow.ShowDialog() == true)
                {
                    dataTable.Rows.Add(row);
                    dbOps.UpdateRow(dataTable, dataAdapter);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dataGrid.SelectedItem is DataRowView rowView)
                {
                    DataRow row = rowView.Row;
                    EditWindow editWindow = new EditWindow(row);
                    if (editWindow.ShowDialog() == true)
                    {
                        dbOps.UpdateRow(dataTable, dataAdapter);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem is DataRowView rowView)
            {
                DataRow row = rowView.Row;
                dbOps.DeleteRow(row, dataTable, dataAdapter);
            }
        }
    }

}
