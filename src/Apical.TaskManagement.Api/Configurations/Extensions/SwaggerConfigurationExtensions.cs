using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

namespace Apical.TaskManagement.Api.Configurations.Extensions;

public static class SwaggerConfigurationExtensions
{
    internal static void UseSwaggerDocumentation(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
    {
        app.UseSwagger();
        app.UseSwaggerUI(
            options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
            });
    }

    internal static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Apical.TaskManagement API",
                Description = "API endpoints for the Apical.TaskManagement API",
                Contact = new OpenApiContact
                {
                    Name = "",
                    Email = string.Empty,
                }
            });

            options.SwaggerDoc("v2", new OpenApiInfo
            {
                Version = "v2",
                Title = "Apical.TaskManagement API",
                Description = "API endpoints for the Apical.TaskManagement API",
                Contact = new OpenApiContact
                {
                    Name = "",
                    Email = string.Empty,
                }
            });
            // This coupled with the properties in the csproj allow the swagger page to show additional comments for methods
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
        });
    }
}
