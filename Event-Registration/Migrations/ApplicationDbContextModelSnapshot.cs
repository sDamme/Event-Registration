﻿// <auto-generated />
using System;
using Event_Registration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Event_Registration.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Event_Registration.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ExtraInformation")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Time")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Event_Registration.Guest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("EventId")
                        .HasColumnType("integer");

                    b.Property<string>("ExtraInformation")
                        .IsRequired()
                        .HasMaxLength(5000)
                        .HasColumnType("character varying(5000)");

                    b.Property<int>("GuestType")
                        .HasColumnType("integer");

                    b.Property<int>("PaymentType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.ToTable("Guests");

                    b.HasDiscriminator<int>("GuestType");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Event_Registration.Business", b =>
                {
                    b.HasBaseType("Event_Registration.Guest");

                    b.Property<string>("LegalName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("NumberOfAttendees")
                        .HasColumnType("integer");

                    b.Property<string>("RegistrationCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasDiscriminator().HasValue(2);
                });

            modelBuilder.Entity("Event_Registration.Individual", b =>
                {
                    b.HasBaseType("Event_Registration.Guest");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PersonalIdCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("Event_Registration.Guest", b =>
                {
                    b.HasOne("Event_Registration.Event", "Event")
                        .WithMany("Guests")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");
                });

            modelBuilder.Entity("Event_Registration.Event", b =>
                {
                    b.Navigation("Guests");
                });
#pragma warning restore 612, 618
        }
    }
}
