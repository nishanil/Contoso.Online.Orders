using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoOnlineOrders.Abstractions;
using ContosoOnlineOrders.Api.Infrastructure;
using ContosoOnlineOrders.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ContosoOnlineOrders.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddMemoryCache();
            //services.AddSingleton<IStoreDataService, MemoryCachedStoreServices>();

            services.AddCosmosDbStorage(Configuration.GetConnectionString("ContosoOrdersConnectionString"));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                // Commenting out for the AKS public API URL
                //c.AddServer(new OpenApiServer { Url = "http://0.0.0.0:5000" });

                c.OperationFilter<SwaggerDefaultValues>();
            });
            services.AddApiVersioning();
            services.AddVersionedApiExplorer(options =>
            {
                options.DefaultApiVersion = ApiVersion.Parse("1.2");
                options.AssumeDefaultVersionWhenUnspecified = true;
            });
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IWebHostEnvironment env,
            IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.DisplayOperationId();
                    var versionDescriptions = provider.ApiVersionDescriptions;
                    foreach (var description in provider.ApiVersionDescriptions.OrderByDescending(_ => _.ApiVersion))
                    {
                        c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"Contoso Online Orders {description.GroupName}");
                    }
                });
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
