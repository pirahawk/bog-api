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

            services.AddTransient<ICreateArticleCoordinator, CreateArticleCoordinator>();
            services.AddTransient<IFindBlogArticleCoordinator, FindBlogArticleCoordinator>();
            services.AddTransient<IUpdateArticleCoordinator, UpdateArticleCoordinator>();
            services.AddTransient<IDeleteArticleCoordinator, DeleteArticleCoordinator>();
            services.AddTransient<ICreateArticleEntryCoordinator, CreateArticleEntryCoordinator>();
            services.AddTransient<IUploadArticleEntryCoordinator, UploadArticleEntryCoordinator>();
            services.AddTransient<ICreateAndPersistArticleEntryStrategy, CreateAndPersistArticleEntryStrategy>();
            services.AddTransient<ICreateAndPersistArticleEntryMediaStrategy, CreateAndPersistArticleEntryMediaStrategy>();
            services.AddTransient<ICreateEntryMediaCoordinator, CreateEntryMediaCoordinator>();
            services.AddTransient<IUploadArticleEntryMediaCoordinator, UploadArticleEntryMediaCoordinator>();
            services.AddTransient<IEntryMediaSearchStrategy, EntryMediaSearchStrategy>();
            services.AddTransient<IGetLatestArticleEntryStrategy, GetLatestArticleEntryStrategy>();
            services.AddTransient<IGetArticleMediaLookupStrategy, GetArticleMediaLookupStrategy>();
            services.AddTransient<IGetMediaLookupQuery, GetMediaLookupQuery>();
            services.AddTransient<IAddMetaTagForArticleCoordinator, AddMetaTagForArticleCoordinator>();
            services.AddTransient<IRemoveMetaTagForArticleCoordinator, RemoveMetaTagForArticleCoordinator>();
            services.AddTransient<IBogMarkdownConverterStrategy, BogMarkdownConverterStrategy>();
            services.AddTransient<IGetTagsForArticleCoordinator, GetTagsForArticleCoordinator>();

        }
    }
}