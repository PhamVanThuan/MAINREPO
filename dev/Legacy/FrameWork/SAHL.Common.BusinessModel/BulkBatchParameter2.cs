using System.Collections.Generic;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel
{
    /// <summary>
    ///
    /// </summary>
    public partial class BulkBatchParameter : BusinessModelBase<SAHL.Common.BusinessModel.DAO.BulkBatchParameter_DAO>, IBulkBatchParameter
    {
        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);
            Rules.Add("BulkBatchParameterArrearBalanceMultiple");
        }
    }
}