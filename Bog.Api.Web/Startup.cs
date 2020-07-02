using Bog.Api.Web.Configuration;
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
            services.WithMvc();
            services.WithMarkdownConverterClient();
            services.WithUtilities();
            services.WithCloudUtilities();
            services.WithApiConfiguration(_configuration);
            services.WithEFDbContext();
            services.WithBlogStartupFilters();
            services.WithDTOCoordinators();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseCors(builder =>
            //{
            //    builder.AllowAnyOrigin();
            //    builder.AllowAnyHeader();
            //    builder.AllowAnyMethod();
            //});

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
