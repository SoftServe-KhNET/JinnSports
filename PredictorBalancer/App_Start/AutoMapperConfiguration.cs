using AutoMapper;
using PredictorBalancer.Models;
using PredictorBalancer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PredictorBalancer.App_Start
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<Predictor, PredictorViewModel>()
                .ForMember(
                    e => e.Id,
                    opt => opt.MapFrom(
                        s => s.Id))
               .ForMember(
                    e => e.CurrentStatus,
                    opt => opt.MapFrom(
                        s => s.CurrentStatus));
            });
        }
    }
}