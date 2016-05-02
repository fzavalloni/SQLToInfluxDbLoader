# SQLToInfluxDbLoader

Executable which run a SQL Server Query and post the result to a InfluxDb Server

It was built based in an spefific need I got, I had some reports in SQL Server reporting services.
In my scenario, it runs daily and post the query information to InfluxDB.
``
# Configuration File
```
<?xml version="1.0" encoding="utf-8" ?>
<Settings>
  <InfluxDB>
    <databaseName>dbName</databaseName>
    <url>https://influxdb.domain.com.br</url>
    <userName>influxUserName</userName>
    <password>b6eae4bxxxxxxxxxxx65129779c</password>
    <service_type>sql_tag_product</service_type><!--Tag to make the filters in Grafana-->
  </InfluxDB>
  <SQLServer>
    <connectionString>Data Source=SQLServer001; Initial Catalog=DatabaseName;Integrated Security=SSPI;</connectionString>
    <!--Is possible insert more than one query-->
    <!--The parameter Measurement is the table name that will be created in InfluxDB-->
    <Queries>
      <Query id="1" Query="select * FROM Statistics where date = CONVERT(date,GETDATE())" Measurement="DAGReport2013"/>
      <Query id="2" Query="EXEC sp_GetStorageSpaces" Measurement="StorageReport2013"/>
      <Query id="3" Query="select * from FlowStatistics where Date = CONVERT(date,GETDATE()-1) group by Date" Measurement="MessageFlowReport2013"/>	  
    </Queries>
  </SQLServer>
</Settings>
```

# Parameters

/TestSQLConnection  -  Test the SQL credentials

/TestInfluxDBConnection  -  Test the InfluxDB credentials

/Run  -  Post the SQL Data to InfluxDB"
