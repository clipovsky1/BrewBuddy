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
        => optionsBuilder.UseSqlServer("Data Source=LIPOVSKYC-PC;Initial Catalog=BrewBuddyDb;Integrated Security=True;TrustServerCertificate=true\n");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Beer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC074A3E7A34");

            entity.HasIndex(e => e.BreweryId, "IX_Beers_BreweryId");

            entity.HasIndex(e => e.StyleId, "IX_Beers_StyleId");

            entity.HasOne(d => d.Brewery).WithMany(p => p.Beers)
                .HasForeignKey(d => d.BreweryId)
                .HasConstraintName("FK_Beer_ToBrewery");

            entity.HasOne(d => d.Style).WithMany(p => p.Beers)
                .HasForeignKey(d => d.StyleId)
                .HasConstraintName("FK_Beer_ToBeerStyle");
        });

        modelBuilder.Entity<BeerStyle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC07AB057972");

            entity.Property(e => e.Description).HasColumnType("text");
        });

        modelBuilder.Entity<Brewery>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tmp_ms_x__3214EC07BECBCBAC");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
