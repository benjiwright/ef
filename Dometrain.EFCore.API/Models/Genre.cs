using System.Text.Json.Serialization;

namespace Dometrain.EFCore.API.Models;

public record Genre
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    // EF will create a shadow property, so we will comment out
    // it is possible to use the CreatedDate property because it's a column on the table. E.g.:
    // DateTime createdAt = _context.Entry(genre).Property("CreatedDate").CurrentValue;
    // [JsonIgnore] public DateTime CreateDate { get; set; }
    
    // EF will make a collection navigation property
    [JsonIgnore]
    public ICollection<Movie> Movies { get; set; } = new HashSet<Movie>();
}