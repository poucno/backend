using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Poucno.Backend.Api.Data;
using Poucno.Backend.Api.Features.Collections.Dtos;

namespace Poucno.Backend.Api.Features.Collections.Queries;

public record GetCollectionByIdQuery(Guid Id) : IRequest<CollectionDto?>;

public class GetCollectionByIdQueryHandler(AppDbContext db, IMapper mapper)
    : IRequestHandler<GetCollectionByIdQuery, CollectionDto?>
{
    public async Task<CollectionDto?> Handle(
        GetCollectionByIdQuery request,
        CancellationToken ct)
    {
        return await mapper
            .ProjectTo<CollectionDto>(
                db.Collections
                    .AsNoTracking()
                    .Where(c => c.Id == request.Id))
            .FirstOrDefaultAsync(ct);
    }
}
