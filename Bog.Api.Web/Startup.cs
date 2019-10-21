using Bog.Api.Db.DbContexts;
using Bog.Api.Domain.DbContext;
using Bog.Api.Web.Configuration;
using Bog.Api.Web.Configuration.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            services.WithCloudUtilities();
            services.WithApiConfiguration(_configuration);
            services.WithEFDbContext();
            services.WithBlogStartupFilters();
            services.WithDTOCoordinators();
            services.WithMvc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
