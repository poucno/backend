using AutoMapper;
using Poucno.Backend.Api.Data.Entities;
using Poucno.Backend.Api.Features.Collections.Dtos;

namespace Poucno.Backend.Api.Features.Collections.Mappings;

public class CollectionMappingProfile : Profile
{
    public CollectionMappingProfile()
    {
        CreateMap<CollectionQuestion, QuestionDto>()
            .ForCtorParam(nameof(QuestionDto.Question), opt => opt.MapFrom(q => q.Text))
            .ForCtorParam(nameof(QuestionDto.Answer), opt => opt.MapFrom(q => q.CorrectAnswer));

        CreateMap<Collection, CollectionDto>()
            .ForMember(dto => dto.Questions, opt => opt.MapFrom(c => c.Questions.OrderBy(q => q.Order)));

        CreateMap<Collection, CollectionSummaryDto>()
            .ForCtorParam(nameof(CollectionSummaryDto.QuestionCount), opt => opt.MapFrom(c => c.Questions.Count));
    }
}
