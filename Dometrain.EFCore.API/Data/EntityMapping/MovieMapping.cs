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
            .ToTable("Films") // change the table name
            // can add a GLOBAL query filter. The Db can store movies before 1980, but we don't EVER want to show them
            .HasQueryFilter(movie => movie.ReleaseDate >= new DateTime(1980, 1, 1))
            .HasKey(movie => movie.Id);

        builder.Property(movie => movie.Title)
            .HasMaxLength(128)
            .HasColumnType("varchar")
            .IsRequired();

        builder.Property(movie => movie.ReleaseDate)
            // .HasColumnType("date")
            .HasColumnType("char(8)")
            .HasConversion(new DateTimeToChar8Converter()); // only store 8 chars

        builder.Property(movie => movie.Synopsis)
            .HasColumnName("Plot")
            .HasColumnType("varchar(max)");

        builder.Property(mov => mov.AgeRating)
            .HasColumnType("varchar(32)")
            // instead of saving the int enum string value
            // DANGER this will impact queries. Instead of comparing ints, it will compare string values
            .HasConversion<string>();

        // Fluent API for the relationship
        // for when we do not use the default naming convention
        builder
            .HasOne(movie => movie.Genre)
            .WithMany(genre => genre.Movies) // define both directional navigational properties
            .HasPrincipalKey(genre => genre.Id) // point to the Pk of the Genre
            .HasForeignKey(movie => movie.MainGenreId); // point to the Fk in the Movie table

        // this will put a column on the Movie table called 'Director_FirstName' and `Director_LastName`
        // builder.ComplexProperty(mov => mov.Director);

        // this will put a column on the Movie table called 'DirectorFirstName' and `DirectorLastName`
        // but we can move this out to a separate table with .ToTable  
        builder.OwnsOne(mov => mov.Director)
            .ToTable("Movie_Directors");
        // new Table: Movie_Directors with 3 columns:
        // - Id: PK & FK to Movie table
        // - FirstName
        // - LastName

        builder.OwnsMany(mov => mov.Actors)
            .ToTable("Movie_Actors"); // this is a 1:many relationship with composite primary key
        // new Table: Movie_Actors with 4 columns:
        // - MovieId: PK & FK to Movie table
        // - Id: PK part 2 (composite)
        // - FirstName
        // - LastName


        // SeedData(builder);
    }

    private void SeedData(EntityTypeBuilder<Movie> builder)
    {
        var movies = new List<Movie>
        {
            new()
            {
                Id = 1,
                Title = "The Matrix",
                ReleaseDate = new DateTime(1999, 3, 31),
                Synopsis =
                    "A computer hacker learns from mysterious rebels about the true nature of his reality and his role in the war against its controllers.",
                MainGenreId = 1, // need to seed this as well
                AgeRating = AgeRating.Teen,
            },
            // We have a global filter on ReleaseDate, so this will not be shown but will be in the Db
            new()
            {
                Id = 2,
                Title = "Gone with the Wind",
                ReleaseDate = new DateTime(1939, 12, 15),
                Synopsis = "A sheltered and manipulative Southern belle and a roguish profiteer face off in a turbulent romance as the society around them crumbles with the end of slavery and is rebuilt during the Civil War and Reconstruction periods.",
                MainGenreId = 2, // need to seed this as well
                AgeRating = AgeRating.All,
            }
        };
        
        
        
        // seed some movie data
        builder.HasData(movies);

        // seed owned director data
        builder.OwnsOne(mov => mov.Director)
            .HasData(new
            {
                MovieId = 1, // anon type, we know the Id aka FK is 1
                FirstName = "Lana",
                LastName = "Wachowski",
            });


        // seed owned actor(s) data
        builder.OwnsMany(mov => mov.Actors)
            .HasData(
                new { MovieId = 1, Id = 1, FirstName = "Keanu", LastName = "Reeves", },
                new { MovieId = 1, Id = 2, FirstName = "Laurence", LastName = "Fishburne", }
            );
    }
}