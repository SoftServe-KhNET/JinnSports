using JinnSports.BLL.Dtos;
using JinnSports.BLL.Dtos.Charts;

namespace JinnSports.BLL.Interfaces
{
    public interface IChartService
    {
        GoogleVisualizationDataTable GetDataTableForTeam(int id);
    }
}