using System;
using System.Collections.Generic;

using SAHL.Core.BusinessModel;
using SAHL.Core.Data;
using SAHL.UI.Halo.Shared.Configuration;

namespace SAHL.UI.Halo.Configuration.Account.MortgageLoan.Actions
{
    public class MortgageLoanDynamicActionProvider : IHaloTileDynamicActionProvider
    {
        public IEnumerable<IHaloTileAction> GetTileActions(BusinessContext businessContext)
        {
            var dynamicActions = new List<IHaloTileAction>();
            dynamicActions.Add(new MortgageLoanCloseWizardAction());
            return dynamicActions;
        }
    }
}
