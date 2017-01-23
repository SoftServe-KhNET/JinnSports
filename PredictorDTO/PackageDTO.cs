using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredictorDTO
{
    public class PackageDTO
    {
        public string CallBackURL { get; set; }
        public string CallBackController { get; set; }
        public int CallBackTimeout { get; set; }

        public IEnumerable<IncomingEventDTO> IncomigEvents { get; set; }
    }
}
