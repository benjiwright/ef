# Entity Framework - Get Good

dometrain.com

## Setup Local

```sh
# run MS SQL in docker
docker run --restart unless-stopped \
  -e "ACCEPT_EULA=Y" \
  -e "MSSQL_SA_PASSWORD=BenjiPwd123$" \
  -p 1433:1433 \
  -d mcr.microsoft.com/mssql/server:2022-latest
```

```sql
-- create the DB in the container
CREATE DATABASE [MoviesDB];
```

