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


```xml
<!-- add the package to the project -->
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.7" />
```

```csharp
// create the context
public class MoviesContext : DbContext
{
    // OK way to do this
    public DbSet<Movie> Movies { get; set; } = null!;
    
    // Better way to do this with get only property
    public DbSet<Movie> Movies
    {
        get { return Set<Movie>(); }
    }
    
    // Shorthand for the above
    public DbSet<Movie> Movies => Set<Movie>();
}
```

```csharp
// controller uses context
[ApiController]
[Route("[controller]")]
public class MoviesController : Controller
{
    private readonly MoviesContext _context;

    public MoviesController(MoviesContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(List<Movie>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _context.Movies.ToListAsync();
        return Ok(result);
    }
}    
```

