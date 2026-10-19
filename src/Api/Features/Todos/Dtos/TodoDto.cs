namespace Poucno.Backend.Api.Features.Todos.Dtos;

public record TodoDto(
    Guid Id,
    string Title,
    bool IsCompleted
);
