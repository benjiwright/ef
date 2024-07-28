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
}