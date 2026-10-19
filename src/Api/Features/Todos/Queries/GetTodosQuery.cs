using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Poucno.Backend.Api.Data;
using Poucno.Backend.Api.Features.Todos.Dtos;

namespace Poucno.Backend.Api.Features.Todos.Queries;

public record GetTodosQuery : IRequest<List<TodoDto>>;

public class GetTodosQueryHandler(AppDbContext db, IMapper mapper) : IRequestHandler<GetTodosQuery, List<TodoDto>>
{
    public async Task<List<TodoDto>> Handle(
        GetTodosQuery request,
        CancellationToken ct)
    {
        return await mapper
            .ProjectTo<TodoDto>(
                db.TodoItems
                    .AsNoTracking()
                    .OrderBy(todo => todo.Title))
            .ToListAsync(ct);
    }
}
