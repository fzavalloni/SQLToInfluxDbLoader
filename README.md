# SQLToInfluxDbLoader

Executable which run a SQL Server Query and post its results to a InfluxDb Server

It was built based in an spefific need I got, I had some reports in SQL Server reporting services and I wanted to
put in InfluxDb + Grafana.

# Configuration File
```
<?xml version="1.0" encoding="utf-8"?>
<Settings xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <InfluxSetting>
    <Databasename>InFluxDatabaseName</Databasename>
    <Url>http://influx.domain.com.br</Url>
    <UserName>InfluxUser</UserName>
    <Password>SecretPassword</Password>
    <Tags>
      <Tag>
        <TagName>service_type</TagName>
        <TagValue>sql_server</TagValue>
      </Tag>
      <Tag>
        <TagName>version</TagName>
        <TagValue>2008_r2</TagValue>
      </Tag>
    </Tags>
  </InfluxSetting>
  <DatabaseSettings>
    <ProviderList>
      <Provider>
        <ConnectionString>Data Source=Server01; Initial Catalog=DBName;Integrated Security=SSPI;</ConnectionString>
        <Queries>
          <QueryItem id="1" query="EXEC sp_GetStatistics" measurement="Report2017" />
          <QueryItem id="2" query="EXEC sp_GetStorageSpace" measurement="StorageReport2017" />
        </Queries>
      </Provider>
    </ProviderList>
  </DatabaseSettings>
</Settings>
```

# Parameters

/TestSQLConnection  -  Test the SQL credentials

/TestInfluxDBConnection  -  Test the InfluxDB credentials

/Run  -  Post the SQL Data to InfluxDB"
