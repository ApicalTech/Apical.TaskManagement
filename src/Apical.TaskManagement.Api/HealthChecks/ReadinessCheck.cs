using Apical.TaskManagement.Application.Models;
using MediatR;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Apical.TaskManagement.Application.Commands.Readiness;
using Serilog;

namespace Apical.TaskManagement.Api.HealthChecks;

public class ReadinessCheck : IHealthCheck
{
    private readonly ILogger<ReadinessCheck> _logger;
    private readonly IMediator _mediator;

    public ReadinessCheck(
        ILogger<ReadinessCheck> logger,
        IMediator mediator
    )
    {
        _logger = logger;
        _mediator = mediator;
    }


    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new())
    {
        try
        {
            var performReadinessCheckCommand = new PerformReadinessCheckCommand();

            var result = await _mediator.Send(performReadinessCheckCommand, cancellationToken);
            if (result.Type != CommandResultTypeEnum.Success)
            {
                throw new Exception(result.Result);
            }

            return HealthCheckResult.Healthy();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "ReadinessController has encountered an error: {Message}", e.Message);
            throw;
        }
    }
}
