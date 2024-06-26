﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using dotnetapp.Data;

#nullable disable

namespace dotnetapp.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240626065823_asdfg")]
    partial class asdfg
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("dotnetapp.Models.Booking", b =>
                {
                    b.Property<int>("BookingID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookingID"), 1L, 1);

                    b.Property<string>("ContactNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DurationInMinutes")
                        .HasColumnType("int");

                    b.Property<int>("VehicleID")
                        .HasColumnType("int");

                    b.HasKey("BookingID");

                    b.HasIndex("VehicleID");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("dotnetapp.Models.Vehicle", b =>
                {
                    b.Property<int>("VehicleID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("VehicleID"), 1L, 1);

                    b.Property<bool>("Availability")
                        .HasColumnType("bit");

                    b.Property<string>("Make")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("VehicleID");

                    b.ToTable("Vehicles");

                    b.HasData(
                        new
                        {
                            VehicleID = 1,
                            Availability = true,
                            Make = "Tesla",
                            Model = "Model S",
                            Year = 2024
                        },
                        new
                        {
                            VehicleID = 2,
                            Availability = true,
                            Make = "Ford",
                            Model = "Mustang",
                            Year = 2024
                        },
                        new
                        {
                            VehicleID = 3,
                            Availability = false,
                            Make = "Chevrolet",
                            Model = "Camaro",
                            Year = 2024
                        },
                        new
                        {
                            VehicleID = 4,
                            Availability = true,
                            Make = "BMW",
                            Model = "M3",
                            Year = 2022
                        },
                        new
                        {
                            VehicleID = 5,
                            Availability = true,
                            Make = "Audi",
                            Model = "A4",
                            Year = 2023
                        });
                });

            modelBuilder.Entity("dotnetapp.Models.Booking", b =>
                {
                    b.HasOne("dotnetapp.Models.Vehicle", "Vehicle")
                        .WithMany("Bookings")
                        .HasForeignKey("VehicleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Vehicle");
                });

            modelBuilder.Entity("dotnetapp.Models.Vehicle", b =>
                {
                    b.Navigation("Bookings");
                });
#pragma warning restore 612, 618
        }
    }
}
