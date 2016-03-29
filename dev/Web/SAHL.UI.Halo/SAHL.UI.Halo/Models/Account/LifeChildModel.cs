using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Models.Account
{
    public class LifeChildModel : IHaloTileModel
    {
        public string AccountNumber { get; set; }
        public bool IsInArrears { get; set; }
        public bool IsInAdvance { get; set; }
        public bool IsNonPerforming { get; set; }
        public string AccountStatus { get; set; }
        public string PolicyNumber { get; set; }
        public string PolicyType { get; set; }
        public string PolicyStatus { get; set; }
        public double CurrentSumAssured { get; set; }
    }
}
