using SAHL.Services.Interfaces.LifeDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Workflow.LifeClaims.Specs.Factories.Models
{
    public class DisabilityClaimModels
    {
        public static DisabilityClaimDetailModel GetEmptyDisabilityClaimDetailModel()
        {
            return new DisabilityClaimDetailModel(0, 0, 0, 0, String.Empty, DateTime.Now, null, null, String.Empty, null, String.Empty, String.Empty, null, 0, String.Empty, null, null, null);
        }
    }
}
