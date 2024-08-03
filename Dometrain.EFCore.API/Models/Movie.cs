namespace Dometrain.EFCore.API.Models;

public class Movie
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string? Synopsis { get; set; }

    // EF will make Fk relationship
    public Genre Genre { get; set; }

    // EF will make a FK property in the Movie table for free!
    // public int GenreId { get; set; }
    // but what if the Fk is named differently like 'MainGenreId'?
    // need to make this relationship in MovieMapping
    public int MainGenreId { get; set; }

    public AgeRating AgeRating { get; set; }

    public Person Director { get; set; }

    public ICollection<Person> Actors { get; set; } = new List<Person>();
}