﻿using Dometrain.EFCore.API.Data.EntityMapping;
using Dometrain.EFCore.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Dometrain.EFCore.API.Data;

public class MoviesContext : DbContext
{
    // OK way to do this
    // public DbSet<Movie> Movies { get; set; } = null!;

    // Better way to do this with get only property
    public DbSet<Movie> Movies => Set<Movie>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("""
                                    Server=localhost;
                                    Database=Movies;
                                    User Id=sa;
                                    Password=BenjiPwd123$;
                                    TrustServerCertificate=True;
                                    """);
        // ghetto logging
        optionsBuilder.LogTo(Console.WriteLine);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(MoviesContext).Assembly);

        modelBuilder.ApplyConfiguration(new MovieMapping());
        
        
        base.OnModelCreating(modelBuilder);
    }
}