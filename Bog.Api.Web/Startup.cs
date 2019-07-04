using Bog.Api.Db.DbContexts;
using Bog.Api.Domain.Configuration;
using Bog.Api.Domain.DbContext;
using Bog.Api.Web.Configuration;
using Bog.Api.Web.Configuration.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bog.Api.Web
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<BlogApiDbContext>((sp, dbCtxBuilder) => { dbCtxBuilder.Options.UseSqlServer(); });
            services.WithUtilities();
            services.Configure<EntityConfiguration>(_configuration.GetSection("ConnectionStrings"));
            services.AddDbContext<BlogApiDbContext>();
            services.AddTransient<IStartupFilter, BlogDbContextStartupDataSeeder>();
            services.AddTransient<IBlogApiDbContext, BlogApiDbContextAdapter>();
            services.AddMvc();
            services.WithDTOCoordinators();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
