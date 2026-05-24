## Install Package
Microsoft.EntityFrameworkCore                10.0.8
Microsoft.EntityFrameworkCore.Design         10.0.8
Microsoft.EntityFrameworkCore.Tools          10.0.8
Npgsql.EntityFrameworkCore.PostgreSQL       10.0.1


## Creating Migration
dotnet ef migrations add InitialFitnessSchema
dotnet ef database update

# to update 
dotnet ef migrations add AddDateCreated

dotnet add package Dapper

dotnet add package Npgsql

dotnet ef migrations add AddAuthenticationTables

dotnet add package CloudinaryDotNet


# Loging
dotnet add package Serilog.AspNetCorecle
dotnet add package Serilog.Sinks.Console