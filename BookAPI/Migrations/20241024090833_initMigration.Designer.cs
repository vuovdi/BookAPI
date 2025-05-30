﻿// <auto-generated />
using BookAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BookAPI.Migrations
{
    [DbContext(typeof(BookDbContext))]
    [Migration("20241024090833_initMigration")]
    partial class initMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.35");

            modelBuilder.Entity("BookAPI.Models.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FirstName")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Author");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            FirstName = "Bill",
                            LastName = "Hopper"
                        },
                        new
                        {
                            Id = 2,
                            FirstName = "Trude",
                            LastName = "Moe"
                        },
                        new
                        {
                            Id = 3,
                            FirstName = "Ada",
                            LastName = "Love"
                        });
                });

            modelBuilder.Entity("BookAPI.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CategoryName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Category");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CategoryName = "Horror"
                        },
                        new
                        {
                            Id = 2,
                            CategoryName = "Fantasy"
                        },
                        new
                        {
                            Id = 3,
                            CategoryName = "Crime"
                        },
                        new
                        {
                            Id = 4,
                            CategoryName = "Romance"
                        },
                        new
                        {
                            Id = 5,
                            CategoryName = "Historical"
                        });
                });

            modelBuilder.Entity("BookAPI.Models.Entities.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AuthorId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Year")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Books");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AuthorId = 3,
                            CategoryId = 1,
                            Description = "Set 50 years after the collapse of the internet",
                            Title = "The Webpocalypse",
                            Year = 2024
                        },
                        new
                        {
                            Id = 2,
                            AuthorId = 2,
                            CategoryId = 2,
                            Description = "A mystical forest village rebels against the mountain village",
                            Title = "The Fabled Forest",
                            Year = 2023
                        },
                        new
                        {
                            Id = 3,
                            AuthorId = 2,
                            CategoryId = 2,
                            Description = "A fae is found murdered in a forest village. Can detective Fungus solve the crime?",
                            Title = "The forest murders",
                            Year = 2024
                        },
                        new
                        {
                            Id = 4,
                            AuthorId = 1,
                            CategoryId = 5,
                            Description = "We look into the rich histories of 50 castles in Scotland",
                            Title = "50 Fabulous Castles",
                            Year = 2022
                        },
                        new
                        {
                            Id = 5,
                            AuthorId = 1,
                            CategoryId = 4,
                            Description = "A true love story",
                            Title = "Ben & Jerries",
                            Year = 2020
                        },
                        new
                        {
                            Id = 6,
                            AuthorId = 3,
                            CategoryId = 3,
                            Description = "Detective Forkster has a hard time solving the case of a missing steak.",
                            Title = "A cook, a fork and a kitchen",
                            Year = 2023
                        });
                });

            modelBuilder.Entity("BookAPI.Models.Entities.Book", b =>
                {
                    b.HasOne("BookAPI.Models.Author", "Author")
                        .WithMany("Books")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookAPI.Models.Category", "Category")
                        .WithMany("Books")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("BookAPI.Models.Author", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("BookAPI.Models.Category", b =>
                {
                    b.Navigation("Books");
                });
#pragma warning restore 612, 618
        }
    }
}
