﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProjectSem3.Models;

#nullable disable

namespace ProjectSem3.Migrations
{
    [DbContext(typeof(DoctorContext))]
    [Migration("20250105094217_v5")]
    partial class v5
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ProjectSem3.Models.Account", b =>
                {
                    b.Property<int>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AccountId"));

                    b.Property<string>("Avatar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Experience")
                        .HasColumnType("int");

                    b.Property<int?>("ExpertiseId")
                        .HasColumnType("int");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("LevelId")
                        .HasColumnType("int");

                    b.Property<int>("Online")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<int?>("PositionId")
                        .HasColumnType("int");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Status")
                        .HasColumnType("int");

                    b.HasKey("AccountId");

                    b.HasIndex("ExpertiseId");

                    b.HasIndex("LevelId");

                    b.HasIndex("PositionId");

                    b.ToTable("Account");
                });

            modelBuilder.Entity("ProjectSem3.Models.Answer", b =>
                {
                    b.Property<int>("AnswerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AnswerId"));

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.HasKey("AnswerId");

                    b.HasIndex("AccountId");

                    b.HasIndex("QuestionId");

                    b.ToTable("Answer");
                });

            modelBuilder.Entity("ProjectSem3.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryId"));

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("CategoryId");

                    b.ToTable("Categorie");
                });

            modelBuilder.Entity("ProjectSem3.Models.Expertise", b =>
                {
                    b.Property<int>("ExpertiseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ExpertiseId"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ExpertiseName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ExpertiseId");

                    b.ToTable("Expertise");
                });

            modelBuilder.Entity("ProjectSem3.Models.Favorite", b =>
                {
                    b.Property<int>("FavoriteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FavoriteId"));

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<int?>("AnswerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("PostId")
                        .HasColumnType("int");

                    b.Property<int?>("QuestionId")
                        .HasColumnType("int");

                    b.HasKey("FavoriteId");

                    b.HasIndex("AccountId");

                    b.HasIndex("AnswerId");

                    b.HasIndex("PostId");

                    b.HasIndex("QuestionId");

                    b.ToTable("Favorite");
                });

            modelBuilder.Entity("ProjectSem3.Models.Level", b =>
                {
                    b.Property<int>("LevelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LevelId"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("LevelName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LevelId");

                    b.ToTable("Level");
                });

            modelBuilder.Entity("ProjectSem3.Models.Position", b =>
                {
                    b.Property<int>("PositionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PositionId"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("PositionName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PositionId");

                    b.ToTable("Position");
                });

            modelBuilder.Entity("ProjectSem3.Models.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PostId"));

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TopicId")
                        .HasColumnType("int");

                    b.HasKey("PostId");

                    b.HasIndex("AccountId");

                    b.HasIndex("TopicId");

                    b.ToTable("Post");
                });

            modelBuilder.Entity("ProjectSem3.Models.Question", b =>
                {
                    b.Property<int>("QuestionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("QuestionId"));

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("QuestionId");

                    b.HasIndex("AccountId");

                    b.ToTable("Question");
                });

            modelBuilder.Entity("ProjectSem3.Models.Topic", b =>
                {
                    b.Property<int>("TopicId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TopicId"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TopicId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Topic");
                });

            modelBuilder.Entity("ProjectSem3.Models.Account", b =>
                {
                    b.HasOne("ProjectSem3.Models.Expertise", "Expertise")
                        .WithMany("Account")
                        .HasForeignKey("ExpertiseId");

                    b.HasOne("ProjectSem3.Models.Level", "Level")
                        .WithMany("Account")
                        .HasForeignKey("LevelId");

                    b.HasOne("ProjectSem3.Models.Position", "Position")
                        .WithMany("Account")
                        .HasForeignKey("PositionId");

                    b.Navigation("Expertise");

                    b.Navigation("Level");

                    b.Navigation("Position");
                });

            modelBuilder.Entity("ProjectSem3.Models.Answer", b =>
                {
                    b.HasOne("ProjectSem3.Models.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectSem3.Models.Question", "Question")
                        .WithMany("Answer")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("ProjectSem3.Models.Favorite", b =>
                {
                    b.HasOne("ProjectSem3.Models.Account", "Account")
                        .WithMany("Favorite")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ProjectSem3.Models.Answer", "Answer")
                        .WithMany()
                        .HasForeignKey("AnswerId");

                    b.HasOne("ProjectSem3.Models.Post", "Post")
                        .WithMany("Favorite")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ProjectSem3.Models.Question", "Question")
                        .WithMany("Favorite")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Account");

                    b.Navigation("Answer");

                    b.Navigation("Post");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("ProjectSem3.Models.Post", b =>
                {
                    b.HasOne("ProjectSem3.Models.Account", "Accounts")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjectSem3.Models.Topic", "Topic")
                        .WithMany("Post")
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Accounts");

                    b.Navigation("Topic");
                });

            modelBuilder.Entity("ProjectSem3.Models.Question", b =>
                {
                    b.HasOne("ProjectSem3.Models.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("ProjectSem3.Models.Topic", b =>
                {
                    b.HasOne("ProjectSem3.Models.Category", "Category")
                        .WithMany("Topic")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("ProjectSem3.Models.Account", b =>
                {
                    b.Navigation("Favorite");
                });

            modelBuilder.Entity("ProjectSem3.Models.Category", b =>
                {
                    b.Navigation("Topic");
                });

            modelBuilder.Entity("ProjectSem3.Models.Expertise", b =>
                {
                    b.Navigation("Account");
                });

            modelBuilder.Entity("ProjectSem3.Models.Level", b =>
                {
                    b.Navigation("Account");
                });

            modelBuilder.Entity("ProjectSem3.Models.Position", b =>
                {
                    b.Navigation("Account");
                });

            modelBuilder.Entity("ProjectSem3.Models.Post", b =>
                {
                    b.Navigation("Favorite");
                });

            modelBuilder.Entity("ProjectSem3.Models.Question", b =>
                {
                    b.Navigation("Answer");

                    b.Navigation("Favorite");
                });

            modelBuilder.Entity("ProjectSem3.Models.Topic", b =>
                {
                    b.Navigation("Post");
                });
#pragma warning restore 612, 618
        }
    }
}
