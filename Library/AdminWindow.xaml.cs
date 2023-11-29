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
        private SqlDataAdapter editDataAdapter;
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

        private void btnLibraryRooms_Click(object sender, RoutedEventArgs e)
        {
            currentTable = "LibraryRooms";
            dataAdapter = dbOps.FillDataGridForDisplay($"SELECT * FROM {currentTable}", out dataTable);
            editDataAdapter = dbOps.FillDataGridForEdit($"SELECT * FROM {currentTable}", out editDataTable);
            dataTable.TableName = currentTable;
            editDataTable.TableName = currentTable;
            dataGrid.ItemsSource = dataTable.DefaultView;
            dataGrid.Columns[0].Visibility = Visibility.Collapsed;
        }

        private void btnSections_Click(object sender, RoutedEventArgs e)
        {
            currentTable = "Sections";
            dataAdapter = dbOps.FillDataGridForDisplay($"EXEC GetSectionsData", out dataTable);
            editDataAdapter = dbOps.FillDataGridForEdit($"SELECT * FROM {currentTable}", out editDataTable);
            dataTable.TableName = currentTable;
            editDataTable.TableName = currentTable;
            dataGrid.ItemsSource = dataTable.DefaultView;
            dataGrid.Columns[0].Visibility = Visibility.Collapsed;
            dataGrid.Columns[2].Visibility = Visibility.Collapsed;
        }

        private void btnShelves_Click(object sender, RoutedEventArgs e)
        {
            currentTable = "Shelves";
            dataAdapter = dbOps.FillDataGridForDisplay($"EXEC GetShelvesData", out dataTable);
            editDataAdapter = dbOps.FillDataGridForEdit($"SELECT * FROM {currentTable}", out editDataTable);
            dataTable.TableName = currentTable;
            editDataTable.TableName = currentTable;
            dataGrid.ItemsSource = dataTable.DefaultView;
            dataGrid.Columns[0].Visibility = Visibility.Collapsed;
            dataGrid.Columns[2].Visibility = Visibility.Collapsed;
            dataGrid.Columns[4].Visibility = Visibility.Collapsed;
        }
        private void btnWorks_Click(object sender, RoutedEventArgs e)
        {
            currentTable = "Works";
            dataAdapter = dbOps.FillDataGridForDisplay($"EXEC GetWorksData", out dataTable);
            editDataAdapter = dbOps.FillDataGridForEdit($"SELECT * FROM {currentTable}", out editDataTable);
            dataTable.TableName = currentTable;
            editDataTable.TableName = currentTable;
            dataGrid.ItemsSource = dataTable.DefaultView;
            dataGrid.Columns[0].Visibility = Visibility.Collapsed;
        }

        private void btnBooks_Click(object sender, RoutedEventArgs e)
        {
            currentTable = "Books";
            dataAdapter = dbOps.FillDataGridForDisplay($"EXEC GetBooksData", out dataTable);
            editDataAdapter = dbOps.FillDataGridForEdit($"SELECT * FROM {currentTable}", out editDataTable);
            dataTable.TableName = currentTable;
            editDataTable.TableName = currentTable;
            dataGrid.ItemsSource = dataTable.DefaultView;
            dataGrid.Columns[0].Visibility = Visibility.Collapsed;
        }

        private void btnWorksBooks_Click(object sender, RoutedEventArgs e)
        {
            currentTable = "Works_Books";
            dataAdapter = dbOps.FillDataGridForDisplay($"EXEC GetWorks_BooksData", out dataTable);
            editDataAdapter = dbOps.FillDataGridForEdit($"SELECT * FROM {currentTable}", out editDataTable);
            dataTable.TableName = currentTable;
            editDataTable.TableName = currentTable;
            dataGrid.ItemsSource = dataTable.DefaultView;
            dataGrid.Columns[0].Visibility = Visibility.Collapsed;
            dataGrid.Columns[1].Visibility = Visibility.Collapsed;
            dataGrid.Columns[3].Visibility = Visibility.Collapsed;
        }

        private void btnBooksShelves_Click(object sender, RoutedEventArgs e)
        {
            currentTable = "Books_Shelves";
            dataAdapter = dbOps.FillDataGridForDisplay($"EXEC GetBooks_ShelvesData", out dataTable);
            editDataAdapter = dbOps.FillDataGridForEdit($"SELECT * FROM {currentTable}", out editDataTable);
            dataTable.TableName = currentTable;
            editDataTable.TableName = currentTable;
            dataGrid.ItemsSource = dataTable.DefaultView;
            dataGrid.Columns[0].Visibility = Visibility.Collapsed;
            dataGrid.Columns[1].Visibility = Visibility.Collapsed;
            dataGrid.Columns[3].Visibility = Visibility.Collapsed;
        }

        private void btnAuthors_Click(object sender, RoutedEventArgs e)
        {
            currentTable = "Authors";
            dataAdapter = dbOps.FillDataGridForDisplay($"SELECT * FROM {currentTable}", out dataTable);
            editDataAdapter = dbOps.FillDataGridForEdit($"SELECT * FROM {currentTable}", out editDataTable);
            dataTable.TableName = currentTable;
            editDataTable.TableName = currentTable;
            dataGrid.ItemsSource = dataTable.DefaultView;
            dataGrid.Columns[0].Visibility = Visibility.Collapsed;
        }

        private void btnWorksAuthors_Click(object sender, RoutedEventArgs e)
        {
            currentTable = "Works_Authors";
            dataAdapter = dbOps.FillDataGridForDisplay($"EXEC GetWorks_AuthorsData", out dataTable);
            editDataAdapter = dbOps.FillDataGridForEdit($"SELECT * FROM {currentTable}", out editDataTable);
            dataTable.TableName = currentTable;
            editDataTable.TableName = currentTable;
            dataGrid.ItemsSource = dataTable.DefaultView;
            dataGrid.Columns[0].Visibility = Visibility.Collapsed;
            dataGrid.Columns[1].Visibility = Visibility.Collapsed;
            dataGrid.Columns[3].Visibility = Visibility.Collapsed;
        }

        private void btnGenres_Click(object sender, RoutedEventArgs e)
        {
            currentTable = "Genres";
            dataAdapter = dbOps.FillDataGridForDisplay($"SELECT * FROM {currentTable}", out dataTable);
            editDataAdapter = dbOps.FillDataGridForEdit($"SELECT * FROM {currentTable}", out editDataTable);
            dataTable.TableName = currentTable;
            editDataTable.TableName = currentTable;
            dataGrid.ItemsSource = dataTable.DefaultView;
            dataGrid.Columns[0].Visibility = Visibility.Collapsed;
        }

        private void btnWorksGenres_Click(object sender, RoutedEventArgs e)
        {
            currentTable = "Works_Genres";
            dataAdapter = dbOps.FillDataGridForDisplay($"EXEC GetWorks_GenresData", out dataTable);
            editDataAdapter = dbOps.FillDataGridForEdit($"SELECT * FROM {currentTable}", out editDataTable);
            dataTable.TableName = currentTable;
            editDataTable.TableName = currentTable;
            dataGrid.ItemsSource = dataTable.DefaultView;
            dataGrid.Columns[0].Visibility = Visibility.Collapsed;
            dataGrid.Columns[1].Visibility = Visibility.Collapsed;
            dataGrid.Columns[3].Visibility = Visibility.Collapsed;
        }

        private void btnPublishers_Click(object sender, RoutedEventArgs e)
        {
            currentTable = "Publishers";
            dataAdapter = dbOps.FillDataGridForDisplay($"SELECT * FROM {currentTable}", out dataTable);
            editDataAdapter = dbOps.FillDataGridForEdit($"SELECT * FROM {currentTable}", out editDataTable);
            dataTable.TableName = currentTable;
            editDataTable.TableName = currentTable;
            dataGrid.ItemsSource = dataTable.DefaultView;
            dataGrid.Columns[0].Visibility = Visibility.Collapsed;
        }

        private void btnBooksPublishers_Click(object sender, RoutedEventArgs e)
        {
            currentTable = "Books_Publishers";
            dataAdapter = dbOps.FillDataGridForDisplay($"EXEC GetBooks_PublishersData", out dataTable);
            editDataAdapter = dbOps.FillDataGridForEdit($"SELECT * FROM {currentTable}", out editDataTable);
            dataTable.TableName = currentTable;
            editDataTable.TableName = currentTable;
            dataGrid.ItemsSource = dataTable.DefaultView;
            dataGrid.Columns[0].Visibility = Visibility.Collapsed;
            dataGrid.Columns[1].Visibility = Visibility.Collapsed;
            dataGrid.Columns[3].Visibility = Visibility.Collapsed;
        }

        private void btnActs_Click(object sender, RoutedEventArgs e)
        {
            currentTable = "Acts";
            dataAdapter = dbOps.FillDataGridForDisplay($"EXEC GetActsData", out dataTable);
            editDataAdapter = dbOps.FillDataGridForEdit($"SELECT * FROM {currentTable}", out editDataTable);
            dataTable.TableName = currentTable;
            editDataTable.TableName = currentTable;
            dataGrid.ItemsSource = dataTable.DefaultView;
            dataGrid.Columns[0].Visibility = Visibility.Collapsed;
            dataGrid.Columns[1].Visibility = Visibility.Collapsed;
            dataGrid.Columns[3].Visibility = Visibility.Collapsed;
        }

        private void btnActsBooks_Click(object sender, RoutedEventArgs e)
        {
            currentTable = "Acts_Books";
            dataAdapter = dbOps.FillDataGridForDisplay($"EXEC GetActs_BooksData", out dataTable);
            editDataAdapter = dbOps.FillDataGridForEdit($"SELECT * FROM {currentTable}", out editDataTable);
            dataTable.TableName = currentTable;
            editDataTable.TableName = currentTable;
            dataGrid.ItemsSource = dataTable.DefaultView;

            // Скрываем столбцы с первичными и вторичными ключами
            dataGrid.Columns[0].Visibility = Visibility.Collapsed; 
            dataGrid.Columns[1].Visibility = Visibility.Collapsed; 
            dataGrid.Columns[3].Visibility = Visibility.Collapsed; 
        }


        private void btnInventory_Click(object sender, RoutedEventArgs e)
        {
            currentTable = "BooksInventorisation";
            dataAdapter = dbOps.FillDataGridForDisplay($"EXEC GetBooksInventorisationData", out dataTable);
            editDataAdapter = dbOps.FillDataGridForEdit($"SELECT * FROM {currentTable}", out editDataTable);
            dataTable.TableName = currentTable;
            editDataTable.TableName = currentTable;
            dataGrid.ItemsSource = dataTable.DefaultView;
            dataGrid.Columns[0].Visibility = Visibility.Collapsed;
            dataGrid.Columns[1].Visibility = Visibility.Collapsed;
        }

        private void btnExecutionAct_Click(object sender, RoutedEventArgs e)
        {
            ActWindow actWindow= new ActWindow();
            actWindow.Show();
            Close();
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

        private void btnEditUsers_Click(object sender, RoutedEventArgs e)
        {
            EditUserWindow editUserWindow = new EditUserWindow();
            editUserWindow.Show();
            Close();
        }


        private void DataView()
        {
            if (currentTable == "Sections")
            {
                dataAdapter = dbOps.FillDataGridForDisplay($"EXEC GetSectionsData", out dataTable);
            }
            else if (currentTable == "Shelves")
            {
                dataAdapter = dbOps.FillDataGridForDisplay($"EXEC GetShelvesData", out dataTable);
            }
            else if (currentTable == "Works_Books")
            {
                dataAdapter = dbOps.FillDataGridForDisplay($"EXEC GetWorks_BooksData", out dataTable);
            }
            else if (currentTable == "Books_Shelves")
            {
                dataAdapter = dbOps.FillDataGridForDisplay($"EXEC GetBooks_ShelvesData", out dataTable);
            }
            else if (currentTable == "Books_Publishers")
            {
                dataAdapter = dbOps.FillDataGridForDisplay($"EXEC GetBooks_PublishersData", out dataTable);
            }
            else if (currentTable == "Works_Authors")
            {
                dataAdapter = dbOps.FillDataGridForDisplay($"EXEC GetWorks_AuthorsData", out dataTable);
            }
            else if (currentTable == "Works_Genres")
            {
                dataAdapter = dbOps.FillDataGridForDisplay($"EXEC GetWorks_GenresData", out dataTable);
            }
            else if (currentTable == "Acts_Books")
            {
                dataAdapter = dbOps.FillDataGridForDisplay($"EXEC GetActs_BooksData", out dataTable);
            }
            else if (currentTable == "BooksInventorisation")
            {
                dataAdapter = dbOps.FillDataGridForDisplay($"EXEC GetBooksInventorisationData", out dataTable);
            }
            else if (currentTable == "Acts")
            {
                dataAdapter = dbOps.FillDataGridForDisplay($"EXEC GetActsData", out dataTable);
            }
            else if (currentTable == "LibraryEvents")
            {
                dataAdapter = dbOps.FillDataGridForDisplay($"EXEC GetLibraryEventsData", out dataTable);
            }
            else
            {
                dataAdapter = dbOps.FillDataGridForDisplay($"SELECT * FROM {currentTable}", out dataTable);
            }
            dataTable.TableName = currentTable;
            dataGrid.ItemsSource = dataTable.DefaultView;
        }
    }

}
