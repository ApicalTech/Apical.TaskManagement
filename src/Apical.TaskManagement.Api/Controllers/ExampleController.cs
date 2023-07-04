using Microsoft.AspNetCore.Mvc;
using Apical.TaskManagement.Application.Models;
using Apical.TaskManagement.Domain.Models;
using Apical.TaskManagement.Application.Commands.Example;
using Apical.TaskManagement.Application.Queries.Example;
using MediatR;
using Serilog;

namespace Apical.TaskManagement.Api.Controllers;

[Route("api/v{version:apiVersion}")]
[ApiController]
public class ExampleController : ApiControllerBase
{
    private readonly ILogger<ExampleController> _logger;


    public ExampleController(
        ILogger<ExampleController> logger
    )
    {
        _logger = logger;
    }

    /*
        ProducesResponseType helps Swagger be more verbose about what endpoints can return.
        Summary - Short description about the endpoint which will show on the Swagger pill next to the name of the endpoint
        Remarks - Long description or explanation about the endpoint which will show once the Swagger pill is opened
        Param - name attribute needs to match the method parameter name and will add a description column to each parameter in Swagger
    */

    /// <summary>
    /// Get an Example by it's ID
    /// </summary>
    /// <remarks>
    /// Retrieves an Example by the ID specified
    /// </remarks>
    /// <param name="id">ID of the example</param>
    [ApiVersion("1")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [HttpGet("examples/{id}")]
    public async Task<ActionResult<Example>> GetExampleById([FromRoute] int id)
    {
        var getExampleByIdQuery = new GetExampleByIdQuery()
        {
            Id = id,
        };
        return await HandleQueryAsync(getExampleByIdQuery);
    }

    /// <summary>
    /// Update an Example's name by it's ID
    /// </summary>
    /// <remarks>
    /// Updates the Name of any Examples with the ID specified to the name specified
    /// </remarks>
    /// <param name="id">ID of the example</param>
    /// <param name="name">The new name for the Example</param>

    [ApiVersion("2")]
    [ApiExplorerSettings(GroupName = "v2")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [HttpPut("{id}")]
    public async Task<ActionResult<bool>> UpdateExampleNameById([FromRoute] int id, [FromBody] string name)
    {
        var updateExampleNameCommand = new UpdateExampleNameCommand()
        {
            Id = id,
            Name = name,
        };
        return await HandleCommandAsync(updateExampleNameCommand);
    }

}
