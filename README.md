# Environ Manager - Server

## Introduction
I wanted to create a simple, networked system that I could customize as needed 
to monitor environmental factors for enclosed environments (terrariums, grow 
chambers, etc.) It should be able to save info to a server for further analysis.
Secondarily, it should be able to react to changes in the environment and correct 
them.

This is meant to be used in a client/server system with remote sensors. See:
- https://github.com/LinkedMink/environ-manager-hw-rpi
- https://github.com/LinkedMink/environ-manager-server
- https://github.com/LinkedMink/environ-manager-client

Disclaimer: This project was mainly intended as a learning aid and for private use.

## Getting Started

### Requirements
The API server runs as a .NET Core application in a client/server model. You will need 
to setup some associated software to run the application.

- .NET Core SDK 2.2
- PostgreSQL or SQL Server

### Optional
- Visual Studio 2017 or 2019
- Docker Host

## Configuration

### Install Packages
If you have Visual Studio, the required assemblies will be automatically restored. You can
run the command manually in each of the projects:

```sh
dotnet restore
```

### Edit appsettings.{environment}.json
Settings are configured on a per environment basis in both the API and command line tools. 
appsettings.json has the available settings where an environment specific file will override
the value in appsettings.json. This environment is determined by the environmental variable
ASPNETCORE_ENVIRONMENT.

For development, copy the appsettings.json.

```sh
# If not running Visual Studio you may need to set the environment
export ASPNETCORE_ENVIRONMENT=Development		# Unix Like
set ASPNETCORE_ENVIRONMENT=Development			# Windows CMD
$env:ASPNETCORE_ENVIRONMENT = 'Development'		# Powershell

cd ./LinkedMink.Processor.EnvironManager.Tools
cp ./appsettings.json ./appsettings.Development.json
cd ../LinkedMink.Web.Api.EnvironManager
cp ./appsettings.json ./appsettings.Development.json
```

Remove any settings you don't plan to override. Two settings other than database settings
should be looked at.

- Authentication.SigningKey: This can be any string since only the translated bytes are used 
to create the key, but it should be kept secret.
- AllowedHosts: "*" for development should be fine, but at deployment this should be set to
an explicit client host.

Email is currently not required.

### Database Setup
Create a user and database in either PostgreSQL or SQL Server. 

The database to use is determined by an environmental variable ASPNETCORE_CONNECT_STRING_KEY. 
If no value is set for ASPNETCORE_CONNECT_STRING_KEY, DefaultConnection will be used for the 
connection string and SQL Server will be the DB engine. Optionally set this variable:

```sh
export ASPNETCORE_CONNECT_STRING_KEY=PostgreSql
```

Edit the desired ConnectionStrings in both appsettings.Development.json files. Build the projects.
Run the database migration scripts to create the database.

```sh
# Manually build if needed
cd ./LinkedMink.Web.Api.EnvironManager
dotnet build

cd ./LinkedMink.Data.Domain.EnvironManager

# initialize the SQL Server database
dotnet ef database update `
    -p "../LinkedMink.Data.Domain.EnvironManager.SqlServer/LinkedMink.Data.Domain.EnvironManager.SqlServer.csproj" `
	-s "../LinkedMink.Web.Api.EnvironManager/LinkedMink.Web.Api.EnvironManager.csproj" `
	-c "SqlServerDbContext"

# or initialize a PostgreSQL database
dotnet ef database update `
    -p "../LinkedMink.Data.Domain.EnvironManager.PostgreSql/LinkedMink.Data.Domain.EnvironManager.PostgreSql.csproj" `
    -s "../LinkedMink.Web.Api.EnvironManager/LinkedMink.Web.Api.EnvironManager.csproj" `
    -c "PostgreSqlDbContext"
```

#### Seed Data
To populate an initial set of users and hardware devices, there is a command to seed data.
Edit the seed.json file in LinkedMink.Processor.EnvironManager.Tools. The device host and port
must match that of the logger/control hardware since it is used to key log entries. See:

https://github.com/LinkedMink/environ-manager-hw-rpi

Build the project LinkedMink.Processor.EnvironManager.Tools. Then execute the seed script.

```sh
dotnet LinkedMink.Processsor.EnvironManager.Tools.dll seed
```

### Run Software
Build the software using the .NET Core SDK via the command line or use Visual Studio.

```sh
dotnet LinkedMink.Web.Api.EnvironManager.dll
```

## Deployment - Docker
Create a separate appsettings.json for your target environment then edit it as necessary.
Build the docker image in the root of the solution.

```sh
docker build \
	-f ./LinkedMink.Web.Api.EnvironManager/Dockerfile \
	-t linkedmink/environ-manager-server .
```

Run the containers on the target machine. Set the needed variables as necessary.

```sh
docker run -d \
  -p 8083:80 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e ASPNETCORE_CONNECT_STRING_KEY=SqlServer \
  --name environ-manager-server \
  linkedmink/environ-manager-server
```
