using AutoMapper;
using PredictorBalancer.Models;
using PredictorBalancer.ViewModels;
using PredictorDTO;
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

                config.CreateMap<PackageDTO, Package>()
                .ForMember(
                    e => e.CallBackURL,
                    opt => opt.MapFrom(
                        s => s.CallBackURL))
               .ForMember(
                    e => e.CallBackController,
                    opt => opt.MapFrom(
                        s => s.CallBackController))
                .ForMember(
                    e => e.CallBackTimeout,
                    opt => opt.MapFrom(
                        s => s.CallBackTimeout));
            });
        }
    }
}