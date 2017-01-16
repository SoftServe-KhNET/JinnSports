using System.Collections.Generic;
using AutoMapper;
using JinnSports.BLL.Dtos;
using JinnSports.Entities.Entities;
using System.Linq;
using JinnSports.BLL.Extentions;
using JinnSports.Entities.Entities.Identity;

namespace JinnSports.WEB
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<Result, ResultDto>()
               .ForMember(
                     e => e.Id,
                     opt => opt.MapFrom(
                         res => res.Id))
               .ForMember(
                    e => e.Score,
                    opt => opt.MapFrom(
                        res => string.Format(
                            "{0} : {1}",
                            res.SportEvent.Results.ElementAt(0).Score,
                            res.SportEvent.Results.ElementAt(1).Score)))
               .ForMember(
                    e => e.TeamNames,
                    opt => opt.MapFrom(
                        res => res.SportEvent.Results.Select(x => x.Team.Name)))
               .ForMember(
                     e => e.TeamIds,
                     opt => opt.MapFrom(
                         res => res.SportEvent.Results.Select(x => x.Team.Id)))
               .ForMember(
                     e => e.Date,
                     opt => opt.MapFrom(
                         res => new EventDate(res.SportEvent.Date).ToString()));

                config.CreateMap<SportEvent, ResultDto>()
                .ForMember(
                    e => e.Id,
                    opt => opt.MapFrom(
                        s => s.Id))
               .ForMember(
                    e => e.TeamNames,
                    opt => opt.MapFrom(
                        s => s.Results.Select(x => x.Team.Name)))
               .ForMember(
                     e => e.TeamIds,
                     opt => opt.MapFrom(
                         res => res.Results.Select(x => x.Team.Id)))
               .ForMember(
                    e => e.Score,
                    opt => opt.MapFrom(
                        res => string.Format(
                            "{0} : {1}",
                            res.Results.ElementAt(0).Score,
                            res.Results.ElementAt(1).Score)))
                .ForMember(
                    e => e.Date,
                    opt => opt.MapFrom(
                        res => new EventDate(res.Date).ToString()));

                config.CreateMap<Team, TeamDto>()
                    .ForMember(
                        e => e.Id,
                        opt => opt.MapFrom(
                            s => s.Id))
                   .ForMember(
                        e => e.Name,
                        opt => opt.MapFrom(
                            s => s.Name));

                config.CreateMap<SportType, SportTypeDto>()
                    .ForMember(
                        e => e.Id,
                        opt => opt.MapFrom(
                            s => s.Id))
                   .ForMember(
                        e => e.Name,
                        opt => opt.MapFrom(
                            s => s.Name));

                config.CreateMap<UserDto, User>()
                    .ForMember(
                        e => e.UserId,
                        opt => opt.MapFrom(
                            s => s.Id))
                   .ForMember(
                        e => e.UserName,
                        opt => opt.MapFrom(
                            s => s.UserName))
                   .ForMember(
                        e => e.PasswordHash,
                        opt => opt.MapFrom(
                            s => s.PasswordHash))
                   .ForMember(
                        e => e.SecurityStamp,
                        opt => opt.MapFrom(
                            s => s.SecurityStamp));

                config.CreateMap<User, UserDto>()
                    .ForMember(
                        e => e.Id,
                        opt => opt.MapFrom(
                            s => s.UserId))
                   .ForMember(
                        e => e.UserName,
                        opt => opt.MapFrom(
                            s => s.UserName))
                   .ForMember(
                        e => e.PasswordHash,
                        opt => opt.MapFrom(
                            s => s.PasswordHash))
                   .ForMember(
                        e => e.SecurityStamp,
                        opt => opt.MapFrom(
                            s => s.SecurityStamp));

                config.CreateMap<Role, RoleDto>()
                    .ForMember(
                        e => e.Id,
                        opt => opt.MapFrom(
                            s => s.RoleId))
                   .ForMember(
                        e => e.Name,
                        opt => opt.MapFrom(
                            s => s.Name));

                config.CreateMap<RoleDto, Role>()
                    .ForMember(
                        e => e.RoleId,
                        opt => opt.MapFrom(
                            s => s.Id))
                   .ForMember(
                        e => e.Name,
                        opt => opt.MapFrom(
                            s => s.Name));
            });
        }
    }
}