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
            int maxId = 0;
            if (dataTable.Rows.Count > 0)
            {
                maxId = dataTable.AsEnumerable().Max(r => r.Field<int>(idColumnName));
            }
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
            else if (currentTable == "Works_Books")
            {
                dataAdapter = FillDataGridForDisplay($"EXEC GetWorks_BooksData", out dataTable);
            }
            else if (currentTable == "Books_Shelves")
            {
                dataAdapter = FillDataGridForDisplay($"EXEC GetBooks_ShelvesData", out dataTable);
            }
            else if (currentTable == "Works_Authors")
            {
                dataAdapter = FillDataGridForDisplay($"EXEC GetWorks_AuthorsData", out dataTable);
            }
            else if (currentTable == "Books_Publishers")
            {
                dataAdapter = FillDataGridForDisplay($"EXEC GetBooks_PublishersData", out dataTable);
            }
            else if (currentTable == "Works_Genres")
            {
                dataAdapter = FillDataGridForDisplay($"EXEC GetWorks_GenresData", out dataTable);
            }
            else if (currentTable == "Acts_Books")
            {
                dataAdapter = FillDataGridForDisplay($"EXEC GetGetActs_BooksData", out dataTable);
            }
            else if (currentTable == "BooksInventorisation")
            {
                dataAdapter = FillDataGridForDisplay($"EXEC GetBooksInventorisationData", out dataTable);
            }
            else if (currentTable == "Acts")
            {
                dataAdapter = FillDataGridForDisplay($"EXEC GetActsData", out dataTable);
            }
            else if (currentTable == "LibraryEvents")
            {
                dataAdapter = FillDataGridForDisplay($"EXEC GetLibraryEventsData", out dataTable);
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

        public void AddUser(string lastName, string firstName, string middleName, string userLogin, string userPassword, bool isLibrarian, bool isAdmin)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Добавляем пользователя
                string query = $"EXEC AddUser '{lastName}', '{firstName}', '{middleName}', '{userLogin}', '{userPassword}'";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Получаем ID только что добавленного пользователя
                int userID = GetLastInsertedID("Users");

                // Если пользователь является библиотекарем, добавляем его в таблицу Librarians
                if (isLibrarian)
                {
                    query = $"EXEC AddLibrarian {userID}";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                // Если пользователь является администратором, добавляем его в таблицу Administrators
                if (isAdmin)
                {
                    // Получаем ID только что добавленного библиотекаря
                    int librarianID = GetLastInsertedID("Librarians");

                    query = $"EXEC AddAdministrator {userID}, {librarianID}";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public void EditUser(int userID, string lastName, string firstName, string middleName, string userLogin, string userPassword, bool isLibrarian, bool isAdmin)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Обновляем информацию о пользователе
                string query = $"UPDATE Users SET LastName = '{lastName}', FirstName = '{firstName}', MiddleName = '{middleName}', UserLogin = '{userLogin}', UserPassword = '{userPassword}' WHERE UserID = {userID}";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }

                // TODO: Обновляем информацию о библиотекаре и администраторе
            }
        }


        public void DeleteUser(int userID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Удаляем пользователя
                string query = $"DELETE FROM Users WHERE UserID = {userID}";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public int GetLastInsertedID(string tableName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = $"SELECT MAX(UserID) FROM {tableName}";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    return (int)command.ExecuteScalar();
                }
            }
        }

    }

}
