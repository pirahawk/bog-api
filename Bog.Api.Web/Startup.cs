using Bog.Api.Db.DbContexts;
using Bog.Api.Domain.Configuration;
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


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<BlogApiDbContext>((sp, dbCtxBuilder) => { dbCtxBuilder.Options.UseSqlServer(); });
            services.Configure<EntityConfiguration>(_configuration.GetSection("ConnectionStrings"));
            services.AddDbContext<BlogApiDbContext>();
            services.AddTransient<IStartupFilter, BlogDbContextStartupDataSeeder>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
