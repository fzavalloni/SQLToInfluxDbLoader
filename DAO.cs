using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SQLtoInfluxDBLoader
{
    public class DAO
    {
        public SqlConnection Connection;

        public DAO(SqlConnection connection)
        {
            this.Connection = connection;
        }
        public DataTable GetSQLQueryResult(string query)
        {
            try
            {
                DataTable dt = new DataTable();

                using (SqlCommand command = new SqlCommand(query, Connection))
                {
                    dt.Load(command.ExecuteReader());
                }

                return dt;
            }
            catch (Exception err)
            {
                throw new Exception("Failed getting SQL Data: " + err.Message);
            }
        }
        public bool IsValidSQLCredentials()
        {
            if (Connection.State == ConnectionState.Open)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
