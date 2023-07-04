using Apical.TaskManagement.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Apical.TaskManagement.Api.Controllers;

public abstract class ApiControllerBase : ControllerBase
{
    private ISender? _mediator;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    protected async Task<ActionResult> HandleCommandAsync<T>(IRequest<CommandResult<T>> command)
    {
        var result = await Mediator.Send(command);

        return result.Type switch
        {
            CommandResultTypeEnum.InvalidInput => new BadRequestResult(),
            CommandResultTypeEnum.NotFound => new NotFoundResult(),
            _ => new OkObjectResult(result.Result)
        };
    }


    protected async Task<ActionResult> HandleQueryAsync<T>(IRequest<QueryResult<T>> query)
    {
        var result = await Mediator.Send(query);

        return result.Type switch
        {
            QueryResultTypeEnum.InvalidInput => new BadRequestResult(),
            QueryResultTypeEnum.NotFound => new NotFoundResult(),
            _ => new OkObjectResult(result.Result)
        };
    }
}
