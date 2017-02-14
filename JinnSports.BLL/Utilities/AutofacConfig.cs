using Autofac;
using JinnSports.BLL.Interfaces;
using JinnSports.BLL.Service;
using JinnSports.DataAccessInterfaces.Interfaces;
using JinnSports.DAL.EFContext;
using JinnSports.DAL.Repositories;

namespace JinnSports.BLL.Utilities
{
    public class AutofacConfig
    {
        public static void Configure(ref ContainerBuilder builder)
        {
            // Data access config
            builder.Register(db => new SportsContext("SportsContext")).InstancePerLifetimeScope();
            builder.RegisterType<EFUnitOfWork>().As<IUnitOfWork>();
            // Services config
            builder.RegisterType<EventsService>().As<IEventService>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<AdminService>().As<IAdminService>();
            builder.RegisterType<TeamService>().As<ITeamService>();
            builder.RegisterType<TeamDetailsService>().As<ITeamDetailsService>();
            builder.RegisterType<SportTypeService>().As<ISportTypeService>();
            builder.RegisterType<NewsService>().As<INewsService>();
            builder.RegisterType<ChartService>().As<IChartService>();
            builder.RegisterType<LoggerService>().As<ILoggerService>();
            builder.RegisterType<PredictoionSender>().As<PredictoionSender>();
            builder.RegisterType<PredictionsService>().As<PredictionsService>();
        }
    }
}
