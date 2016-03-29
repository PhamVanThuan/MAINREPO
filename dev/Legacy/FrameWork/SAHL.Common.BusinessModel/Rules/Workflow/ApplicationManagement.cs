using System.Collections.Generic;
using System.Text;
using SAHL.Common.Service.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using Castle.ActiveRecord.Queries;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.Collections.Interfaces;
using System;
using SAHL.Common.DataAccess;
using System.Data;

namespace SAHL.Common.BusinessModel.Rules.Workflow
{
    /// <summary>
    /// Before a case can move to the disbursed state the following must be true. 
    /// For New Business (Offer Type 6,7,8) App must be accepted and account open
    /// </summary>
    [RuleDBTag("CheckCanMoveToDisbursement",
        "",
        "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.Workflow.CheckCanMoveToDisbursement")]
    [RuleInfo]
    public class CheckCanMoveToDisbursement : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (!(Parameters[0] is IApplication))
                throw new ArgumentException("Parameters[0] is not of type IApplication.");
            IApplication app = Parameters[0] as IApplication;
            if (app.ApplicationType.Key == 6 || app.ApplicationType.Key == 7 || app.ApplicationType.Key == 8)
            {
                // Check that the application status has been set to accepted.
                if (app.ApplicationStatus.Key != 3)
                {
                     AddMessage("Application Status is not Accepted", "",Messages);
                    return 0;
                }
                // Check the account has been opened.
                if (app.Account.AccountStatus.Key != 1)
                {
                     AddMessage("Account Status is not Open", "", Messages);
                    return 0;
                }
            }
            return 1;
        }
    }

    [RuleDBTag("CheckEWorkInResubmitted",
    "",
    "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.Workflow.CheckEWorkInResubmitted")]
    [RuleInfo]
    public class CheckEWorkInResubmitted : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (!(Parameters[0] is IApplication))
                throw new ArgumentException("Parameters[0] is not of type IApplication.");
            IApplication app = Parameters[0] as IApplication;
            IDbConnection conn = Helper.GetSQLDBConnection("X2");
            string SQL = string.Format("select EWorkFolderID from x2.x2data.Application_Management where applicationkey={0}", app.Key);
            DataTable dt = new DataTable();
            Helper.FillFromQuery(dt, SQL, conn, new ParameterCollection());
            if (dt.Rows.Count > 0 && dt.Rows[0][0] != null)
            {
                string EFolderID = dt.Rows[0][0].ToString();
                if (!string.IsNullOrEmpty(EFolderID))
                {
                    dt = new DataTable();
                    SQL = string.Format("select estagename from [e-work]..efolder ef where ef.efolderid='{0}'", EFolderID);
                    Helper.FillFromQuery(dt, SQL, conn, new ParameterCollection());
                    if (dt.Rows.Count > 0 && dt.Rows[0][0] != null)
                    {
                        if (dt.Rows[0][0].ToString() == "Resubmitted")
                            return 0;
                        else
                        {
                            AddMessage(string.Format("Case must be at the 'Resubmitted' stage in Pipeline. It is at {0}",dt.Rows[0][0]), "", Messages);
                            return 3;
                        }
                    }
                    else
                    {
                        AddMessage("Case must be at the 'Resubmitted' case in Pipeline.", "", Messages);
                        return 2;
                    }
                }
            }
            AddMessage("Unable to get EFolderID for Application", "", Messages);
            return 1;
        }
    }
}
