using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace JinnSports.BLL.Dtos.Charts
{
    public class GoogleVisualizationDataTable
    {
        [JsonProperty("cols")]
        public IList<Col> Cols { get; } = new List<Col>();
        [JsonProperty("rows")]
        public IList<Row> Rows { get; } = new List<Row>();

        public void AddColumn(string label, string type)
        {
            this.Cols.Add(new Col { Label = label, Type = type });
        }

        public void AddRow(IList<object> values)
        {
            this.Rows.Add(new Row { C = values.Select(x => new Row.RowValue { V = x }) });
        }

        public class Col
        {
            [JsonProperty("label")]
            public string Label { get; set; }
            [JsonProperty("type")]
            public string Type { get; set; }
        }

        public class Row
        {
            [JsonProperty("c")]
            public IEnumerable<RowValue> C { get; set; }
            public class RowValue
            {
                [JsonProperty("v")]
                public object V { get; set; }
            }
        }
    }
}