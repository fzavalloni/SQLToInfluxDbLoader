using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace SQLtoInfluxDBLoader.Configurations
{
    public static class Serializer
    {
        private static readonly string configFileName = @".\Settings.xml";        

        public static Settings GetConfiguration()
        {
            return DeserializerConfigFile();
        }

        public static void SerializerConfigFile()
        {
            if (!System.IO.File.Exists(configFileName))
            {
                Settings settings = ExampleFileSetting();
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                using (TextWriter writer = new StreamWriter(configFileName))
                {
                    serializer.Serialize(writer, settings);
                }
            }
        }


        public static DatabaseSettings GetExampleDatabaseSettings()
        {
            //Adding configuration
            Provider provider = new Provider();
            provider.ConnectionString = "Data Source=Server01; Initial Catalog=DBName;Integrated Security=SSPI;";
            //Adding Querie collection
            List<QueryItem> queries = new List<QueryItem>();
            queries.Add(GetQuery(1, "EXEC sp_GetStatistics", "Report2017"));
            queries.Add(GetQuery(2, "EXEC sp_GetStorageSpace", "StorageReport2017"));
            provider.Queries = queries;
            //Adding provider list
            List<Provider> providerList = new List<Provider>();
            providerList.Add(provider);

            DatabaseSettings databaseSettings = new DatabaseSettings();
            databaseSettings.ProviderList = providerList;

            return databaseSettings;
        }

        public static Settings ExampleFileSetting()
        {
            //Create the class to generate the XML config file
            Settings settings = new Settings();

            settings.InfluxSetting = GetExampleDataInfluxSettings();
            settings.DatabaseSettings = GetExampleDatabaseSettings();

            return settings;
        }

        private static QueryItem GetQuery(int id, string query, string measurement)
        {
            QueryItem queryItem = new QueryItem();
            queryItem.Id = id;
            queryItem.Query = query;
            queryItem.Measurement = measurement;

            return queryItem;
        }

        private static Settings DeserializerConfigFile()
        {
            Settings settings;
            XmlSerializer deserializer = new XmlSerializer(typeof(Settings));
            using (TextReader reader = new StreamReader(configFileName))
            {
                object obj = deserializer.Deserialize(reader);
                settings = (Settings)obj;
            }
            return settings;
        }

        private static InfluxDBSettings GetExampleDataInfluxSettings()
        {
            //Creating InfluxDBSettings
            InfluxDBSettings influxSettings = new InfluxDBSettings();
            influxSettings.Databasename = "InFluxDatabaseName";
            influxSettings.Url = "http://influx.domain.com.br";
            influxSettings.UserName = "InfluxUser";
            influxSettings.Password = "SecretPassword";
            //Addind InfluxDbTags
            List<Tag> tags = new List<Tag>();
            tags.Add(GetTag("service_type", "sql_server"));
            tags.Add(GetTag("version", "2008_r2"));
            influxSettings.Tags = tags;

            return influxSettings;
        }

        private static Tag GetTag(string name, string value)
        {
            Tag tag = new Tag();
            tag.TagName = name;
            tag.TagValue = value;

            return tag;
        }
    }
}
