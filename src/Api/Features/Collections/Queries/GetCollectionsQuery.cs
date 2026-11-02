using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Poucno.Backend.Api.Data;
using Poucno.Backend.Api.Features.Collections.Dtos;

namespace Poucno.Backend.Api.Features.Collections.Queries;

public record GetCollectionsQuery(string OwnerId) : IRequest<List<CollectionSummaryDto>>;

public class GetCollectionsQueryHandler(AppDbContext db, IMapper mapper)
    : IRequestHandler<GetCollectionsQuery, List<CollectionSummaryDto>>
{
    public async Task<List<CollectionSummaryDto>> Handle(
        GetCollectionsQuery request,
        CancellationToken ct)
    {
        return await mapper
            .ProjectTo<CollectionSummaryDto>(
                db.Collections
                    .AsNoTracking()
                    .Where(c => c.OwnerId == request.OwnerId)
                    .OrderByDescending(c => c.UpdatedAt))
            .ToListAsync(ct);
    }
}
