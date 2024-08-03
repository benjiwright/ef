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

