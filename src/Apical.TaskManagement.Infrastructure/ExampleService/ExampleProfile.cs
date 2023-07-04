using AutoMapper;
using Apical.TaskManagement.Domain.Models;

namespace Apical.TaskManagement.Infrastructure.ExampleService;

public class ExampleProfile : Profile
{
    public ExampleProfile()
    {
        CreateMap<ExampleEntity, Example>()
            .ReverseMap();
    }
}
