using System.Text.Json.Serialization;

namespace Dometrain.EFCore.API.Models;

public record Genre
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    [JsonIgnore] public DateTime CreateDate { get; set; }
    
    
    // EF will make a collection navigation property
    public ICollection<Movie> Movies { get; set; } = new HashSet<Movie>();
}