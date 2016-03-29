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
	/// SAHL.Common.BusinessModel.DAO.SuperLo_DAO
	/// </summary>
	public partial class SuperLo : BusinessModelBase<SAHL.Common.BusinessModel.DAO.SuperLo_DAO>, ISuperLo
	{
        public double Balance
        {
            get
            {
                if (null == _DAO.FinancialServiceAttribute) return 0;
                else
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IBalance, Balance_DAO>(_DAO.FinancialServiceAttribute.FinancialService.Balance).Amount;
                }
            }
        }
    }
}


