﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SeaBattleBot.Infrastructure.Context;

#nullable disable

namespace SeaBattleBot.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20240221191909_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.16");

            modelBuilder.Entity("SeaBattleBot.Domain.Entities.EnemyState", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<byte?>("FirstHittedMoveColumn")
                        .HasColumnType("INTEGER");

                    b.Property<byte?>("FirstHittedMoveRow")
                        .HasColumnType("INTEGER");

                    b.Property<long>("GameStateId")
                        .HasColumnType("INTEGER");

                    b.Property<byte?>("LastHittedMoveColumn")
                        .HasColumnType("INTEGER");

                    b.Property<byte?>("LastHittedMoveRow")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LastMoveHitted")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("LastMoveShipDirection")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("EnemiesState");
                });

            modelBuilder.Entity("SeaBattleBot.Domain.Entities.GameState", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("EnemyFieldAsJson")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("EnemyStateId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("GameStatus")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PlayerFieldAsJson")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("EnemyStateId")
                        .IsUnique();

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("GamesState");
                });

            modelBuilder.Entity("SeaBattleBot.Domain.Entities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("ChatId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("GameStateId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SeaBattleBot.Domain.Entities.GameState", b =>
                {
                    b.HasOne("SeaBattleBot.Domain.Entities.EnemyState", "EnemyState")
                        .WithOne("GameState")
                        .HasForeignKey("SeaBattleBot.Domain.Entities.GameState", "EnemyStateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SeaBattleBot.Domain.Entities.User", "User")
                        .WithOne("GameState")
                        .HasForeignKey("SeaBattleBot.Domain.Entities.GameState", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EnemyState");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SeaBattleBot.Domain.Entities.EnemyState", b =>
                {
                    b.Navigation("GameState")
                        .IsRequired();
                });

            modelBuilder.Entity("SeaBattleBot.Domain.Entities.User", b =>
                {
                    b.Navigation("GameState")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
