# hardware-leasing
Service for hardware leasing

## Description
* Server: Python3 REST application using Flask (http://flask.pocoo.org/)
* DB: SQLite (https://www.sqlite.org/)
* Client: C# application in DotNetCore. (https://dotnet.microsoft.com/)
* RestSharp for REST communication. (https://www.nuget.org/packages/RestSharp/)

Backend service and client application are in separate docker containers.

## Instructions
1. Server side: 
   * execute PleaseServer/run.sh
2. Client side:
   * execute CleaseClient/run.sh

