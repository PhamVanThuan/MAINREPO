using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Base;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Factories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
namespace SAHL.Common.BusinessModel
{
	/// <summary>
	/// 
	/// </summary>
	public partial class ValuationCottage : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ValuationCottage_DAO>, IValuationCottage
	{
        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);

            Rules.Add("ValuationCottageValuationMandatory");
            Rules.Add("ManualValuationCottageRoof");
            Rules.Add("ManualValuationCottageExtent");
            Rules.Add("ManualValuationCottageRate");
        }
	}
}


