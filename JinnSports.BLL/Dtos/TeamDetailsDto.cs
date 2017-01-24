using JinnSports.BLL.Dtos.Charts;

namespace JinnSports.BLL.Dtos
{
    public class TeamDetailsDto
    {
        public GoogleVisualizationDataTable WinRateDataTable { get; set; }

        public TeamDto TeamDto { get; set; }
    }
}