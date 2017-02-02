using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace SQLtoInfluxDBLoader
{
    [Serializable]
    public class Settings
    {
        public InfluxDBSettings InfluxSetting { get; set; }
        public DatabaseSettings DatabaseSettings { get; set; }
    }

    [Serializable]
    public class DatabaseSettings
    {
        public List<Provider> ProviderList { get; set; }
    }

    [Serializable]
    public class InfluxDBSettings
    {
        public string Databasename { get; set; }
        public string Url { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<Tag> Tags { get; set; }
    }

    [Serializable]
    public class Tag
    {
        public string TagName { get; set; }
        public string TagValue { get; set; }
    }

    [Serializable]
    public class Provider
    {
        public string ConnectionString { get; set; }
        public List<QueryItem> Queries { get; set; }
    }

    [Serializable]
    public class QueryItem
    {
        [XmlAttribute(AttributeName = "id")]
        public int Id { get; set; }

        [XmlAttribute(AttributeName = "query")]
        public string Query { get; set; }

        [XmlAttribute(AttributeName = "measurement")]
        public string Measurement { get; set; }
    }
}
