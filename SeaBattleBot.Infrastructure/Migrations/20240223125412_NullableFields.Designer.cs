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
    [Migration("20240223125412_NullableFields")]
    partial class NullableFields
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.16");

            modelBuilder.Entity("SeaBattleBot.Core.Domain.Entities.EnemyState", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<byte?>("FirstHittedMoveColumn")
                        .HasColumnType("INTEGER");

                    b.Property<byte?>("FirstHittedMoveRow")
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

            modelBuilder.Entity("SeaBattleBot.Core.Domain.Entities.GameState", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("EnemyFieldAsJson")
                        .HasColumnType("TEXT");

                    b.Property<long>("EnemyStateId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("GameStatus")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PlayerFieldAsJson")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("EnemyStateId");

                    b.ToTable("GamesState");
                });

            modelBuilder.Entity("SeaBattleBot.Core.Domain.Entities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("ChatId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("GameStateId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("GameStateId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SeaBattleBot.Core.Domain.Entities.GameState", b =>
                {
                    b.HasOne("SeaBattleBot.Core.Domain.Entities.EnemyState", "EnemyState")
                        .WithMany()
                        .HasForeignKey("EnemyStateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EnemyState");
                });

            modelBuilder.Entity("SeaBattleBot.Core.Domain.Entities.User", b =>
                {
                    b.HasOne("SeaBattleBot.Core.Domain.Entities.GameState", "GameState")
                        .WithMany()
                        .HasForeignKey("GameStateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GameState");
                });
#pragma warning restore 612, 618
        }
    }
}
