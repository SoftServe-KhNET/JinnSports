using AutoMapper;
using System.Linq;
using JinnSports.BLL.Dtos;
using JinnSports.Entities.Entities;
using JinnSports.BLL.Extentions;

namespace JinnSports.BLL.Mapping
{
    public static class MapperConfig
    {
        public static IMapperConfigurationExpression AddServiceMapping(
        this IMapperConfigurationExpression configurationExpression)
        {
            configurationExpression.CreateMap<Result, ResultDto>()
                .ForMember("Id", opt => opt.MapFrom(res => res.Id))
                .ForMember("Score", opt => opt.MapFrom(res => string.Format("{0} : {1}", res.SportEvent.Results.ElementAt(0).Score, res.SportEvent.Results.ElementAt(1).Score)))
                .ForMember("TeamFirst", opt => opt.MapFrom(res => res.SportEvent.Results.ElementAt(0).Team.Name))
                .ForMember("TeamSecond", opt => opt.MapFrom(res => res.SportEvent.Results.ElementAt(1).Team.Name))
                .ForMember("TeamFirstId", opt => opt.MapFrom(res => res.SportEvent.Results.ElementAt(0).Team.Id))
                .ForMember("TeamSecondId", opt => opt.MapFrom(res => res.SportEvent.Results.ElementAt(1).Team.Id))
                .ForMember("Date", opt => opt.MapFrom(res => new EventDate(res.SportEvent.Date).ToString()));
            return configurationExpression;
        }
    }
}
