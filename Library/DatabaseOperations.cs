using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Library
{
    public class DatabaseOperations
    {
        protected string connectionString = "Data Source=ALIZZAVET\\ELIZAVETA;Initial Catalog=Библиотека;Integrated Security=True";

        public SqlDataAdapter FillDataGrid(string selectCommand, out DataTable dataTable)
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter(selectCommand, connectionString);
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
            dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns[0] };
            return dataAdapter;
        }

        public DataRow AddRow(DataTable dataTable)
        {
            DataRow row = dataTable.NewRow();
            string idColumnName = dataTable.Columns[0].ColumnName;
            int maxId = dataTable.AsEnumerable().Max(r => r.Field<int>(idColumnName));
            row[idColumnName] = maxId + 1;
            return row;
        }

        public void UpdateRow(DataTable dataTable, SqlDataAdapter dataAdapter)
        {
            using (SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter))
            {
                dataAdapter.UpdateCommand = commandBuilder.GetUpdateCommand();
                dataAdapter.InsertCommand = commandBuilder.GetInsertCommand();
                dataAdapter.DeleteCommand = commandBuilder.GetDeleteCommand();
                dataAdapter.Update(dataTable);
            }
        }


        public void DeleteRow(DataRow row, DataTable dataTable, SqlDataAdapter dataAdapter, string currentTable, DataGrid dataGrid)
        {
            // Получаем ID строки, которую нужно удалить
            int id = (int)row[0];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Вызываем хранимую процедуру для удаления записи
                string query = $"EXEC DeleteFrom{currentTable} {id}";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            // Обновляем DataGrid
            if (currentTable == "Sections")
            {
                dataAdapter = FillDataGridForDisplay($"EXEC GetSectionsData", out dataTable);
            }
            else if (currentTable == "Shelves")
            {
                dataAdapter = FillDataGridForDisplay($"EXEC GetShelvesData", out dataTable);
            }
            else
            {
                dataAdapter = FillDataGridForDisplay($"SELECT * FROM {currentTable}", out dataTable);
            }
            dataTable.TableName = currentTable;
            dataGrid.ItemsSource = dataTable.DefaultView;
        }

        
        public SqlDataAdapter FillDataGridFromStoredProcedure(string storedProcedureName, out DataTable dataTable)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                    dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    return dataAdapter;
                }
            }
        }

        public SqlDataAdapter FillDataGridForDisplay(string selectCommand, out DataTable dataTable)
        {
            return FillDataGrid(selectCommand, out dataTable);
        }

        public SqlDataAdapter FillDataGridForEdit(string selectCommand, out DataTable dataTable)
        {
            return FillDataGrid(selectCommand, out dataTable);
        }
    }

}
