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
            if (InputName == null || ExistedName == null)
            {
                return false;
            }

            if ((object)conformity == null || conformity.InputName == null || conformity.ExistedName == null)
            {
                return false;
            }

            return (InputName == conformity.InputName) && (ExistedName == conformity.ExistedName) && (IsConfirmed == conformity.IsConfirmed);
        }

        public override int GetHashCode()
        {
            if (InputName == null || ExistedName == null || TempResult == null)
            {
                return 0;
            }

            int hashCode = InputName.GetHashCode() ^ ExistedName.GetHashCode() ^ IsConfirmed.GetHashCode() ^ TempResult.GetHashCode();
            
            return hashCode;
        }
    }
}
