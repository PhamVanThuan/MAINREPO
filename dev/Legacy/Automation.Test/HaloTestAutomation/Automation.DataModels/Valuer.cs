using Common.Enums;
using System;

namespace Automation.DataModels
{
    public class Valuer : IComparable<Valuer>
    {
        public int ValuatorKey { get; set; }

        public string ValuatorContact { get; set; }

        public string ValuatorPassword { get; set; }

        public byte LimitedUserGroup { get; set; }

        public GeneralStatusEnum GeneralStatusKey { get; set; }

        public int LegalEntityKey { get; set; }

        public Automation.DataModels.LegalEntity LegalEntity { get; set; }

        public int CompareTo(Valuer other)
        {
            throw new NotImplementedException();
        }
    }
}