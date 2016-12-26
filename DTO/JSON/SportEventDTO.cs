using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DTO.JSON
{
    
    public class SportEventDTO
    {
        public long Date { get; set; }

        public string SportType { get; set; }

        public ICollection<ResultDTO> Results { get; set; } = new List<ResultDTO>();
    }
}
