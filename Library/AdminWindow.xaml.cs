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
        private DataTable editDataTable;
        private string currentTable;

        public AdminWindow()
        {
            InitializeComponent();
        }

        private void btnLibraryEvents_Click(object sender, RoutedEventArgs e)
        {
            currentTable = "LibraryEvents";
            dataAdapter = dbOps.FillDataGrid($"SELECT * FROM {currentTable}", out dataTable);
            dataTable.TableName = currentTable;
            dataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void btnSubscriptions_Click(object sender, RoutedEventArgs e)
        {
            currentTable = "Subscriptions";
            dataAdapter = dbOps.FillDataGrid($"SELECT * FROM {currentTable}", out dataTable);
            dataTable.TableName = currentTable;
            dataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void btnLibraryRooms_Click(object sender, RoutedEventArgs e)
        {
            currentTable = "LibraryRooms";
            dataAdapter = dbOps.FillDataGrid($"SELECT * FROM {currentTable}", out dataTable);
            dataTable.TableName = currentTable;
            dataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void btnSections_Click(object sender, RoutedEventArgs e)
        {
            currentTable = "Sections";
            dataAdapter = dbOps.FillDataGrid($"SELECT * FROM {currentTable}", out dataTable);
            dataTable.TableName = currentTable;
            dataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void btnShelves_Click(object sender, RoutedEventArgs e)
        {
            currentTable = "Shelves";
            dataAdapter = dbOps.FillDataGrid($"SELECT * FROM {currentTable}", out dataTable);
            dataTable.TableName = currentTable;
            dataGrid.ItemsSource = dataTable.DefaultView;
        }


        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRow row = dbOps.AddRow(dataTable);
                EditWindow editWindow = new EditWindow(row, true); // передаем true, потому что это новая запись
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
                    EditWindow editWindow = new EditWindow(row, false); // передаем false, потому что это существующая запись
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
