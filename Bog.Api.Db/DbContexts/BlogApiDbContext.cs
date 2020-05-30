using System;
using Bog.Api.Domain.Configuration;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bog.Api.Db.DbContexts
{
    public partial class BlogApiDbContext : DbContext
    {
        private readonly string _connection;
        private readonly ILogger<BlogApiDbContext> _logger;

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<EntryContent> EntryContents { get; set; }
        public DbSet<EntryMedia> EntryMedia { get; set; }
        public DbSet<MetaTag> MetaTags { get; set; }

        public BlogApiDbContext(IOptionsMonitor<EntityConfiguration> entityContextOptionsAccessor, ILogger<BlogApiDbContext> logger)
        {
            if (entityContextOptionsAccessor.CurrentValue == null) throw new ArgumentNullException(nameof(entityContextOptionsAccessor));
            _logger = logger;

            _connection = entityContextOptionsAccessor.CurrentValue.BlogApiDbContext;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(_connection);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _logger.LogInformation(LogEvenIdsValueObject.EnitityFramework, "Entity Model Creation start");

            modelBuilder.Entity<Blog>().HasKey(a => a.Id);

            modelBuilder.Entity<Article>().HasKey(a => a.Id);
            modelBuilder.Entity<Article>().Property(a => a.Author).IsRequired();
            modelBuilder.Entity<Article>().Property(a => a.Title).IsRequired();
            modelBuilder.Entity<Article>().Property(a => a.Created).ValueGeneratedOnAdd();

            modelBuilder.Entity<Article>()
                .HasOne(a => a.Blog)
                .WithMany(b => b.Articles)
                .HasForeignKey(a => a.BlogId);

            modelBuilder.Entity<EntryContent>().HasKey(ec => ec.Id);
            modelBuilder.Entity<EntryContent>()
                .HasOne(ec => ec.Article)
                .WithMany(a => a.ArticleEntries)
                .HasForeignKey(ec => ec.ArticleId);

            modelBuilder.Entity<EntryMedia>().HasKey(em => em.Id);
            modelBuilder.Entity<EntryMedia>().Property(em => em.FileName).IsRequired();
            modelBuilder.Entity<EntryMedia>().Property(em => em.ContentType).IsRequired();
            modelBuilder.Entity<EntryMedia>().Property(em => em.BlobFileName).IsRequired();
            modelBuilder.Entity<EntryMedia>().Property(em => em.MD5Base64Hash).IsRequired();
            modelBuilder.Entity<EntryMedia>()
                .HasOne(em => em.EntryContent)
                .WithMany(ec => ec.EntryMedia)
                .HasForeignKey(em => em.EntryContentId);

            modelBuilder.Entity<MetaTag>().HasKey(mt => mt.Id);
            modelBuilder.Entity<MetaTag>().Property(mt=>mt.Name).IsRequired();
            modelBuilder.Entity<MetaTag>().HasKey(mt => mt.Id);
            modelBuilder.Entity<MetaTag>()
                .HasOne(mt => mt.Article)
                .WithMany(a => a.MetaTags)
                .HasForeignKey(mt => mt.ArticleId);

            base.OnModelCreating(modelBuilder);

            _logger.LogInformation(LogEvenIdsValueObject.EnitityFramework, "Entity Model Creation complete");
        }
    }
}