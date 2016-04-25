using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfluxDB.Net;
using InfluxDB.Net.Models;
using InfluxDB.Net.Contracts;
using InfluxDB.Net.Enums;
using System.Data;

namespace SQLtoInfluxDBLoader
{
    public class InfluxClient
    {
        IInfluxDb client = null; 
        public InfluxClient(string url,string userName,string password)
        {
            client = new InfluxDb(url, userName, password, InfluxDB.Net.Enums.InfluxVersion.v09x);            
        }

        public void WriteData(Point[] pointArray,QueryInfo query)
        {
            foreach (Point p in pointArray)
            {
                var writeResponse = client.WriteAsync(InfluxDBData.DatabaseName, p);                

                if (!writeResponse.Result.Success)
                {
                    throw new Exception(writeResponse.Result.ToString());                    
                }                
            }
        }

        public async Task<Pong> PingAsync()
        {
            return await client.PingAsync();
        }


        public static Point[] GetPointData(DataTable dt, string serviceType, string measurement)
        {
            Point[] pointCollection;
            List<Point> listPoint = new List<Point>();

            foreach (DataRow row in dt.Rows)
            {
                Point point = new Point();
                point = GetPoint(GetColumns(dt), row, measurement, serviceType);
                listPoint.Add(point);
            }

            pointCollection = listPoint.ToArray();

            return pointCollection;
        }

        private static Point GetPoint(List<string> columns, DataRow row, string measurement, string serviceType)
        {
            Point point = new Point();
            //point.Timestamp = DateTime.UtcNow;
            //point.Precision = InfluxDB.Net.Enums.TimeUnit.Milliseconds;
            point.Tags.Add("service_type", serviceType);
            point.Measurement = measurement;

            foreach (string col in columns)
            {
                point.Fields.Add(col, GetRowData(row, col));
            }

            return point;
        }

        private static List<string> GetColumns(DataTable dt)
        {
            List<string> list = new List<string>();
            list.AddRange(dt.Columns.Cast<DataColumn>().Select(i => i.ColumnName).ToList());

            return list;
        }

        private static object GetRowData(DataRow row, string column)
        {
            return row[column].ToString();
        }
    }
}
