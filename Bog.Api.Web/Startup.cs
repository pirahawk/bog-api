using Bog.Api.Web.Configuration;

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
            //services.WithMvc();
            //services.WithMarkdownConverterClient();
            services.WithUtilities();
            services.WithCloudUtilities();
            services.WithApiConfiguration(_configuration);
            services.WithEFDbContext();
            services.WithBlogStartupFilters();
            services.WithAuthenticationFilters();
            services.WithDTOCoordinators();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
