using JinnSports.BLL.Dtos;
using JinnSports.Entities.Entities;
using JinnSports.WEB.Models;

namespace JinnSports.WEB.Mappers
{
    internal static class ResultMapper
    {
        public static ResultViewModel MapToViewModel(this CompetitionEventDto compEvent)
        {
            return new ResultViewModel
            {
                Result = string.Concat(
                    compEvent.Team1, 
                    " ", 
                    compEvent.Result1, 
                    ":", 
                    compEvent.Result2, 
                    " ", 
                    compEvent.Team2),
                Date = compEvent.Date.ToShortDateString()
            };
        }

        public static ResultViewModel MapToViewModel(this Result result)
        {
            return new ResultViewModel
            {
                Date = result.CompetitionEvent.Date.ToShortDateString(),
                Result = $"{result.Team.Name} - {result.Score}"
            };
        }

        public static ResultDetailsViewModel MapToResultDetailsViewModel(this ResultDto result)
        {
            return new ResultDetailsViewModel
            {
                Score = result.Score,
                Team1 = result.Team1,
                Team1Id = result.Team1Id,
                Team2 = result.Team2,
                Team2Id = result.Team2Id
            };
        }
    }
}