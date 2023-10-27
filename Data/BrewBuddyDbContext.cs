using System;
using System.Collections.Generic;
using BrewBuddy.Models;
using Microsoft.EntityFrameworkCore;

namespace BrewBuddy.Data;

public partial class BrewBuddyDbContext : DbContext
{
    public BrewBuddyDbContext()
    {
    }

    public BrewBuddyDbContext(DbContextOptions<BrewBuddyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Beer> Beers { get; set; }

    public virtual DbSet<BeerStyle> BeerStyles { get; set; }

    public virtual DbSet<Brewery> Breweries { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=LIPOVSKYC-PC;Initial Catalog=BrewBuddyDb;Integrated Security=True;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Beer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Beer__3214EC07334BBD2E");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsFixedLength();

            entity.HasOne(d => d.Brewery).WithMany(p => p.Beers)
                .HasForeignKey(d => d.BreweryId)
                .HasConstraintName("FK_Beer_ToBrewery");

            entity.HasOne(d => d.Style).WithMany(p => p.Beers)
                .HasForeignKey(d => d.StyleId)
                .HasConstraintName("FK_Beer_ToBeerStyle");
        });

        modelBuilder.Entity<BeerStyle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BeerStyl__3214EC07B8484D5D");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name)
                .HasMaxLength(12)
                .IsFixedLength();
        });

        modelBuilder.Entity<Brewery>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Brewery__3214EC07B6A8E5A1");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name)
                .HasMaxLength(25)
                .IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
