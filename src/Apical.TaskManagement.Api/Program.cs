//using Lamar.Microsoft.DependencyInjection;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.Hosting;

//namespace Apical.TaskManagement.Api
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            CreateHostBuilder(args).Build().Run();

//        }

//        public static IHostBuilder CreateHostBuilder(string[] args) =>
//            Host.CreateDefaultBuilder(args)
//                .UseLamar()
//                .ConfigureWebHostDefaults(webBuilder =>
//                {
//                    webBuilder.UseStartup<Startup>();
//                    webBuilder.UseUrls("http://localhost:5000");
//                });
//    }
//}

using System.Text.Json.Serialization;
using Apical.TaskManagement.Api;
using Apical.TaskManagement.Api.Configurations.Extensions;
using Apical.TaskManagement.Api.HealthChecks;
using Apical.TaskManagement.Api.Middleware.ExceptionHandling;
using Apical.TaskManagement.Api.Middleware.Logging;

using CorrelationId;
using CorrelationId.DependencyInjection;
using CorrelationId.HttpClient;

using Lamar;
using Lamar.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Win32;

using Serilog;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddDefaultCorrelationId();
//builder.Services.AddHttpContextAccessor();
//builder.Services.AddMvc();
//builder.Services.AddVersionedApiExplorer(options =>
//{
//    options.GroupNameFormat = "'v'VVV";
//    options.SubstituteApiVersionInUrl = true;
//});
//builder.Services.AddApiVersioning(o =>
//{
//    o.ReportApiVersions = true;
//    o.DefaultApiVersion = new ApiVersion(1, 0);
//    o.AssumeDefaultVersionWhenUnspecified = true;
//});

//builder.Services.AddOptions();
//builder.Services.AddHttpClient(string.Empty)
//    .AddCorrelationIdForwarding();

//builder.Services.AddSwagger();
//builder.Services.AddHealthChecks().AddCheck<ReadinessCheck>("Apical.TaskManagement readiness", tags: new[] {"readiness"});
//builder.Services.AddCustomizedLogging();

//builder.Services.AddHealthChecks();
//builder.Services.AddControllers()
//    .AddJsonOptions(options =>
//        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
//builder.Host.UseSerilog();
// use Lamar as DI.
builder.Host.UseLamar((context, registry) =>
{
    // register services using Lamar
    registry.ConfigureContainer(context.Configuration);
});
//builder.Host.UseLamar();
builder.WebHost.UseUrls("http://localhost:5000");

var app=builder.Build();

app.UseCorrelationId();
//app.UseSerilogRequestLogging();
app.UseMiddleware<ExceptionMiddleware>();
app.UseSwaggerDocumentation(app.Services.GetRequiredService<IApiVersionDescriptionProvider>());
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseHealthChecks("/health");
app.MapControllers();

await app.RunAsync();
