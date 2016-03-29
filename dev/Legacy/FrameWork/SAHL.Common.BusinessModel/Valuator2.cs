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
	public partial class Valuator : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Valuator_DAO>, IValuator
	{
         protected void OnOriginationSources_BeforeAdd(ICancelDomainArgs args, object Item)
        {

        }

         protected void OnOriginationSources_BeforeRemove(ICancelDomainArgs args, object Item)
        {

        }

         protected void OnOriginationSources_AfterAdd(ICancelDomainArgs args, object Item)
        {

        }

         protected void OnOriginationSources_AfterRemove(ICancelDomainArgs args, object Item)
        {

        }
    }

    

}


