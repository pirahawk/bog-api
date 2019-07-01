using System;
using Bog.Api.Domain.Coordinators;
using Microsoft.Extensions.DependencyInjection;

namespace Bog.Api.Web.Configuration
{
    public static class CoordinatorsServicesExtensions
    {
        public static void WithDTOCoordinators(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddTransient<ICreateBlogEntryCoordinator, CreateBlogEntryCoordinator>();
            services.AddTransient<IFindBlogArticleCoordinator, FindBlogArticleCoordinator>();
        }
    }
}