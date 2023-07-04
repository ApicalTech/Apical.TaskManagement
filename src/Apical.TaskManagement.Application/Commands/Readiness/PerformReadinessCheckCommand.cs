using Apical.TaskManagement.Application.Models;
using MediatR;

namespace Apical.TaskManagement.Application.Commands.Readiness;

public class PerformReadinessCheckCommand : IRequest<CommandResult<string>>
{
}
