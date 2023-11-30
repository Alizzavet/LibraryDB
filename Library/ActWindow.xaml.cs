﻿using System;
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
    /// Логика взаимодействия для ActWindow.xaml
    /// </summary>
    public partial class ActWindow : Window
    {
        protected string connectionString = "Data Source=ALIZZAVET\\ELIZAVETA;Initial Catalog=Библиотека;Integrated Security=True";
        public ActWindow()
        {
            InitializeComponent();
        }

        private void LoadData_Click(object sender, RoutedEventArgs e)
        {
            LoadDataGrid();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            EditActWindow editActWindow = new EditActWindow();
            editActWindow.ShowDialog();
            LoadDataGrid();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem is DataRowView row)
            {
                int actId = (int)row["ActID"];
                EditActWindow editActWindow = new EditActWindow(actId);
                editActWindow.ShowDialog();
                LoadDataGrid();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem is DataRowView row)
            {
                int actId = (int)row["ActID"];

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("DeleteAct", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ActID", actId);
                    command.ExecuteNonQuery();
                }
                LoadDataGrid();
            }
        }

        private void LoadDataGrid()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("GetActsAndActsBooksData", connection);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable("ActsAndActsBooks");
                dataAdapter.Fill(dataTable);
                dataGrid.ItemsSource = dataTable.DefaultView;
                dataGrid.Columns[0].Visibility = Visibility.Collapsed;
                dataGrid.Columns[1].Visibility = Visibility.Collapsed;
                dataGrid.Columns[3].Visibility = Visibility.Collapsed;
                dataGrid.Columns[8].Visibility = Visibility.Collapsed;
                dataGrid.Columns[7].Visibility = Visibility.Collapsed;
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
