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

# Alter the Movie model to include a new property
public int ImdbRating { get; set; }

# new migration
dotnet-ef migrations add AddImdbRatingToMovie

# let's say we want to change that property to a float AND rename it
public decimal InternetRating { get; set; }

dotnet-ef migrations add ChangeImdbRatingToInternetRating

# we will need to manually update the migration script to change the type and rename the column
# because EF doesn't know how to do that and will DROP old column and ADD new column instead of ALTER
```

```csharp

public partial class ChangeImdbRatingToInternetRating : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<int>(
            name: "ImdbRating",
            table: "Films",
            type: "decimal(18,2)",
            nullable: false,
            defaultValue: 0m);
        
        migrationBuilder.RenameColumn(
            name: "ImdbRating",
            table: "Films",
            newName: "InternetRating");

        // BAD AUTO GENERATED CODE
        // migrationBuilder.DropColumn(
        //     name: "ImdbRating",
        //     table: "Films");
        //
        // migrationBuilder.AddColumn<decimal>(
        //     name: "InternetRating",
        //     table: "Films",
        //     type: "decimal(18,2)",
        //     nullable: false,
        //     defaultValue: 0m);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // reverse the changes
        migrationBuilder.AlterColumn<int>(
            name: "ImdbRating",
            table: "Films",
            type: "int",
            nullable: false,
            defaultValue: 0m);
        
        migrationBuilder.RenameColumn(
            name: "InternetRating",
            table: "Films",
            newName: "ImdbRating");
    }
}

```sh
# apply the migration
dotnet-ef database update
```

```sh
# apply a specific migration aka rollback
dotnet-ef database update AddImdbRatingToMovie  
```

#### Execute SQL Migration Script - Two ways

```sh
# Option 1: Everything
dotnet-ef migrations script > migration.sql

# Option 2: Specific migration range from x to the end
dotnet-ef migrations script ImdbRatingToMovie > partial_migration.sql
```

```sh
# create efbundle.exe migration file
dotnet-ef migrations bundle
```