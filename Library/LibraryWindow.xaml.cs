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
    /// Логика взаимодействия для LibraryWindow.xaml
    /// </summary>
    public partial class LibraryWindow : Window
    {
        private DatabaseOperations dbOps = new DatabaseOperations();
        private SqlDataAdapter dataAdapter;
        private SqlDataAdapter editDataAdapter;
        private DataTable dataTable;
        private DataTable editDataTable;
        private string currentTable;

        public LibraryWindow()
        {
            InitializeComponent();
        }

        private void btnLibraryEvents_Click(object sender, RoutedEventArgs e)
        {
            currentTable = "LibraryEvents";
            dataAdapter = dbOps.FillDataGridForDisplay($"EXEC GetLibraryEventsData", out dataTable);
            editDataAdapter = dbOps.FillDataGridForEdit($"SELECT * FROM {currentTable}", out editDataTable);
            dataTable.TableName = currentTable;
            editDataTable.TableName = currentTable;
            dataGrid.ItemsSource = dataTable.DefaultView;

            dataGrid.Columns[0].Visibility = Visibility.Collapsed;
            dataGrid.Columns[4].Visibility = Visibility.Collapsed;
        }

        private void btnSubscriptions_Click(object sender, RoutedEventArgs e)
        {
            currentTable = "Subscriptions";
            dataAdapter = dbOps.FillDataGridForDisplay($"SELECT * FROM {currentTable}", out dataTable);
            editDataAdapter = dbOps.FillDataGridForEdit($"SELECT * FROM {currentTable}", out editDataTable);
            dataTable.TableName = currentTable;
            editDataTable.TableName = currentTable;
            dataGrid.ItemsSource = dataTable.DefaultView;
            dataGrid.Columns[0].Visibility = Visibility.Collapsed;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataRow row = dbOps.AddRow(editDataTable);
                EditWindow editWindow = new EditWindow(row, true); // передаем true, потому что это новая запись
                if (editWindow.ShowDialog() == true)
                {
                    editDataTable.Rows.Add(row);
                    dbOps.UpdateRow(editDataTable, editDataAdapter); // Обновляем editDataTable

                    DataView();
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
                    DataRow selectedRow = rowView.Row;
                    DataRow editRow = editDataTable.Rows.Find(selectedRow[0]); // Находим редактируемую строку в editDataTable

                    if (editRow != null)
                    {
                        EditWindow editWindow = new EditWindow(editRow, false); // передаем false, потому что это существующая запись
                        if (editWindow.ShowDialog() == true)
                        {
                            // Обновляем editDataTable
                            dbOps.UpdateRow(editDataTable, editDataAdapter);

                            DataView();
                        }
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
                int id = (int)row[0]; // Получаем ID строки здесь
                dbOps.DeleteRow(row, editDataTable, dataAdapter, currentTable, dataGrid);
            }
        }

        private void btnExecutionAct_Click(object sender, RoutedEventArgs e)
        {
            ActWindow actWindow = new ActWindow();
            actWindow.Show();
            Close();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            // Вызов окна подтверждения выхода
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите выйти?", "Подтверждение выхода",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void DemonstrationButton_Click(object sender, RoutedEventArgs e)
        {
            DemonstrationWindow demonstrationWindow = new DemonstrationWindow();
            demonstrationWindow.Show();
        }


        private void DataView()
        {
            if (currentTable == "Acts")
            {
                dataAdapter = dbOps.FillDataGridForDisplay($"EXEC GetActsData", out dataTable);
            }
            else if (currentTable == "LibraryEvents")
            {
                dataAdapter = dbOps.FillDataGridForDisplay($"EXEC GetLibraryEventsData", out dataTable);
                DataGridView();
                dataGrid.Columns[4].Visibility = Visibility.Collapsed;
            }
            else
            {
                dataAdapter = dbOps.FillDataGridForDisplay($"SELECT * FROM {currentTable}", out dataTable);
                DataGridView();
            }
        }

        private void DataGridView()
        {
            dataTable.TableName = currentTable;
            dataGrid.ItemsSource = dataTable.DefaultView;
            dataGrid.Columns[0].Visibility = Visibility.Collapsed;
        }

    }

}
