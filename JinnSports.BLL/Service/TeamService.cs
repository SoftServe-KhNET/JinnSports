using System.Collections.Generic;
using System.Linq;
using JinnSports.BLL.Exceptions;
using JinnSports.BLL.Interfaces;
using JinnSports.BLL.Mappers;
using JinnSports.DataAccessInterfaces.Interfaces;
using JinnSports.DAL.Repositories;
using JinnSports.Entities.Entities;
using JinnSports.BLL.Dtos;

namespace JinnSports.BLL.Service
{
    public class TeamService : ITeamService
    {
        private readonly IUnitOfWork dataUnit;
        private readonly IRepository<Team> teamsRepository;
        private readonly IRepository<Result> resultsRepository;
        private readonly IRepository<CompetitionEvent> competitionsEventsRepository;

        public TeamService()
        {
            this.dataUnit = new EFUnitOfWork("SportsContext");
            this.teamsRepository = this.dataUnit.Set<Team>();
            this.resultsRepository = this.dataUnit.Set<Result>();
            this.competitionsEventsRepository = this.dataUnit.Set<CompetitionEvent>();
        }

        public IEnumerable<TeamDto> GetAllTeams()
        {
            List<TeamDto> teamDtos = new List<TeamDto>();
            IEnumerable<Team> teams = this.teamsRepository.GetAll();
            var teamsList = teams as IList<Team> ?? teams.ToList();

            if (!teamsList.Any())
            {
                throw new TeamNotFoundException("Teams are not exist.");
            }

            foreach (var team in teamsList)
            {
                teamDtos.Add(team.MapToTeamDto());
            }

            return teamDtos;
        }

        public TeamDetailsDto GetTeamDetailsById(int id)
        {
            Team team = this.teamsRepository.GetById(id);

            if (team == null)
            {
                throw new TeamNotFoundException($"Team with id '{id}' does not exists.");
            }

            List<CompetitionEvent> events = new List<CompetitionEvent>();
            events.AddRange(team
                .Results
                .Select(s => s.CompetitionEvent));
            List<Result> results = this.resultsRepository
                .GetAll()
                .Where(s => events.Contains(s.CompetitionEvent))
                .ToList();
            TeamDetailsDto teamDetails = new TeamDetailsDto
            {
                Id = team.Id,
                Name = team.Name,
                Results = new List<ResultDto>()
            };
            
            foreach (var result in results)
            {
                teamDetails.Results.Add(result
                    .MapToResultDto(results
                        .SingleOrDefault(r => object.Equals(r.CompetitionEvent, result.CompetitionEvent) && r.Team != result.Team)));
            }

            return teamDetails;
        }

        public void Dispose()
        {
            this.dataUnit.Dispose();
        }
    }
}
