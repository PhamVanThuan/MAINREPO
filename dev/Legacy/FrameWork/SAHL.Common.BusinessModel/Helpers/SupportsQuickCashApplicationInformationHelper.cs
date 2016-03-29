using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using Castle.ActiveRecord.Queries;
using Castle.ActiveRecord;
using SAHL.Common.Exceptions;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.Security;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;

namespace SAHL.Common.BusinessModel.Helpers
{
    public static class SupportsQuickCashApplicationInformationHelper
    {
        internal static IApplicationInformationQuickCash ApplicationInformationQuickCash(IApplicationInformation ApplicationInfo)
        {
            object dao_obj = (ApplicationInfo as IDAOObject).GetDAOObject();
            if (dao_obj != null)
            {
                ApplicationInformation_DAO dao = dao_obj as ApplicationInformation_DAO; 
                if (dao != null)
                {
                    IBusinessModelTypeMapper BMTM = TypeFactory.CreateType<IBusinessModelTypeMapper>();
                    return BMTM.GetMappedType<IApplicationInformationQuickCash, ApplicationInformationQuickCash_DAO>(dao.ApplicationInformationQuickCash);
                }
            }
            return null;
        }
    }
}
