using System.Data;
using System.Data.SqlClient;
using Apical.TaskManagement.Application;
using Apical.TaskManagement.Application.Models;
using Apical.TaskManagement.Infrastructure;

using FluentValidation;
using Lamar;
using MediatR;
using Microsoft.Extensions.Options;

namespace Apical.TaskManagement.Api.Configurations.Extensions;

public static class DependencyInjectionConfigurationExtensions
{
    internal static void AddDependencyInjection(this ServiceRegistry services, IConfiguration configuration)
    {
        // Map the environment variables to an object that represents them
        services.Configure<EnvironmentConfiguration>(configuration);
        services.AddTransient<IDbConnection>(x =>
        {
            var config =
                x.GetRequiredService<IOptions<EnvironmentConfiguration>>();
            return new SqlConnection(config.Value.SQL_CONNECTION_STRING);
        });
        // https://jasperfx.github.io/lamar/documentation/ioc/registration/auto-registration-and-conventions/
        services.Scan(_ =>
        {
            _.TheCallingAssembly();
            _.Assembly(typeof(IApplication).Assembly);
            _.Assembly(typeof(IInfrastructure).Assembly);
            _.AddAllTypesOf<IValidator>();
            _.ConnectImplementationsToTypesClosing(typeof(IValidator<>));
            _.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>));
            _.ConnectImplementationsToTypesClosing(typeof(INotificationHandler<>));
            _.WithDefaultConventions();
            _.LookForRegistries();
        });
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Startup).Assembly));
    }

    //internal static void AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    //{
    //    // Map the environment variables to an object that represents them
    //    services.Configure<EnvironmentConfiguration>(configuration);
    //    services.AddTransient<IDbConnection>(x =>
    //    {
    //        var config =
    //            x.GetRequiredService<IOptions<EnvironmentConfiguration>>();
    //        return new SqlConnection(config.Value.SQL_CONNECTION_STRING);
    //    });
    //    // https://jasperfx.github.io/lamar/documentation/ioc/registration/auto-registration-and-conventions/
    //    services.Scan(_ =>
    //    {
    //        _.TheCallingAssembly();
    //        _.Assembly(typeof(IApplication).Assembly);
    //        _.Assembly(typeof(IInfrastructure).Assembly);
    //        _.AddAllTypesOf<IValidator>();
    //        _.ConnectImplementationsToTypesClosing(typeof(IValidator<>));
    //        _.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>));
    //        _.ConnectImplementationsToTypesClosing(typeof(INotificationHandler<>));
    //        _.WithDefaultConventions();
    //        _.LookForRegistries();
    //    });
    //    services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Startup).Assembly));
    //}
}
