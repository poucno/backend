using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Poucno.Backend.Api.Data;
using Poucno.Backend.Api.Features.Todos.Dtos;

namespace Poucno.Backend.Api.Features.Todos.Queries;

public record GetTodoByIdQuery(Guid Id) : IRequest<TodoDto?>;

public class GetTodoByIdQueryHandler(AppDbContext db, IMapper mapper) : IRequestHandler<GetTodoByIdQuery, TodoDto?>
{
    public async Task<TodoDto?> Handle(GetTodoByIdQuery request, CancellationToken ct)
    {
        return await mapper
            .ProjectTo<TodoDto>(
                db.TodoItems
                    .AsNoTracking()
                    .Where(todo => todo.Id == request.Id))
            .FirstOrDefaultAsync(ct);
    }
}
