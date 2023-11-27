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
            dataAdapter = dbOps.FillDataGridForDisplay($"SELECT * FROM {currentTable}", out dataTable);
            editDataAdapter = dbOps.FillDataGridForEdit($"SELECT * FROM {currentTable}", out editDataTable);
            dataTable.TableName = currentTable;
            editDataTable.TableName = currentTable;
            dataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void btnSubscriptions_Click(object sender, RoutedEventArgs e)
        {
            currentTable = "Subscriptions";
            dataAdapter = dbOps.FillDataGridForDisplay($"SELECT * FROM {currentTable}", out dataTable);
            editDataAdapter = dbOps.FillDataGridForEdit($"SELECT * FROM {currentTable}", out editDataTable);
            dataTable.TableName = currentTable;
            editDataTable.TableName = currentTable;
            dataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void btnLibraryRooms_Click(object sender, RoutedEventArgs e)
        {
            currentTable = "LibraryRooms";
            dataAdapter = dbOps.FillDataGridForDisplay($"SELECT * FROM {currentTable}", out dataTable);
            editDataAdapter = dbOps.FillDataGridForEdit($"SELECT * FROM {currentTable}", out editDataTable);
            dataTable.TableName = currentTable;
            editDataTable.TableName = currentTable;
            dataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void btnSections_Click(object sender, RoutedEventArgs e)
        {
            currentTable = "Sections";
            dataAdapter = dbOps.FillDataGridForDisplay($"EXEC GetSectionsData", out dataTable);
            editDataAdapter = dbOps.FillDataGridForEdit($"SELECT * FROM {currentTable}", out editDataTable);
            dataTable.TableName = currentTable;
            editDataTable.TableName = currentTable;
            dataGrid.ItemsSource = dataTable.DefaultView;
        }

        private void btnShelves_Click(object sender, RoutedEventArgs e)
        {
            currentTable = "Shelves";
            dataAdapter = dbOps.FillDataGridForDisplay($"EXEC GetShelvesData", out dataTable);
            editDataAdapter = dbOps.FillDataGridForEdit($"SELECT * FROM {currentTable}", out editDataTable);
            dataTable.TableName = currentTable;
            editDataTable.TableName = currentTable;
            dataGrid.ItemsSource = dataTable.DefaultView;
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

                    // Обновляем dataTable и DataGrid
                    if (currentTable == "Sections")
                    {
                        dataAdapter = dbOps.FillDataGridForDisplay($"EXEC GetSectionsData", out dataTable);
                    }
                    else if (currentTable == "Shelves")
                    {
                        dataAdapter = dbOps.FillDataGridForDisplay($"EXEC GetShelvesData", out dataTable);
                    }
                    else
                    {
                        dataAdapter = dbOps.FillDataGridForDisplay($"SELECT * FROM {currentTable}", out dataTable);
                    }
                    dataTable.TableName = currentTable;
                    dataGrid.ItemsSource = dataTable.DefaultView;
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
                    EditWindow editWindow = new EditWindow(row, false);
                    if (editWindow.ShowDialog() == true)
                    {
                        dbOps.UpdateRow(editDataTable, editDataAdapter);

                        // Обновляем реальные данные в базе данных
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
                int id = (int)row[0]; // Получаем ID строки здесь
                dbOps.DeleteRow(row, editDataTable, dataAdapter, currentTable, dataGrid);
            }
        }

    }

}
