﻿using Bog.Api.Web.Configuration.Filters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Bog.Api.Web.Configuration
{
    public static class StartupFilterExtensions
    {
        public static void WithBlogStartupFilters(this IServiceCollection services)
        {
            services.AddTransient<IStartupFilter, BlogDbContextStartupDataSeeder>();
        }
    }
}