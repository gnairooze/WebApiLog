# WebApiLog
Log Request and Response of Web API

## Concepts

The log is build by creating a message handler that extends the DelegatingHandler. Thanks to the article [How to log request and response metadata in ASP.Net Web API](https://www.infoworld.com/article/3211590/application-development/how-to-log-request-and-response-metadata-in-aspnet-web-api.html "How to log request and response metadata in ASP.Net Web API")

## How to Use it

1. Add reference to WebLog project or its compiled DLL 
2. In file App_Start/WebApiConfig.cs, in the Register mthod, add the following line:

```csharp
config.MessageHandlers.Add(new WebLog.LogHandler());
```

3. In the file Web.config, add connection string to the log DB. The connection string must be named WebLogDB
4. Use the SQL/01- WebLog.sql SQL script to create the necessary tables and a stored procedure.
5. Use the following SQL query to read the log:

```SQL
select
	*
from WebLog

select
	*
from WebLog_Query
```
