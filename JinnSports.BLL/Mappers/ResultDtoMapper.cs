using JinnSports.BLL.Dtos;
using JinnSports.Entities.Entities;

namespace JinnSports.BLL.Mappers
{
    internal static class ResultDtoMapper
    {
        public static ResultDto MapToResultDto(this Result result, Result result2)
        {
            return new ResultDto
            {
                Date = result.CompetitionEvent.Date.ToShortDateString(),
                Score = $"{result.Score}:{result2.Score}",
                Team1 = result.Team.Name,
                Team2 = result2.Team.Name,
                Team1Id = result.Team.Id,
                Team2Id = result2.Team.Id
            };
        }
    }
}
