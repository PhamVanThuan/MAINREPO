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
	public partial class ITC : BusinessModelBase<SAHL.Common.BusinessModel.DAO.ITC_DAO>, IITC
	{
        public override void OnPopulateRules(List<string> Rules)
        {
            Rules.Add("ITCRequestLegalEntityNaturalPersonAddress");
            Rules.Add("ITCRequestLegalEntityNaturalPersonIDNumber");
            Rules.Add("ITCRequestLegalEntityNaturalPersonIDNumberForeigner");
            Rules.Add("ITCRequestLegalEntityNaturalPerson");
            Rules.Add("ITCRequestFrequency");
        }
	}
}


