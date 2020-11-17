﻿// <auto-generated />
using System;
using FullTextSearch.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using NpgsqlTypes;

namespace FTSApp.Migrations
{
    [DbContext(typeof(FTSDBContext))]
    [Migration("20201112031810_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("FTSApp.Models.Image", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("images_id")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnName("description")
                        .HasColumnType("text");

                    b.Property<NpgsqlTsVector>("ImageVector")
                        .HasColumnName("image_vector")
                        .HasColumnType("tsvector");

                    b.Property<string>("KeyWords")
                        .HasColumnName("key_words")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasColumnName("title")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("images");
                });
#pragma warning restore 612, 618
        }
    }
}
