using Apical.TaskManagement.Application.Models;
using MediatR;

namespace Apical.TaskManagement.Application.Commands.Example;

public class UpdateExampleNameCommand : IRequest<CommandResult<bool>>
{
    public int Id { get; set; }
    public string Name { get; set; }
}
