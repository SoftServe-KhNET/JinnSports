using System.Collections.Generic;

namespace PredictorDTO
{
    public class TeamInfoDTO
    {
        public int TeamId { get; set; }
        public bool IsHomeGame { get; set; }
        public IEnumerable<TeamEventDTO> TeamEvents { get; set; }
    }
}
