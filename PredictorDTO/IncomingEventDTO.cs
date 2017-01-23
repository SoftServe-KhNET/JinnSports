using System.Collections.Generic;

namespace PredictorDTO
{
    public class IncomingEventDTO
    {
        public int Id { get; set; }
        public string SportType { get; set; }
        public IEnumerable<TeamInfoDTO> TeamsInfo { get; set; }
    }
}
