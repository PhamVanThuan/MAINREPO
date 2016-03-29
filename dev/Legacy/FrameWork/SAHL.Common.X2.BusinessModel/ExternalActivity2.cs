using SAHL.Common.Collections.Interfaces;
using SAHL.Common.X2.BusinessModel.Interfaces;
using SAHL.Common.X2.BusinessModel.Validation;

namespace SAHL.Common.X2.BusinessModel
{
    public partial class ExternalActivity : IEntityValidation, IExternalActivity, IDAOObject
    {
        protected void OnActiveExternalActivities_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnActiveExternalActivities_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnActivities_BeforeAdd(ICancelDomainArgs args, object Item)
        {
        }

        protected void OnActivities_BeforeRemove(ICancelDomainArgs args, object Item)
        {
        }
    }
}