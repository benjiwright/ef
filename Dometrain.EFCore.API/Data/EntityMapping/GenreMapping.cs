using Dometrain.EFCore.API.Data.ValueGenerators;
using Dometrain.EFCore.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Dometrain.EFCore.API.Data.EntityMapping;

public class GenreMapping : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.Property(genre => genre.CreateDate)
            // execute the GETDATE() function in SQL Server when inserting a new record
            // Thus, the Db will set the value
            //.HasDefaultValueSql("GETDATE()")

            // better: to generate the value within the application
            // keep in mind that this will only work when inserting a new record and not when seeding
            // data in our GenreMapping.cs => builder.HasData(new Genre ...);
            .HasValueGenerator<CreatedDateGenerator>();
            
        
        // seed data
        builder.HasData(new Genre
        {
            Id = 1,
            Name = "Sci-Fi",
        });
    }
}