using System;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ContosoOnlineOrders.Api.Infrastructure
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider provider;
        private IConfiguration _configuration;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, IConfiguration configuration)
        {
            this.provider = provider;
            this._configuration = configuration;
        }

        public void Configure(SwaggerGenOptions options)
        {
            var environment = _configuration["ENV"];

            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description, environment));
            }
        }

        static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description, string environment)
        {


            var info = new OpenApiInfo()
            {
                Title = $"Store APIs - {environment}",
                Version = description.ApiVersion.ToString()
            };

            return info;
        }
    }
}
