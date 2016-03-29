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
using Castle.ActiveRecord.Queries;
using Castle.ActiveRecord;
namespace SAHL.Common.BusinessModel
{
    /// <summary>
    /// SAHL.Common.BusinessModel.DAO.Court_DAO
    /// </summary>
    public partial class Court : BusinessModelBase<SAHL.Common.BusinessModel.DAO.Court_DAO>, ICourt
    {

    }
}


