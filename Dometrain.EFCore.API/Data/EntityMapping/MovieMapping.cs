using Dometrain.EFCore.API.Data.ValueConverters;
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
            .HasColumnType("date")
            .HasConversion(new DateTimeToChar8Converter()); // only store 8 chars
        
        builder.Property(movie => movie.Synopsis)
            .HasColumnName("Plot")
            .HasColumnType("varchar(max)");

        // builder.Property(mov => mov.AgeRating)
        //     .HasColumnType("varchar(32)");
            // instead of saving the int enum value.. but DANGER this will impact queries
            //.HasConversion<string>(); 

        // Fluent API for the relationship
        // for when we do not use the default naming convention
        builder
            .HasOne(movie => movie.Genre)
            .WithMany(genre => genre.Movies) // define both directional navigational properties
            .HasPrincipalKey(genre => genre.Id) // point to the Pk of the Genre
            .HasForeignKey(movie => movie.MainGenreId); // point to the Fk in the Movie table
        
        // seed some data
        builder.HasData(new Movie
        {
            Id = 1,
            Title = "The Matrix",
            ReleaseDate = new DateTime(1999, 3, 31),
            Synopsis =
                "A computer hacker learns from mysterious rebels about the true nature of his reality and his role in the war against its controllers.",
            MainGenreId = 1, // need to seed this as well
            // AgeRating = AgeRating.Teen,
        });
    }
}