using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.DAO;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.Base;

namespace SAHL.Common.BusinessModel
{
    public partial class CreditCriteria : BusinessModelBase<SAHL.Common.BusinessModel.DAO.CreditCriteria_DAO>, ICreditCriteria
	{
	}
}


