﻿// <auto-generated />
using System;
using CitiInfo.WebAPI.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CitiInfo.WebAPI.Migrations
{
    [DbContext(typeof(CityInfoContext))]
    partial class CityInfoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.25");

            modelBuilder.Entity("CitiInfo.WebAPI.Entities.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Cities");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "The one with that big park.",
                            Name = "New York City"
                        },
                        new
                        {
                            Id = 2,
                            Description = "Large city of Viet nam",
                            Name = "Ho chi minh"
                        },
                        new
                        {
                            Id = 3,
                            Description = "The one with that big tower",
                            Name = "Paris"
                        });
                });

            modelBuilder.Entity("CitiInfo.WebAPI.Entities.PointOfInterest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CityId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("PointsOfInterest");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CityId = 1,
                            Description = "The most visited park in US",
                            Name = "Central Park"
                        },
                        new
                        {
                            Id = 2,
                            CityId = 2,
                            Description = "A skyscraper in US",
                            Name = "Empire State Building"
                        },
                        new
                        {
                            Id = 3,
                            CityId = 3,
                            Description = "US",
                            Name = "Cathedral"
                        });
                });

            modelBuilder.Entity("CitiInfo.WebAPI.Models.PointOfInterestDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CityId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.ToTable("PointOfInterestDto");
                });

            modelBuilder.Entity("CitiInfo.WebAPI.Entities.PointOfInterest", b =>
                {
                    b.HasOne("CitiInfo.WebAPI.Entities.City", "City")
                        .WithMany()
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");
                });

            modelBuilder.Entity("CitiInfo.WebAPI.Models.PointOfInterestDto", b =>
                {
                    b.HasOne("CitiInfo.WebAPI.Entities.City", null)
                        .WithMany("PointsOfInterest")
                        .HasForeignKey("CityId");
                });

            modelBuilder.Entity("CitiInfo.WebAPI.Entities.City", b =>
                {
                    b.Navigation("PointsOfInterest");
                });
#pragma warning restore 612, 618
        }
    }
}
