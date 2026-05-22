## Install Package
Microsoft.EntityFrameworkCore                10.0.8
Microsoft.EntityFrameworkCore.Design         10.0.8
Microsoft.EntityFrameworkCore.Tools          10.0.8
Npgsql.EntityFrameworkCore.PostgreSQL       10.0.1

## Creating Migration
dotnet ef migrations add InitialFitnessSchema
dotnet ef database update

dotnet add package Dapper

dotnet add package Npgsql
