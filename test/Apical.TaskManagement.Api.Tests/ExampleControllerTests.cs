using System.Threading;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Apical.TaskManagement.Application.Models;
using Apical.TaskManagement.Domain.Models;
using Apical.TaskManagement.Application.Commands.Example;
using Apical.TaskManagement.Application.Queries.Example;
using Apical.TaskManagement.Api.Controllers;
using Moq;
using Serilog;
using Xunit;
using Microsoft.Extensions.Logging;

namespace Apical.TaskManagement.Api.Tests;

public class ExampleControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<ILogger<ExampleController>> _loggerMock;
    public ExampleControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _loggerMock = new();
    }

    [Fact]
    public async void GetExampleById_ShouldReturnOkResult()
    {
        // ARRANGE     
        _mediatorMock
            .Setup(x => x.Send(It.IsAny<GetExampleByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new QueryResult<Example>());
        var controller = new ExampleController(
            _loggerMock.Object
        );

        // ACT
        var response = await controller.GetExampleById(1);

        // ASSERT
        Assert.IsType<OkObjectResult>(response.Result);
        _mediatorMock.Verify(x => x.Send(It.IsAny<GetExampleByIdQuery>(), It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async void GetExampleById_ShouldReturnNotFoundResult_WhenQueryResultNotFound()
    {
        // ARRANGE     
        _mediatorMock
            .Setup(x => x.Send(It.IsAny<GetExampleByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new QueryResult<Example>() { Type = QueryResultTypeEnum.NotFound });
        var controller = new ExampleController(
            _loggerMock.Object
        );

        // ACT
        var response = await controller.GetExampleById(1);

        // ASSERT
        Assert.IsType<NotFoundResult>(response.Result);
    }

    [Fact]
    public async void GetExampleById_ShouldReturnBadResult_WhenInvalidInput()
    {
        // ARRANGE     
        _mediatorMock
            .Setup(x => x.Send(It.IsAny<GetExampleByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new QueryResult<Example>() { Type = QueryResultTypeEnum.InvalidInput });
        var controller = new ExampleController(
            _loggerMock.Object
        );

        // ACT
        var response = await controller.GetExampleById(1);

        // ASSERT
        Assert.IsType<BadRequestResult>(response.Result);
    }

    [Fact]
    public async void UpdateExampleNameById_ShouldReturnOkResult()
    {
        // ARRANGE      
        _mediatorMock
            .Setup(x => x.Send(It.IsAny<UpdateExampleNameCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CommandResult<bool>());
        var controller = new ExampleController(
            _loggerMock.Object
        );

        // ACT
        var response = await controller.UpdateExampleNameById(1, "newName");

        // ASSERT
        Assert.IsType<OkObjectResult>(response.Result);
        _mediatorMock.Verify(x => x.Send(It.IsAny<UpdateExampleNameCommand>(), It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async void UpdateExampleNameById_ShouldReturnNotFoundResult_WhenCommandResultNotFound()
    {
        // ARRANGE 
        _mediatorMock
            .Setup(x => x.Send(It.IsAny<UpdateExampleNameCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CommandResult<bool>() { Type = CommandResultTypeEnum.NotFound });
        var controller = new ExampleController(
            _loggerMock.Object
        );

        // ACT
        var response = await controller.UpdateExampleNameById(1, "newName");

        // ASSERT
        Assert.IsType<NotFoundResult>(response.Result);
    }

    [Fact]
    public async void UpdateExampleNameById_ShouldReturnBad_Result_WhenInvalidInput()
    {
        // ARRANGE 
        _mediatorMock
            .Setup(x => x.Send(It.IsAny<UpdateExampleNameCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CommandResult<bool>() { Type = CommandResultTypeEnum.InvalidInput });
        var controller = new ExampleController(
            _loggerMock.Object
        );

        // ACT
        var response = await controller.UpdateExampleNameById(1, "newName");

        // ASSERT
        Assert.IsType<BadRequestResult>(response.Result);
    }
}
