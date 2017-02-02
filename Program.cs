using SQLtoInfluxDBLoader.Configurations;
using System;
using System.Linq;
using System.Net;
using System.Data.SqlClient;

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

            InfluxClient client = null;
            Settings settings;

            try
            {
                //Create a config example file if it does not exist
                Serializer.SerializerConfigFile();
                settings = Serializer.GetConfiguration();
            }
            catch(Exception err)
            {
                Console.WriteLine("Error in configuration file: " + err.InnerException.ToString());
                return;
            }

            try
            {
                switch (args[0].ToLower())
                {
                    case "/testsqlconnection":
                        {
                            //It checks the Database connection and authentication                             
                            foreach (Provider provider in settings.DatabaseSettings.ProviderList)
                            {
                                using (SqlConnection connection = new SqlConnection(provider.ConnectionString))
                                {
                                    connection.Open();

                                    DAO dao = new DAO(connection);

                                    if (dao.IsValidSQLCredentials())
                                    {
                                        SuccessSQLAuthMessage(provider.ConnectionString);
                                        foreach (QueryItem item in provider.Queries)
                                        {
                                            Console.WriteLine("=================================");
                                            Console.WriteLine("Query Id: " + item.Id);
                                            System.Data.DataTable ft = dao.GetSQLQueryResult(item.Query);
                                            Console.WriteLine("Returned " + ft.Rows.Count + " row(s).");
                                        }
                                    }
                                }
                            }

                            break;
                        }

                    case "/testinfluxdbconnection":
                        {
                            //It checks only the Database Connction, the InfluxDB.Net library does not contain any method that tests it
                            client = new InfluxClient(settings.InfluxSetting.Url, settings.InfluxSetting.UserName, settings.InfluxSetting.Password);

                            var response = client.PingAsync();
                            if (response.Result.Success)
                            {
                                SuccessConnection(settings.InfluxSetting.Url);
                            }

                            break;
                        }

                    case "/run":
                        {
                            System.Data.DataTable dt = null;
                            DAO dao = null;
                            int counter = 1;                            

                            //Start config client
                            InfluxDBSettings config = Serializer.GetConfiguration().InfluxSetting;
                            client = new InfluxClient(config.Url, config.UserName, config.Password);                            

                            foreach (Provider provider in settings.DatabaseSettings.ProviderList)
                            {
                                using (SqlConnection connection = new SqlConnection(provider.ConnectionString))
                                {
                                    dao = new DAO(connection);
                                    //Read the Queries collection in the Settings.xml file
                                    foreach (QueryItem item in provider.Queries)
                                    {
                                        //Run the query
                                        dt = dao.GetSQLQueryResult(item.Query);

                                        //Convert the data of DataTable to Point[]
                                        var pointArray = InfluxClient.GetPointData(dt, settings.InfluxSetting.Tags, item.Measurement);

                                        //Post all the data to InfluxDB Server
                                        client.WriteData(pointArray,settings.InfluxSetting);

                                        Console.WriteLine(string.Format("{0} - Query {0} posted successfully", counter));
                                        counter++;
                                    }
                                }
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

        private static void SuccessSQLAuthMessage(string provider)
        {
            Console.WriteLine("");
            Console.WriteLine("SUCCESS!!!");
            Console.WriteLine("");
            Console.WriteLine("Authentication Successfully");
            Console.WriteLine("Provider: " + provider);
            Console.WriteLine("============================================");
        }

        private static void SuccessSQLQueryMessage(int queryId, int queryResultCount)
        {
            Console.WriteLine("============================================");
            Console.WriteLine("Query Id: " + queryId);
            Console.WriteLine("Returned: " + queryResultCount + " row(s).");
            Console.WriteLine("============================================");
        }

        private static void SuccessConnection(string urlInflux)
        {
            Console.WriteLine("");
            Console.WriteLine("SUCCESS!!!");
            Console.WriteLine("");
            Console.WriteLine(string.Format("URL {0} - Reached Successfully", urlInflux));
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
