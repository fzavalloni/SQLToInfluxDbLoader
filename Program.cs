using System;
using System.Linq;
using System.Net;

namespace SQLtoInfluxDBLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                ShowHelp();
                return;
            }

            DAL dal = null;
            InfluxClient client = null;

            try
            {
                dal = new DAL(SQLDatabaseConfigData.ConnString);
            }
            catch
            {
                Console.WriteLine("Error to read SQL Configuration in settings.xml file");
                return;
            }

            try
            {
                switch (args[0].ToLower())
                {
                    case "/testsqlconnection":
                        {
                            //It checks the Database connection and authentication
                            if (dal.IsValidSQLCredentials())
                            {
                                SuccessMessage();
                            }

                            break;
                        }

                    case "/testinfluxdbconnection":
                        {
                            //It checks only the Database Connction, the InfluxDB.Net library does not contain any method that tests it
                            client = new InfluxClient(InfluxDBData.Url, InfluxDBData.UserName, InfluxDBData.Password);

                            var response = client.PingAsync();
                            if (response.Result.Success)
                            {
                                SuccessConnection(InfluxDBData.Url);
                            }

                            break;
                        }

                    case "/run":
                        {

                            System.Data.DataTable dt = null;
                            int counter = 1;

                            //Start config client
                            client = new InfluxClient(InfluxDBData.Url, InfluxDBData.UserName, InfluxDBData.Password);

                            //Read the Queries collection in the Settings.xml file
                            foreach (QueryInfo item in SQLDatabaseConfigData.Queries)
                            {
                                //Run the query
                                dt = dal.GetSQLQueryResult(item.Query);

                                //Convert the data of DataTable to Point[]
                                var pointArray = InfluxClient.GetPointData(dt, InfluxDBData.Servicetype, item.Measurement);

                                //Post all the data to InfluxDB Server
                                client.WriteData(pointArray, item);

                                Console.WriteLine(string.Format("{0} - Query {0} posted successfully", counter));
                                counter++;
                            }

                            Console.WriteLine("DONE....");
                            Environment.Exit(0);

                            break;
                        }
                    default:
                        {
                            ShowHelp();
                            return;
                        }
                }
            }

            catch (Exception err)
            {
                Console.WriteLine("Error: " + err.Message);
                Environment.Exit(2);
            }
        }

        private static void SuccessMessage()
        {
            Console.WriteLine("");
            Console.WriteLine("SUCCESS!!!");
            Console.WriteLine("");
            Console.WriteLine("Authentication Successfully");
        }

        private static void SuccessConnection(string urlInflux)
        {
            Console.WriteLine("");
            Console.WriteLine("SUCCESS!!!");
            Console.WriteLine("");
            Console.WriteLine(string.Format("URL {0} - Reached Successfully",urlInflux));
        }

        private static void ShowHelp()
        {
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Missing parameters!");
            Console.WriteLine("=========================================");
            Console.WriteLine("Parameters usage:");
            Console.WriteLine("");
            Console.WriteLine("/TestSQLConnection  -  Test the SQL credentials");
            Console.WriteLine("");
            Console.WriteLine("/TestInfluxDBConnection  -  Test the InfluxDB credentials");
            Console.WriteLine("");
            Console.WriteLine("/Run  -  Post the SQL Data to InfluxDB");
            Console.WriteLine("");
        }
    }
}
