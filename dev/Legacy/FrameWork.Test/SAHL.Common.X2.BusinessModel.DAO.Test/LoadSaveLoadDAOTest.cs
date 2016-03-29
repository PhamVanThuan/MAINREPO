using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using SAHL.Test;
using SAHL.Common.X2.BusinessModel;
using SAHL.Common.Security;

using Castle.ActiveRecord;
using NUnit.Framework;
using System.Security.Principal;
using SAHL.Common.X2.BusinessModel.DAO;
using System.Reflection;
using Castle.ActiveRecord.Framework.Internal;
using Castle.ActiveRecord.Framework.Config;
using System.Collections;
using System.Diagnostics;

namespace SAHL.Common.BusinessModel.X2.Test
{
    [TestFixture]
    public class LoadSaveLoadDAOTest : TestBase
    {
        [Test]
        public void LoadSaveLoadDAO()
        {
            List<String> Messages = new List<string>();
            List<String> StackTrace = new List<string>();
            List<Type> DAOTypes = new List<Type>();
            DAOTypes.Add(typeof(SAHL.Common.X2.BusinessModel.DAO.ActivityType_DAO));
            DAOTypes.Add(typeof(SAHL.Common.X2.BusinessModel.DAO.ExternalActivity_DAO));
            DAOTypes.Add(typeof(SAHL.Common.X2.BusinessModel.DAO.ExternalActivityTarget_DAO));
            DAOTypes.Add(typeof(SAHL.Common.X2.BusinessModel.DAO.Form_DAO));
            DAOTypes.Add(typeof(SAHL.Common.X2.BusinessModel.DAO.Instance_DAO));
            DAOTypes.Add(typeof(SAHL.Common.X2.BusinessModel.DAO.InstanceActivitySecurity_DAO));
            DAOTypes.Add(typeof(SAHL.Common.X2.BusinessModel.DAO.Log_DAO));
            DAOTypes.Add(typeof(SAHL.Common.X2.BusinessModel.DAO.Process_DAO));
            DAOTypes.Add(typeof(SAHL.Common.X2.BusinessModel.DAO.SecurityGroup_DAO));
            DAOTypes.Add(typeof(SAHL.Common.X2.BusinessModel.DAO.StageActivity_DAO));
            DAOTypes.Add(typeof(SAHL.Common.X2.BusinessModel.DAO.State_DAO));
            DAOTypes.Add(typeof(SAHL.Common.X2.BusinessModel.DAO.StateType_DAO));
            DAOTypes.Add(typeof(SAHL.Common.X2.BusinessModel.DAO.WorkFlow_DAO));
            DAOTypes.Add(typeof(SAHL.Common.X2.BusinessModel.DAO.WorkFlowActivity_DAO));
            DAOTypes.Add(typeof(SAHL.Common.X2.BusinessModel.DAO.WorkFlowHistory_DAO));
            DAOTypes.Add(typeof(SAHL.Common.X2.BusinessModel.DAO.WorkFlowIcon_DAO));
            DAOTypes.Add(typeof(SAHL.Common.X2.BusinessModel.DAO.WorkList_DAO));
            DAODataConsistancyChecker.CheckTypes(DAOTypes, "ID");
        }
    }

    
}
