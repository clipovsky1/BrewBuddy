﻿// <auto-generated />
using System;
using BrewBuddy.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BrewBuddy.Migrations
{
    [DbContext(typeof(BrewBuddyDbContext))]
    partial class BrewBuddyDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BrewBuddy.Models.Beer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("BreweryId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nchar(100)")
                        .IsFixedLength();

                    b.Property<int?>("StyleId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__Beer__3214EC07334BBD2E");

                    b.HasIndex("BreweryId");

                    b.HasIndex("StyleId");

                    b.ToTable("Beers");
                });

            modelBuilder.Entity("BrewBuddy.Models.BeerStyle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("nchar(12)")
                        .IsFixedLength();

                    b.HasKey("Id")
                        .HasName("PK__BeerStyl__3214EC07B8484D5D");

                    b.ToTable("BeerStyles");
                });

            modelBuilder.Entity("BrewBuddy.Models.Brewery", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nchar(25)")
                        .IsFixedLength();

                    b.HasKey("Id")
                        .HasName("PK__Brewery__3214EC07B6A8E5A1");

                    b.ToTable("Breweries");
                });

            modelBuilder.Entity("BrewBuddy.Models.Beer", b =>
                {
                    b.HasOne("BrewBuddy.Models.Brewery", "Brewery")
                        .WithMany("Beers")
                        .HasForeignKey("BreweryId")
                        .HasConstraintName("FK_Beer_ToBrewery");

                    b.HasOne("BrewBuddy.Models.BeerStyle", "Style")
                        .WithMany("Beers")
                        .HasForeignKey("StyleId")
                        .HasConstraintName("FK_Beer_ToBeerStyle");

                    b.Navigation("Brewery");

                    b.Navigation("Style");
                });

            modelBuilder.Entity("BrewBuddy.Models.BeerStyle", b =>
                {
                    b.Navigation("Beers");
                });

            modelBuilder.Entity("BrewBuddy.Models.Brewery", b =>
                {
                    b.Navigation("Beers");
                });
#pragma warning restore 612, 618
        }
    }
}