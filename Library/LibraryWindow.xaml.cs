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
        private string connectionString = "Data Source=ALIZZAVET\\ELIZAVETA;Initial Catalog=Библиотека;Integrated Security=True";
        private SqlDataAdapter dataAdapter;
        private DataTable dataTable;
        private string currentTable;

        public LibraryWindow()
        {
            InitializeComponent();
        }

        private void btnLibraryEvents_Click(object sender, RoutedEventArgs e)
        {
            currentTable = "LibraryEvents";
            FillDataGrid($"SELECT * FROM {currentTable}");
        }

        private void btnSubscriptions_Click(object sender, RoutedEventArgs e)
        {
            currentTable = "Subscriptions";
            FillDataGrid($"SELECT * FROM {currentTable}");
        }

        private void FillDataGrid(string selectCommand)
        {
            dataAdapter = new SqlDataAdapter(selectCommand, connectionString);
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
            dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            dataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            DataRow row = dataTable.NewRow();
            // Получаем имя первого столбца (ID)
            string idColumnName = dataTable.Columns[0].ColumnName;
            // Генерируем новый ID
            int maxId = dataTable.AsEnumerable().Max(r => r.Field<int>(idColumnName));
            row[idColumnName] = maxId + 1;
            EditWindow editWindow = new EditWindow(row);
            if (editWindow.ShowDialog() == true)
            {
                dataTable.Rows.Add(row);
                dataAdapter.Update(dataTable);
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem is DataRowView rowView)
            {
                DataRow row = rowView.Row;
                EditWindow editWindow = new EditWindow(row);
                if (editWindow.ShowDialog() == true)
                {
                    dataAdapter.Update(dataTable);
                }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem is DataRowView rowView)
            {
                DataRow row = rowView.Row;
                row.Delete();
                dataAdapter.Update(dataTable);
            }
        }
    }
}
