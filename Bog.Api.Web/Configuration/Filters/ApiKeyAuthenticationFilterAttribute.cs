using System;
using System.Linq;
using Bog.Api.Domain.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace Bog.Api.Web.Configuration.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAuthenticationFilterAttribute : Attribute, IAuthorizationFilter
    {
        private readonly BlogApiSettings _apiSettings;
        private const string API_KEY_HEADER_NAME = "bog-api-k";

        public ApiKeyAuthenticationFilterAttribute(IOptions<BlogApiSettings> apiSettings)
        {
            _apiSettings = apiSettings.Value;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            StringValues apiKey;
            Guid apiKeyHeaderVal;

            if (!context.HttpContext.Request.Headers.TryGetValue(API_KEY_HEADER_NAME, out apiKey) 
                || string.IsNullOrWhiteSpace(apiKey.First())
                || !Guid.TryParse(apiKey.First(), out apiKeyHeaderVal)
                || apiKeyHeaderVal != _apiSettings.Api)

            {
                context.Result = new StatusCodeResult(401);
            }
        }
    }
}