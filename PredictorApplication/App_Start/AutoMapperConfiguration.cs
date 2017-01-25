using AutoMapper;
using PredictorDTO;
using ScorePredictor.EventData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PredictorApplication
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<Prediction, PredictionDTO>()
                .ForMember(
                    e => e.IncomingEventId,
                    opt => opt.MapFrom(
                        s => s.IncomingEventId))
                .ForMember(
                    e => e.HomeTeamId,
                    opt => opt.MapFrom(
                        s => s.HomeTeamId))
               .ForMember(
                    e => e.AwayTeamId,
                    opt => opt.MapFrom(
                        s => s.AwayTeamId))
               .ForMember(
                    e => e.HomeWinProbability,
                    opt => opt.MapFrom(
                        s => s.HomeWinProbability))
               .ForMember(
                    e => e.AwayWinProbability,
                    opt => opt.MapFrom(
                        s => s.AwayWinProbability))
               .ForMember(
                    e => e.DrawProbability,
                    opt => opt.MapFrom(
                        s => s.DrawProbability));

                config.CreateMap<IncomingEventDTO, IncomingEvent>()
                .ForMember(
                    e => e.Id,
                    opt => opt.MapFrom(
                        s => s.Id))
                .ForMember(
                    e => e.SportType,
                    opt => opt.MapFrom(
                        s => s.SportType))
               .ForMember(
                    e => e.TeamsInfo,
                    opt => opt.MapFrom(
                        s => s.TeamsInfo));

                config.CreateMap<TeamInfoDTO, TeamInfo>()
                .ForMember(
                    e => e.TeamId,
                    opt => opt.MapFrom(
                        s => s.TeamId))
                .ForMember(
                    e => e.IsHomeGame,
                    opt => opt.MapFrom(
                        s => s.IsHomeGame))
               .ForMember(
                    e => e.TeamEvents,
                    opt => opt.MapFrom(
                        s => s.TeamEvents));

                config.CreateMap<TeamEventDTO, TeamEvent>()
                .ForMember(
                    e => e.IsHomeGame,
                    opt => opt.MapFrom(
                        s => s.IsHomeGame))
                .ForMember(
                    e => e.AttackScore,
                    opt => opt.MapFrom(
                        s => s.AttackScore))
               .ForMember(
                    e => e.DefenseScore,
                    opt => opt.MapFrom(
                        s => s.DefenseScore))
               .ForMember(
                    e => e.Date,
                    opt => opt.MapFrom(
                        s => new DateTime(s.Date)));
            });
        }
    }
}