﻿namespace JinnSports.BLL.Dtos.Charts
{
    public class WinRateChartDto
    {
        public string Title { get; set; }

        public string Subtitle { get; set; }

        public GoogleVisualizationDataTable DataTable { get; set; }
    }
}