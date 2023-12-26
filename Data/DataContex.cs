namespace Data;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Net.Security;
using System.Collections.Generic;
using System.Numerics;

public class DataContex : DbContext
{
    public DataContex()
    {
        this.Database.EnsureDeleted();
        this.Database.EnsureCreated();
    }

    public DbSet<Game> Games => this.Set<Game>();

    public DbSet<Studio> Studios => this.Set<Studio>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=games.sqlite;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>()
            .HasOne(g => g.GameStudio)
            .WithMany(s => s.Games)
            .HasForeignKey(g => g.StudioId);
    }
}