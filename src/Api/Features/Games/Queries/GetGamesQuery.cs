using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using Poucno.Backend.Api.Features.Games.Dtos;

namespace Poucno.Backend.Api.Features.Games.Queries;

public record GetGamesQuery : IRequest<List<GameDto>>;

public class GetGamesQueryHandler(IOptions<GamesOptions> options, IMapper mapper)
    : IRequestHandler<GetGamesQuery, List<GameDto>>
{
    public Task<List<GameDto>> Handle(GetGamesQuery request, CancellationToken ct)
    {
        var games = mapper.Map<List<GameDto>>(options.Value.Items);

        return Task.FromResult(games);
    }
}
