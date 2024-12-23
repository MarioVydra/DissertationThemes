﻿// <auto-generated />
using System;
using DissertationThemes.SharedLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DissertationThemes.ImporterApp.Migrations
{
    [DbContext(typeof(DissertationThemesContext))]
    partial class DissertationThemesContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("DissertationThemes.SharedLibrary.StProgram", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FieldOfStudy")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("StProgram");
                });

            modelBuilder.Entity("DissertationThemes.SharedLibrary.Supervisor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Supervisors");
                });

            modelBuilder.Entity("DissertationThemes.SharedLibrary.Theme", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsExternalStudy")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsFullTimeStudy")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ResearchType")
                        .HasColumnType("INTEGER");

                    b.Property<int>("StProgramId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SupervisorId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("StProgramId");

                    b.HasIndex("SupervisorId");

                    b.ToTable("Themes");
                });

            modelBuilder.Entity("DissertationThemes.SharedLibrary.Theme", b =>
                {
                    b.HasOne("DissertationThemes.SharedLibrary.StProgram", "StProgram")
                        .WithMany("Themes")
                        .HasForeignKey("StProgramId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DissertationThemes.SharedLibrary.Supervisor", "Supervisor")
                        .WithMany("Themes")
                        .HasForeignKey("SupervisorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("StProgram");

                    b.Navigation("Supervisor");
                });

            modelBuilder.Entity("DissertationThemes.SharedLibrary.StProgram", b =>
                {
                    b.Navigation("Themes");
                });

            modelBuilder.Entity("DissertationThemes.SharedLibrary.Supervisor", b =>
                {
                    b.Navigation("Themes");
                });
#pragma warning restore 612, 618
        }
    }
}
