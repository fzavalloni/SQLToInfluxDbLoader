using System;
using System.Collections.Generic;

namespace SQLtoInfluxDBLoader
{    
    public class InfluxDBData
    {
        private static string url = Tools.ReadXmlAttribute("url");
        public static string Url
        {
            get
            {
                return url;
            }

        }

        private static string databaseName = Tools.ReadXmlAttribute("databaseName");
        public static string DatabaseName
        {
            get
            {
                return databaseName;
            }

        }

        private static string userName = Tools.ReadXmlAttribute("userName");
        public static string UserName
        {
            get
            {
                return userName;
            }
        }

        private static string password = Tools.ReadXmlAttribute("password");
        public static string Password
        {
            get
            {
                return password;
            }
        }

        private static string servicetype = Tools.ReadXmlAttribute("service_type");
        public static string Servicetype
        {
            get
            {
                return servicetype;
            }
        }
    }

    public class SQLDatabaseConfigData
    {
        private static string connString = Tools.ReadXmlAttribute("connectionString");
        public static string ConnString
        {
            get
            {
                return connString;
            }
        }

        private static List<QueryInfo> queries = Tools.ReadXmlCollectionAttribute();
        public static List<QueryInfo> Queries 
        {
            get
            {
                return queries;
            }
        }
    }

    public class QueryInfo
    {
        public string Query { get; set; }
        public string Measurement { get; set; }

        public QueryInfo(string query, string measurement)
        {
            this.Query = query;
            this.Measurement = measurement;
        }
    }
}
