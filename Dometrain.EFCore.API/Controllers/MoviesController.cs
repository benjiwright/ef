using Dometrain.EFCore.API.Data;
using Dometrain.EFCore.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dometrain.EFCore.API.Controllers;

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
        var movies = await _context.Movies.ToListAsync();
        return Ok(movies);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Movie), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        // return first match
        // var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);
        
        // expect single match or Exception
        // var movie = await _context.Movies.SingleAsync(m => m.Id == id);
        
        // serves from memory, else makes trip to DB. Faster, but could be stale
        var movie = await _context.Movies.FindAsync(id);
        
        return movie == null ? NotFound() : Ok(movie);
    }


    [HttpGet("age-limited-movies/{ageRating}")]
    [ProducesResponseType(typeof(MovieTitle), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAgeRating([FromRoute] AgeRating ageRating)
    {
        var movies = await _context.Movies
            // Danger: this will do string wise compare, not the enum if we save AgeRating as a string in DB
            // by using a ValueConverter
            .Where(movie => movie.AgeRating <= ageRating) 
            .Select(movie => new MovieTitle( movie.Id, movie.Title ?? string.Empty ))
            .ToListAsync();
    
        return movies.Count == 0 ? NotFound() : Ok(movies);
    
    }

    [HttpPost]
    [ProducesResponseType(typeof(Movie), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] Movie movie)
    {
        await _context.Movies.AddAsync(movie);
        // before: has no Id
        await _context.SaveChangesAsync();
        // after: has Id
        return CreatedAtAction(nameof(Get), new { id = movie.Id }, movie);
    }
    
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Movie), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] Movie movie)
    {
        var existingMovie = await _context.Movies.FindAsync(id);
        if (existingMovie == null)
        {
            return NotFound();
        }
        
        existingMovie.Title = movie.Title;
        existingMovie.ReleaseDate = movie.ReleaseDate;
        existingMovie.Synopsis = movie.Synopsis;

        // EF recognizes the change to the entity and marks it as modified via change tracking
        await _context.SaveChangesAsync();
        
        return Ok(existingMovie);
    }
    
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remove([FromRoute] int id)
    {
        var movieToDelete = await _context.Movies.FindAsync(id);
        if (movieToDelete == null)
        {
            return NotFound();
        }
        
        _context.Movies.Remove(movieToDelete);
        // _context.Remove(movieToDelete); // shorthand, b/c context knows it's a DbSet<Movie>
        await _context.SaveChangesAsync();

        return Ok();
    }
    
    [HttpGet("by-year/{year:int}")]
    [ProducesResponseType(typeof(List<Movie>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllByYear([FromRoute] int year)
    {
        // take notice of type
        IQueryable<Movie> allMovies = _context.Movies;

        // `where` is not executed here, but when `ToListAsync` is called
        IQueryable<Movie> filteredMovies = allMovies.Where(m => m.ReleaseDate.Year == year);
       
        // deferred execution. DB is executed here
        return Ok(await filteredMovies.ToListAsync());
    }
   
    [HttpGet("by-year-query-syntax/{year:int}")]
    [ProducesResponseType(typeof(List<Movie>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllByYearQuerySyntax([FromRoute] int year)
    {
        IQueryable<Movie> filteredMovies =
            from movie in _context.Movies
            where movie.ReleaseDate.Year == year
            select movie;
       
        // deferred execution. DB is executed here
        return Ok(await filteredMovies.ToListAsync());
    }
   
    [HttpGet("by-year-projections/{year:int}")]
    [ProducesResponseType(typeof(List<Movie>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllByYearUsingProjections([FromRoute] int year)
    {
       
        var filteredMovies = await _context.Movies
            // query on DbSet
            .Where(m => m.ReleaseDate.Year == year)
            // project to new type for performance. We don't want "select *"
            .Select(movie => new MovieTitle(movie.Id, movie.Title ?? "No Title"))
            // execute query
            .ToListAsync();
       
        return Ok(filteredMovies);
    }
}