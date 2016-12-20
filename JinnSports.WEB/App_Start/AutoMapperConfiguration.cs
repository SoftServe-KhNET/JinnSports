using System.Collections.Generic;
using AutoMapper;
using JinnSports.BLL.Dtos;
using JinnSports.Entities.Entities;
using System.Linq;

namespace JinnSports.WEB
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(config =>
            {
                // Конфигурация отображения SportEvent на SportEventDto
                config.CreateMap<SportEvent, SportEventDto>()
                .ForMember(
                    e => e.TeamFirst,
                    opt => opt.MapFrom(
                        s => s.Results.ElementAt(0).Team.Name))
                .ForMember(
                    e => e.TeamFirstId,
                    opt => opt.MapFrom(
                        s => s.Results.ElementAt(0).Team.Id))
                .ForMember(
                    e => e.ScoreFirst,
                    opt => opt.MapFrom(
                        s => s.Results.ElementAt(0).Score))
                .ForMember(
                    e => e.TeamSecond,
                    opt => opt.MapFrom(
                        s => s.Results.ElementAt(1).Team.Name))
                .ForMember(
                    e => e.TeamSecondId,
                    opt => opt.MapFrom(
                        s => s.Results.ElementAt(1).Team.Id))
                .ForMember(
                    e => e.ScoreSecond,
                    opt => opt.MapFrom(
                        s => s.Results.ElementAt(1).Score))
                .ForMember(
                    e => e.Date,
                    opt => opt.MapFrom(
                        s => s.Date.ToShortDateString()));
            });
        }
    }
}