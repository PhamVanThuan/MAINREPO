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
	public partial class ValuationOutbuilding : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ValuationOutbuilding_DAO>, IValuationOutbuilding
	{
        public override void OnPopulateRules(List<string> Rules)
        {
            base.OnPopulateRules(Rules);

            Rules.Add("ManualValuationOutbuildingRoof");
            Rules.Add("ManualValuationOutbuildingExtent");
            Rules.Add("ManualValuationOutbuildingRate");
        }
	}
}


