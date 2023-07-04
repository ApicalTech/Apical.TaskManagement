using System.Threading;
using System.Threading.Tasks;
using Apical.TaskManagement.Application.Models;

using JetBrains.Annotations;

using MediatR;
using Microsoft.Extensions.Options;
using Serilog;


namespace Apical.TaskManagement.Application.Commands.Readiness;
[UsedImplicitly]
public class PerformReadinessCheckCommandHandler : IRequestHandler<PerformReadinessCheckCommand, CommandResult<string>>
{
    private readonly EnvironmentConfiguration _configuration;
    private readonly ILogger _logger;

    public PerformReadinessCheckCommandHandler(
        ILogger logger,
        IOptions<EnvironmentConfiguration> configuration)
    {
        _logger = logger;
        _configuration = configuration.Value;
    }

    public  Task<CommandResult<string>> Handle(PerformReadinessCheckCommand command, CancellationToken cancellationToken)
    {
        // Not using a validator here because we want to dynamically check the environment variables that are added or removed
        var type = _configuration.GetType();

        foreach (var property in type.GetProperties())
        {
            if (property.GetValue(_configuration, null) == null)
            {
                return Task.FromResult( new CommandResult<string>(result: $"Configuration Error. Property {property.Name} is null.", type: CommandResultTypeEnum.InvalidInput));
            }
        }
        // Here we can check if we can connect to the database or other dependent services

        return Task.FromResult(new CommandResult<string>(result: string.Empty, type: CommandResultTypeEnum.Success));
    }
}
