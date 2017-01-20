using JinnSports.Entities.Entities.Temp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JinnSports.Entities.Entities
{
    public class Conformity
    {
        public int Id { get; set; }

        public string InputName { get; set; }

        public string ExistedName { get; set; }

        public bool IsConfirmed { get; set; }

        public TempResult TempResult { get; set; }

        public bool Equals(Conformity conformity)
        {
            if (this.InputName == null || this.ExistedName == null)
            {
                return false;
            }

            if ((object)conformity == null || conformity.InputName == null || conformity.ExistedName == null)
            {
                return false;
            }

            return (this.InputName == conformity.InputName) && 
                (this.ExistedName == conformity.ExistedName) && 
                (this.IsConfirmed == conformity.IsConfirmed);
        }

        public override int GetHashCode()
        {
            if (this.InputName == null || this.ExistedName == null || this.TempResult == null)
            {
                return 0;
            }

            int hashCode = this.InputName.GetHashCode() ^ 
                this.ExistedName.GetHashCode() ^ 
                this.IsConfirmed.GetHashCode() ^ 
                this.TempResult.GetHashCode();
            
            return hashCode;
        }
    }
}
