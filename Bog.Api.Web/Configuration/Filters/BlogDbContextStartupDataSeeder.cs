using System;
using System.Linq;
using System.Threading.Tasks;
using Bog.Api.Db.DbContexts;
using Bog.Api.Domain.Data;
using Bog.Api.Domain.Values;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Bog.Api.Web.Configuration.Filters
{
    public class BlogDbContextStartupDataSeeder : IStartupFilter
    {
        private readonly ILogger<BlogDbContextStartupDataSeeder> _logger;
        private readonly IHostingEnvironment _env;

        public BlogDbContextStartupDataSeeder(ILogger<BlogDbContextStartupDataSeeder> logger, IHostingEnvironment env)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _env = env;
        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return (builder) =>
            {

                using (var scope = builder.ApplicationServices.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetService<BlogApiDbContext>();
                    var created = dbContext.Database.EnsureCreated();

                    _logger.LogInformation(LogEvenIdsValueObject.EnitityFramework, $"Check Database created status: {created}");

                    if (!created)
                    {
                        _logger.LogInformation(LogEvenIdsValueObject.EnitityFramework, "Applying entity framework DB migrations");
                        dbContext.Database.Migrate();
                        _logger.LogInformation(LogEvenIdsValueObject.EnitityFramework, "Applying entity framework DB migrations");
                    }
                    
                    if (_env.IsDevelopment())
                    {
                        SeedTestData(dbContext);
                    }
                }

                next(builder);
            };
        }

        private void SeedTestData(BlogApiDbContext dbContext)
        {
            if (!dbContext.Blogs.Any())
            {
                using (var transaction = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var testBlog = new Blog
                        {
                            Id = Guid.Parse("8724c4c5-7956-484a-896b-f379fdbc7d8c")
                        };

                        dbContext.Add(testBlog);

                        var article = new Article()
                        {
                            BlogId = testBlog.Id,
                            Author = "Test Guy",
                            Created = DateTimeOffset.UtcNow
                        };

                        dbContext.Add(article);
                        dbContext.SaveChanges();

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(LogEvenIdsValueObject.EnitityFramework, $"Could not see data: {ex}");
                        transaction.Rollback();
                    }
                    
                }
            }
        }
    }
}