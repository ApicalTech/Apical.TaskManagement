using Apical.TaskManagement.Application.Models;
using MediatR;

namespace Apical.TaskManagement.Application.Queries.Example;

public class GetExampleByIdQuery : IRequest<QueryResult<Domain.Models.Example>>
{
    public int Id { get; set; }
}
