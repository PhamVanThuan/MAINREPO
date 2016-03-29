using System;
using System.Text;
using System.Collections.Generic;
namespace SAHL.Test
{
  public class DAOX2
  {
    public static List<Type> DAOTypes()
    {
      List<Type> DAOTypes = new List<Type>();
      DAOTypes.Add(typeof(SAHL.Common.X2.BusinessModel.DAO.ActiveExternalActivity_DAO));
      DAOTypes.Add(typeof(SAHL.Common.X2.BusinessModel.DAO.Activity_DAO));
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
      return DAOTypes;
    }
  }
}

