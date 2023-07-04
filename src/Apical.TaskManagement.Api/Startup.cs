using System.Text.Json.Serialization;
using CorrelationId;
using Lamar;
using Microsoft.AspNetCore.Mvc;
using Apical.TaskManagement.Api.Configurations.Extensions;
using Apical.TaskManagement.Api.Middleware.ExceptionHandling;
using CorrelationId.DependencyInjection;
using CorrelationId.HttpClient;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Apical.TaskManagement.Api.HealthChecks;
using Apical.TaskManagement.Api.Middleware.Logging;

namespace Apical.TaskManagement.Api;

public static class Startup
{
    //public IConfiguration Configuration { get; }

    //public Startup(IConfiguration configuration)
    //{
    //    Configuration = configuration;
    //}

    // This method gets called by the runtime. Use this method to add services to the container.
    public static void ConfigureContainer(this ServiceRegistry services,IConfiguration configuration)
    {
        services.AddDefaultCorrelationId();
        services.AddHttpContextAccessor();
        services.AddMvc();
        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
        services.AddApiVersioning(o =>
        {
            o.ReportApiVersions = true;
            o.DefaultApiVersion = new ApiVersion(1, 0);
            o.AssumeDefaultVersionWhenUnspecified = true;
        });

        services.AddOptions();
        services.AddHttpClient(string.Empty)
            .AddCorrelationIdForwarding();

        services.AddSwagger();
        services.AddHealthChecks().AddCheck<ReadinessCheck>("Apical.TaskManagement readiness", tags: new[] {"readiness"});
        services.AddCustomizedLogging();
        services.AddDependencyInjection(configuration);

        services.AddHealthChecks();
        services.AddControllers()
            .AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public static IApplicationBuilder Configure(IApplicationBuilder app, IApiVersionDescriptionProvider provider)
    {
        app.UseCorrelationId();
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseSwaggerDocumentation(provider);
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseHealthChecks("/health");
        app.UseEndpoints(endpoints => {
            endpoints.MapControllers();
        });

        return app;
    }
}
