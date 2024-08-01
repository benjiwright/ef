namespace Dometrain.EFCore.API.Models;

public class Movie
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string? Synopsis { get; set; }
    
    // EF will make Fk relationship
    public Genre Genre { get; set; }
    
    // EF will make explicit FK property in the database for us
    // public int GenreId { get; set; }
    
    // but what if the Fk is named differently like 'MainGenreId'?
    // need to make this relationship in MovieMapping
    public int MainGenreId { get; set; }

    // public AgeRating AgeRating { get; set; }
    
}

public record MovieTitle(int Id, string Title);

public enum AgeRating
{
    All = 0,
    Kids = 6,
    Tween = 10,
    Teen =13,
    YoungAdult= 17,
    Adult =18,
}