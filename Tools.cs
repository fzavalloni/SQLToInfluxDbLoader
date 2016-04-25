using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

using InfluxDB;
using InfluxDB.Net;
using InfluxDB.Net.Infrastructure;
using InfluxDB.Net.Infrastructure.Influx;
using InfluxDB.Net.Models;
using InfluxDB.Net.Contracts;

using System.Data;
using System.Xml.Linq;

namespace SQLtoInfluxDBLoader
{
    public class Tools
    {
        private const string fileSettings = @".\Settings.xml";

        public static string ReadXmlAttribute(string key)
        {
            try
            {
                using (XmlTextReader reader = new XmlTextReader(fileSettings))
                {
                    while (reader.Read())
                    {
                        if (reader.Name == key)
                        {
                            return reader.ReadElementString();
                        }
                    }
                    return "not found";
                }
            }
            catch (Exception erro)
            {
                throw new Exception("Failed reading XML: " + erro.Message);
            }
        }

        public static List<QueryInfo> ReadXmlCollectionAttribute()
        {
            string query = string.Empty;
            string measurement = string.Empty;
            List<QueryInfo> list = new List<QueryInfo>();

            try
            {
                XDocument doc = XDocument.Load(fileSettings);
                foreach (var node in doc.Root.Descendants("Query"))
                {
                    query = node.Attribute("Query").Value;
                    measurement = node.Attribute("Measurement").Value;

                    list.Add(new QueryInfo(query, measurement));
                }

                return list;
            }
            catch (Exception erro)
            {
                throw new Exception("Failed reading XML: " + erro.Message);
            }
        }        
    }
}
