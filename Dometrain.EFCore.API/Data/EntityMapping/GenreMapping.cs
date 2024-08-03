using Dometrain.EFCore.API.Data.ValueGenerators;
using Dometrain.EFCore.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dometrain.EFCore.API.Data.EntityMapping;

public class GenreMapping : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        // CreateTimestampGenerator(builder);

        
        // this is shadow property and hidden from the Genre class, but will be stored in the Db
        builder.Property<DateTime>("CreatedDate")
            .HasColumnName("CreatedAt")
            .HasValueGenerator<CreatedDateGenerator>();

        // SeedData(builder);
    }

    private static void SeedData(EntityTypeBuilder<Genre> builder)
    {
        var genres = new List<Genre>
        {
            new() { Id = 1, Name = "Sci-Fi" },
            new() { Id = 2, Name = "Drama" },
            new() { Id = 3, Name = "Action" },
        };

        builder.HasData(genres);
    }


    // on insert, the Db will generate the value for "CreatedAt"
    private static void CreateTimestampGenerator(EntityTypeBuilder<Genre> builder)
    {
        // builder.Property(genre => genre.CreateDate)
        //     .HasColumnName("CreatedAt")
            
            // Option 1: execute the GETDATE() function in SQL Server when inserting a new record
            // Thus, the Db will set the value
            //.HasDefaultValueSql("GETDATE()")

            // Option 2: to generate the value within the application
            // keep in mind that this will only work when inserting a new record and not when seeding
            // data in our GenreMapping.cs => builder.HasData(new Genre ...);
            // .HasValueGenerator<CreatedDateGenerator>();
    }
}