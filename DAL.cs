using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace SQLtoInfluxDBLoader
{
    class DAL
    {
        private string ConnString { get; set; }
        public DAL(string connString)
        {
            this.ConnString = connString;
        }
        public DataTable GetSQLQueryResult(string query)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection connection = new SqlConnection(this.ConnString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();

                        dt.Load(command.ExecuteReader());
                    }
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
            try
            {
                using (SqlConnection connection = new SqlConnection(this.ConnString))
                {
                    connection.Open();
                }
                return true;
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
        }
    }
}
