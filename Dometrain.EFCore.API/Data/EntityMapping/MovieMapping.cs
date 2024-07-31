using Dometrain.EFCore.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dometrain.EFCore.API.Data.EntityMapping;

public class MovieMapping : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder
            .ToTable("MoviesDb")
            .HasKey(movie => movie.Id);
        
        builder.Property(movie => movie.Title)
            .HasMaxLength(128)
            .HasColumnType("varchar")
            .IsRequired();
        
        builder.Property(movie => movie.ReleaseDate)
            .HasColumnType("date");
        
        builder.Property(movie => movie.Synopsis)
            .HasColumnName("Plot")
            .HasColumnType("varchar(max)");

        builder.HasOne(movie => movie.Genre)
            .WithMany(genre => genre.Movies) // define both directional navigational properties
            .HasPrincipalKey(genre => genre.Id) // point to the Pk of the Genre
            .HasForeignKey(movie => movie.MainGenreId); // point to the Fk in the Movie table
    }
}