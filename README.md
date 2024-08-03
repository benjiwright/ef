# Entity Framework - Get Good

dometrain.com

## Setup Local

```sh
# run MS SQL in docker
# Note: mapping externally accessible port to 1499 because of conflict with local SQL Server
docker run --restart unless-stopped \
  -e "ACCEPT_EULA=Y" \
  -e "MSSQL_SA_PASSWORD=BenjiPwd123$" \
  -p 1499:1433 \
  -d mcr.microsoft.com/mssql/server:2022-latest
```

```sql
-- create the DB in the container
use master;
CREATE DATABASE [MoviesDB];
GO
```

## Migrations

### snapshot tooling (automation)

Take a snapshot of two databases and compare them to generate a migration script.
E.g.: 

- Production DB (A)
- Development DB (B)

Find delta between A and B and generate a migration script to apply to A.

### delta based migrations (manual, small, incremental changes)

Every time you make a change to the model, generate a migration script to apply to the DB. 
They can be bi-directional. This is like git commits for DB


```sh
# cd to the project
cd Dometrain.EFCore.API/

# add package to project
dotnet add package Microsoft.EntityFrameworkCore.Tools

# install the tools globally
dotnet tool install -g dotnet-ef    

# add to PATH so we can use
export PATH="$PATH:$HOME/.dotnet/tools"

# add first migration
dotnet-ef migrations add InitialSchema    

```