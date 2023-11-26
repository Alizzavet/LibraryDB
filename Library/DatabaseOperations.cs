using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if (dataAdapter == null)
            {
                throw new InvalidOperationException("dataAdapter не был инициализирован.");
            }
            dataAdapter.Update(dataTable);
        }

        public void DeleteRow(DataRow row, DataTable dataTable, SqlDataAdapter dataAdapter)
        {
            row.Delete();
            dataAdapter.Update(dataTable);
        }
    }

}
