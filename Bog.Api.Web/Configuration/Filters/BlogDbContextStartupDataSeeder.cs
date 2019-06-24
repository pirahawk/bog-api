using System;
using Bog.Api.Db.DbContexts;
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
        //private readonly BlogApiDbContext _context;
        private readonly ILogger<BlogDbContextStartupDataSeeder> _logger;

        public BlogDbContextStartupDataSeeder(ILogger<BlogDbContextStartupDataSeeder> logger)
        {
            //_context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
                }

                next(builder);
            };
        }
    }
}