namespace Dometrain.EFCore.API.Models;

public record Genre
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    // EF will make a collection navigation property
    public ICollection<Movie> Movies { get; set; } = new HashSet<Movie>();
}