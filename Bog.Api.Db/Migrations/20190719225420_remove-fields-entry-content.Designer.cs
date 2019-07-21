﻿// <auto-generated />
using System;
using Bog.Api.Db.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Bog.Api.Db.Migrations
{
    [DbContext(typeof(BlogApiDbContext))]
    [Migration("20190719225420_remove-fields-entry-content")]
    partial class removefieldsentrycontent
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Bog.Api.Domain.Data.Article", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author")
                        .IsRequired();

                    b.Property<Guid>("BlogId");

                    b.Property<DateTimeOffset>("Created")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset?>("Deleted");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsPublished");

                    b.Property<DateTimeOffset?>("Updated");

                    b.HasKey("Id");

                    b.HasIndex("BlogId");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("Bog.Api.Domain.Data.Blog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.HasKey("Id");

                    b.ToTable("Blogs");
                });

            modelBuilder.Entity("Bog.Api.Domain.Data.EntryContent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("ArticleId");

                    b.Property<DateTimeOffset>("Created");

                    b.HasKey("Id");

                    b.HasIndex("ArticleId");

                    b.ToTable("EntryContents");
                });

            modelBuilder.Entity("Bog.Api.Domain.Data.Article", b =>
                {
                    b.HasOne("Bog.Api.Domain.Data.Blog", "Blog")
                        .WithMany("Articles")
                        .HasForeignKey("BlogId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Bog.Api.Domain.Data.EntryContent", b =>
                {
                    b.HasOne("Bog.Api.Domain.Data.Article", "Article")
                        .WithMany("ArticleEntries")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
