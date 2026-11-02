using AutoMapper;
using Poucno.Backend.Api.Features.Games.Dtos;

namespace Poucno.Backend.Api.Features.Games.Mappings;

public class GameMappingProfile : Profile
{
    public GameMappingProfile()
    {
        CreateMap<GameOption, GameDto>();
    }
}
