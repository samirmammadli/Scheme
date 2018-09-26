﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Scheme.Entities;
using Scheme.Models;
using System;

namespace Scheme.Migrations
{
    [DbContext(typeof(ProjectContext))]
    partial class ProjectContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Scheme.Entities.Backlog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<short>("ColumnType");

                    b.Property<string>("Name");

                    b.Property<int?>("ProjectId");

                    b.Property<int?>("SprintId");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.HasIndex("SprintId");

                    b.ToTable("Backlogs");
                });

            modelBuilder.Entity("Scheme.Entities.Card", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("BacklogId");

                    b.Property<int?>("ColumnId");

                    b.Property<int?>("ColumnId1");

                    b.Property<string>("Text");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("BacklogId");

                    b.HasIndex("ColumnId");

                    b.HasIndex("ColumnId1");

                    b.HasIndex("UserId");

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("Scheme.Entities.Column", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<short>("ColumnType");

                    b.Property<string>("Name");

                    b.Property<int?>("ProjectId");

                    b.Property<int?>("ProjectId1");

                    b.Property<int?>("SprintId");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.HasIndex("ProjectId");

                    b.HasIndex("ProjectId1");

                    b.HasIndex("SprintId");

                    b.ToTable("Columns");
                });

            modelBuilder.Entity("Scheme.Entities.ForgotPassCode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Code");

                    b.Property<DateTime>("ExpireDate");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("ForgotCodes");
                });

            modelBuilder.Entity("Scheme.Entities.History", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CardId");

                    b.Property<DateTime>("Date");

                    b.Property<bool>("IsSended");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("CardId");

                    b.HasIndex("UserId");

                    b.ToTable("History");
                });

            modelBuilder.Entity("Scheme.Entities.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Message");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("Scheme.Entities.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("Scheme.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ProjectId");

                    b.Property<int>("Type");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.HasIndex("UserId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Scheme.Entities.Sprint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("ExpireDate");

                    b.Property<string>("Name");

                    b.Property<int>("ProjectId");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Sprints");
                });

            modelBuilder.Entity("Scheme.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<bool>("IsConfirmed")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<string>("Name");

                    b.Property<string>("PassHash");

                    b.Property<string>("Salt");

                    b.Property<string>("SurnName");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Scheme.Entities.VerificationCode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Code");

                    b.Property<DateTime>("Expires");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("VerificationCodes");
                });

            modelBuilder.Entity("Scheme.Entities.Backlog", b =>
                {
                    b.HasOne("Scheme.Entities.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId");

                    b.HasOne("Scheme.Entities.Sprint", "Sprint")
                        .WithMany()
                        .HasForeignKey("SprintId");
                });

            modelBuilder.Entity("Scheme.Entities.Card", b =>
                {
                    b.HasOne("Scheme.Entities.Backlog")
                        .WithMany("Cards")
                        .HasForeignKey("BacklogId");

                    b.HasOne("Scheme.Entities.Column")
                        .WithMany("Cards")
                        .HasForeignKey("ColumnId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Scheme.Entities.Column", "Column")
                        .WithMany()
                        .HasForeignKey("ColumnId1");

                    b.HasOne("Scheme.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Scheme.Entities.Column", b =>
                {
                    b.HasOne("Scheme.Entities.Project")
                        .WithMany("Columns")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Scheme.Entities.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId1");

                    b.HasOne("Scheme.Entities.Sprint", "Sprint")
                        .WithMany("Columns")
                        .HasForeignKey("SprintId");
                });

            modelBuilder.Entity("Scheme.Entities.ForgotPassCode", b =>
                {
                    b.HasOne("Scheme.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Scheme.Entities.History", b =>
                {
                    b.HasOne("Scheme.Entities.Card", "Card")
                        .WithMany()
                        .HasForeignKey("CardId");

                    b.HasOne("Scheme.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Scheme.Entities.Notification", b =>
                {
                    b.HasOne("Scheme.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Scheme.Entities.Role", b =>
                {
                    b.HasOne("Scheme.Entities.Project", "Project")
                        .WithMany("Roles")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Scheme.Entities.User", "User")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Scheme.Entities.Sprint", b =>
                {
                    b.HasOne("Scheme.Entities.Project", "Project")
                        .WithMany("Sprints")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Scheme.Entities.VerificationCode", b =>
                {
                    b.HasOne("Scheme.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
