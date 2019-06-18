using Bog.Api.Domain.Data;
using Microsoft.EntityFrameworkCore;

namespace Bog.Api.Db.DbContexts
{
    public class BlogApiDbContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Article> Articles { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>().HasKey(a => a.Id);
            modelBuilder.Entity<Article>().HasKey(a => a.Id);
            modelBuilder.Entity<Article>().HasOne(a => a.Blog).WithMany(b => b.Articles);




            base.OnModelCreating(modelBuilder);
        }
    }
}