using System.Collections.Generic;
using System.Linq;
using JinnSports.BLL.Dtos.Charts;
using JinnSports.BLL.Interfaces;
using JinnSports.DataAccessInterfaces.Interfaces;
using JinnSports.Entities.Entities;
using log4net;

namespace JinnSports.BLL.Service
{
    public class ChartService : IChartService
    {
        private static readonly ILog Log = LogManager.GetLogger("AppLog");

        private readonly IUnitOfWork unitOfWork;

        public ChartService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public GoogleVisualizationDataTable GetDataTableForTeam(int id)
        {
            GoogleVisualizationDataTable dataTable = new GoogleVisualizationDataTable();

            dataTable.AddColumn("Date", "date");
            dataTable.AddColumn("WinRate", "number");

            // Get existing results for team ordered by date
            var results = this.unitOfWork.GetRepository<Team>().GetById(id).Results
                .Where(r => r.Score != -1)
                .OrderBy(result => result.SportEvent.Date)
                .ToList();
            int winGamesCounter = 0;
            int totalGamesCounter = 0;

            foreach (var result in results)
            {
                totalGamesCounter++;
                IList<object> row = new List<object>(2);
                row.Add(result.SportEvent.Date);

                var winners = result.SportEvent.Results.Where(r => r.Score == result.SportEvent.Results.Max(mr => mr.Score)).ToList();

                int winrate = 0;
                // If draw - adding last winrate
                if (winners.Count > 1)
                {
                    winrate = int.Parse(dataTable.Rows[dataTable.Rows.Count - 1].C.ElementAt(1).V.ToString());
                    totalGamesCounter--;
                }
                else if (winners.Count == 1 && winners[0].Team.Id == id)
                {
                    winGamesCounter++;
                    winrate = winGamesCounter * 100 / totalGamesCounter;
                }
                else
                {
                    winrate = winGamesCounter * 100 / totalGamesCounter;
                }
                row.Add(winrate);

                dataTable.AddRow(row);
            }

            return dataTable;
        }
    }
}