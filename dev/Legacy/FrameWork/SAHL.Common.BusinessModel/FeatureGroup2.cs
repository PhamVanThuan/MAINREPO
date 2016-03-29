using System;
using System.Text;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Validation;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Base;
namespace SAHL.Common.BusinessModel
{
    public partial class FeatureGroup : BusinessModelBase<SAHL.Common.BusinessModel.DAO.FeatureGroup_DAO>, IFeatureGroup
    {

        public static string[] FindAllGroups()
        {
            return FeatureGroup_DAO.FindAllGroups();
        }
    }
}


