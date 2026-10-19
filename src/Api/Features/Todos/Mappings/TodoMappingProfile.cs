using AutoMapper;
using Poucno.Backend.Api.Data.Entities;
using Poucno.Backend.Api.Features.Todos.Dtos;

namespace Poucno.Backend.Api.Features.Todos.Mappings;

public class TodoMappingProfile : Profile
{
    public TodoMappingProfile()
    {
        CreateMap<TodoItem, TodoDto>();
    }
}
