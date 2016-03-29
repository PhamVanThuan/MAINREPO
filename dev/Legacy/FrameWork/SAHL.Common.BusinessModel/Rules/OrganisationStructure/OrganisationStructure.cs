using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using SAHL.Common.BusinessModel.DAO;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Common.Service.Interfaces;

namespace SAHL.Common.BusinessModel.Rules.OrganisationStructure
{
    [RuleDBTag("UserStatusMaintenanceAtLeastOneActiveUser",
    "Must have at least one active user",
    "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.OrganisationStructure.UserStatusMaintenanceAtLeastOneActiveUser")]
    [RuleInfo]
    public class UserStatusMaintenanceAtLeastOneActiveUser : BusinessRuleBase
    {
        public UserStatusMaintenanceAtLeastOneActiveUser(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            int offerRoleTypeKey = (Int32)Parameters[0];

            string query = UIStatementRepository.GetStatement("Rules.OrganisationStructure", "GetCountActiveADUserInOfferRole");
            ParameterCollection pc = new ParameterCollection();
            pc.Add(new SqlParameter("@OfferRoleTypeKey", offerRoleTypeKey));

            int cnt = 0;
            DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), pc);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                cnt = Convert.ToInt32(dr[0]);
            }
            if (cnt < 1)
            {
                string msg = "There needs to be at least one active user in the group";
                AddMessage(msg, msg, Messages);
            }
            return 1;
        }
    }

    [RuleDBTag("UserOrganisationStructureDistinctCheck",
    "An ADUser can only be added once to OrganisationStructure based on OrganisationType and Description.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.OrganisationStructure.UserOrganisationStructureDistinctCheck")]
    [RuleInfo]
    public class UserOrganisationStructureDistinctCheck : BusinessRuleBase
    {
        public UserOrganisationStructureDistinctCheck(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IUserOrganisationStructure uos = Parameters[0] as IUserOrganisationStructure;
            if (uos.Key == 0)
            {
                string query = UIStatementRepository.GetStatement("Rules.OrganisationStructure", "UserOrganisationStructureDistinctCheck");
                ParameterCollection pc = new ParameterCollection();
                pc.Add(new SqlParameter("@ADUserKey", uos.ADUser.Key));
                pc.Add(new SqlParameter("@Description", uos.OrganisationStructure.Description));
                pc.Add(new SqlParameter("@OrganisationTypeKey", uos.OrganisationStructure.OrganisationType.Key));
                int cnt = 0;
                DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), pc);
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        cnt = Convert.ToInt32(dr[0]);
                        if (cnt > 0)
                        {
                            string msg = string.Format("User can only exist once as {0}: {1}.", uos.OrganisationStructure.OrganisationType.Description, uos.OrganisationStructure.Description);
                            AddMessage(msg, msg, Messages);
                            break;
                        }
                    }
                }
                return 1;
            }
            else
                return 0;
        }
    }

    [RuleDBTag("UserOrganisationStructureLinkedToApplicationCheck",
    "An ADUser can only be added once to OrganisationStructure based on OrganisationType and Description.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.OrganisationStructure.UserOrganisationStructureLinkedToApplicationCheck")]
    [RuleInfo]
    public class UserOrganisationStructureLinkedToApplicationCheck : BusinessRuleBase
    {
        public UserOrganisationStructureLinkedToApplicationCheck(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IUserOrganisationStructure uos = Parameters[0] as IUserOrganisationStructure;
            DateTime endDate = DateTime.Parse(Parameters[1].ToString());

            string query = UIStatementRepository.GetStatement("Rules.OrganisationStructure", "UserOrganisationStructureLinkedToApplicationCheck");
            ParameterCollection pc = new ParameterCollection();
            pc.Add(new SqlParameter("@ADUserKey", uos.ADUser.Key));
            pc.Add(new SqlParameter("@GeneralStatusKey", (int)GeneralStatuses.Active));
            pc.Add(new SqlParameter("@OrganisationStructureKey", uos.OrganisationStructure.Key));
            pc.Add(new SqlParameter("@EndDate", endDate));
            int cnt = 0;
            string desc = string.Empty;
            DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), pc);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    cnt = Convert.ToInt32(dr[0]);
                    desc = dr[1].ToString();
                    if (cnt > 0)
                    {
                        string msg = string.Format("User cannot be removed from {0}. Actively linked to {1} application/s.", desc, cnt);
                        AddMessage(msg, msg, Messages);
                        break;
                    }
                }
            }
            return 1;
        }
    }

    [RuleDBTag("UserOrganisationStructureLinkedToActiveWorkflowRole",
    "An ADUser can only be removed from an OrganisationStructure if there are no active Workflow assignments.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.OrganisationStructure.UserOrganisationStructureLinkedToActiveWorkflowRole")]
    [RuleInfo]
    public class UserOrganisationStructureLinkedToActiveWorkflowRole : BusinessRuleBase
    {
        public UserOrganisationStructureLinkedToActiveWorkflowRole(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IUserOrganisationStructure uos = Parameters[0] as IUserOrganisationStructure;
            int workflowRoleTypeKey = Convert.ToInt32(Parameters[1]);
            string query = UIStatementRepository.GetStatement("Rules.OrganisationStructure", "UserOSLinkedToActiveWorkflowRole");
            ParameterCollection pc = new ParameterCollection();
            pc.Add(new SqlParameter("@ADUserKey", uos.ADUser.Key));
            pc.Add(new SqlParameter("@OrganisationStructureKey", uos.OrganisationStructure.Key));
            pc.Add(new SqlParameter("@WorkflowRoleTypeKey", workflowRoleTypeKey.ToString()));

            int cnt = 0;
            string desc = string.Empty;
            DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), pc);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    cnt = Convert.ToInt32(dr[0]);
                    desc = dr[1].ToString();
                    if (cnt > 0)
                    {
                        string msg = string.Format("User cannot be removed from {0}. Actively linked to {1} application/s.", desc, cnt);
                        AddMessage(msg, msg, Messages);
                        break;
                    }
                }
            }
            return 1;
        }
    }

    [RuleDBTag("UserOrganisationStructureAlreadyHasLegalEntity",
    "An Legal Entity can only be added once to OrganisationStructure.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.OrganisationStructure.UserOrganisationStructureAlreadyHasLegalEntity")]
    [RuleInfo]
    public class UserOrganisationStructureAlreadyHasLegalEntity : BusinessRuleBase
    {
        public UserOrganisationStructureAlreadyHasLegalEntity(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            int leKey = (int)Parameters[0];
            int uosKey = (int)Parameters[1];

            string query = UIStatementRepository.GetStatement("Rules.OrganisationStructure", "UserOrganisationStructureAlreadyHasLegalEntity");
            ParameterCollection pc = new ParameterCollection();
            pc.Add(new SqlParameter("@LEKey", leKey));
            pc.Add(new SqlParameter("@OrgKey", uosKey));

            DataSet ds = castleTransactionService.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), pc);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string msg = string.Format("Legal Entity is already a member of Organisation Structure .");
                    AddMessage(msg, msg, Messages);
                    return 1;
                }
            }
            return 0;
        }
    }

    [RuleDBTag("UserOrganisationStructureStartDateCheck",
    "Start date of a new designation cannot precede the offer start dates for offers linked to the related ADUser.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.OrganisationStructure.UserOrganisationStructureStartDateCheck")]
    [RuleInfo]
    public class UserOrganisationStructureStartDateCheck : BusinessRuleBase
    {
        public UserOrganisationStructureStartDateCheck(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IADUser aduser = Parameters[0] as IADUser;
            DateTime startDate = DateTime.Parse(Parameters[1].ToString());

            string query = UIStatementRepository.GetStatement("Rules.OrganisationStructure", "UserOrganisationStructureStartDateCheck");
            ParameterCollection pc = new ParameterCollection();
            pc.Add(new SqlParameter("@ADUserKey", aduser.Key));
            pc.Add(new SqlParameter("@GeneralStatusKey", (int)GeneralStatuses.Active));
            pc.Add(new SqlParameter("@OfferStartDate", startDate));
            int cnt = 0;
            string desc = string.Empty;
            DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), pc);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    cnt = Convert.ToInt32(dr[0]);
                    if (cnt > 0)
                    {
                        string msg = string.Format("StartDate cannot precede the Application StartDate. Please reassign Application/s to a manager or change Application StartDate for new designation.");
                        AddMessage(msg, msg, Messages);
                        break;
                    }
                }
            }
            return 1;
        }
    }

    [RuleDBTag("UserOrganisationStructureEndDateCheck",
    "End Date cannot precede the Start Date when removing an ADUser from the Organistation Structure.",
    "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.OrganisationStructure.UserOrganisationStructureEndDateCheck")]
    [RuleInfo]
    public class UserOrganisationStructureEndDateCheck : BusinessRuleBase
    {
        public UserOrganisationStructureEndDateCheck(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IUserOrganisationStructureHistory uosh = Parameters[0] as IUserOrganisationStructureHistory;

            string query = UIStatementRepository.GetStatement("Rules.OrganisationStructure", "UserOrganisationStructureEndDateCheck");
            ParameterCollection pc = new ParameterCollection();
            pc.Add(new SqlParameter("@ADUser", uosh.ADUser.Key));
            pc.Add(new SqlParameter("@EndDate", uosh.ChangeDate));
            pc.Add(new SqlParameter("@Action", uosh.Action));
            pc.Add(new SqlParameter("@OrganisationStructureKey", uosh.OrganisationStructureKey.Key));
            int cnt = 0;
            string desc = string.Empty;
            DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), pc);
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    cnt = Convert.ToInt32(dr[0]);
                    if (cnt > 0)
                    {
                        string msg = string.Format("The End Date cannot be set before the Start Date.");
                        AddMessage(msg, msg, Messages);
                        break;
                    }
                }
            }
            return 1;
        }
    }

    [RuleDBTag("UserOrganisationStructureAllocationMandateCheck",
    "Checks if the UserOrganisationStructure is linked to a AllocationMandateSetUserOrganisationStructure record.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.OrganisationStructure.UserOrganisationStructureAllocationMandateCheck")]
    [RuleInfo]
    public class UserOrganisationStructureAllocationMandateCheck : BusinessRuleBase
    {
        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            IUserOrganisationStructure uos = Parameters[0] as IUserOrganisationStructure;

            IOrganisationStructureRepository orgRepo = RepositoryFactory.GetRepository<IOrganisationStructureRepository>();
            IAllocationMandateSetGroup allocationMandateSetGroup = orgRepo.GetAllocationMandateSetGroupByUserOrganisationStructureKey(uos.Key);

            if (allocationMandateSetGroup != null)
            {
                string msg = string.Format("Please remove {0} from their Allocation Mandate {1} before continuing.", uos.ADUser.ADUserName, allocationMandateSetGroup.AllocationGroupName);
                AddMessage(msg, msg, Messages);
                return 1;
            }
            else
                return 0;
        }
    }

    [RuleDBTag("BranchUserSameBranchAsCurrentBranchConsultant",
    "Check if the user trying to perfom the action is in the same branch as users against the application.",
    "SAHL.Rules.DLL",
    "SAHL.Common.BusinessModel.Rules.OrganisationStructure.BranchUserSameBranchAsCurrentBranchConsultant")]
    [RuleInfo]
    public class BranchUserSameBranchAsCurrentBranchConsultant : BusinessRuleBase
    {
        public BranchUserSameBranchAsCurrentBranchConsultant(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            if (Parameters.Length == 0)
                throw new ArgumentException("The BranchUserSameBranchAsCurrentBranchConsultant rule expects a Domain object to be passed.");

            IApplication application = Parameters[0] as IApplication;
            IADUser aduser = Parameters[1] as IADUser;

            if (application == null)
                throw new ArgumentException("The BranchUserSameBranchAsCurrentBranchConsultant rule expects a Domain object to be passed : IApplication");

            if (aduser == null)
                throw new ArgumentException("The BranchUserSameBranchAsCurrentBranchConsultant rule expects a Domain object to be passed : IADUser");

            ParameterCollection prms = new ParameterCollection();
            prms.Add(new SqlParameter("@ADUserName", aduser.ADUserName));
            prms.Add(new SqlParameter("@OfferKey", application.Key));
            string query = UIStatementRepository.GetStatement("Rules.OrganisationStructure", "BranchUserSameBranchAsCurrentBranchConsultant");
            object o = castleTransactionService.ExecuteScalarOnCastleTran(query, typeof(OrganisationStructure_DAO), prms);

            if (o != null && Convert.ToInt16(o) > 0)
                return 0;

            string msg = string.Format("To perform this function you must be within the same branch as the application.");
            AddMessage(msg, msg, Messages);
            return 1;
        }
    }

    [RuleDBTag("CheckActiveUsersInWorkflowRole",
    "",
    "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.OrganisationStructure.UserStatusMaintenanceAtLeastOneActiveUserInWorkflow")]
    [RuleInfo]
    public class UserStatusMaintenanceAtLeastOneActiveUserInWorkflow : BusinessRuleBase
    {
        public UserStatusMaintenanceAtLeastOneActiveUserInWorkflow(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            int workflowRoleTypeKey = (Int32)Parameters[0];

            string query = UIStatementRepository.GetStatement("Rules.OrganisationStructure", "GetCountActiveADUserInWorkflowRole");
            ParameterCollection pc = new ParameterCollection();
            pc.Add(new SqlParameter("@WorkflowRoleTypekey", workflowRoleTypeKey));

            int cnt = 0;
            DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), pc);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                cnt = Convert.ToInt32(dr[0]);
            }
            if (cnt < 1)
            {
                string msg = "There needs to be at least one active user in the group";
                AddMessage(msg, msg, Messages);
            }
            return 1;
        }
    }

    [RuleDBTag("UserStatusMaintenanceRoundRobinStatus",
    "",
    "SAHL.Rules.DLL",
   "SAHL.Common.BusinessModel.Rules.OrganisationStructure.UserStatusMaintenanceRoundRobinStatus")]
    [RuleInfo]
    public class UserStatusMaintenanceRoundRobinStatus : BusinessRuleBase
    {
        public UserStatusMaintenanceRoundRobinStatus(ICastleTransactionsService castleTransactionService)
        {
            this.castleTransactionService = castleTransactionService;
        }

        private ICastleTransactionsService castleTransactionService;

        public override int ExecuteRule(IDomainMessageCollection Messages, params object[] Parameters)
        {
            int workflowRoleTypeKey = (Int32)Parameters[0];

            string query = UIStatementRepository.GetStatement("Rules.OrganisationStructure", "GetUserStatusRoundRobinStatusMaintenance");
            ParameterCollection pc = new ParameterCollection();
            pc.Add(new SqlParameter("@WorkflowRoleTypekey", workflowRoleTypeKey));

            int cnt = 0;
            DataSet ds = this.castleTransactionService.ExecuteQueryOnCastleTran(query, typeof(GeneralStatus_DAO), pc);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                cnt = Convert.ToInt32(dr[0]);
            }
            if (cnt < 1)
            {
                string msg = "At least one user must have both their User Status and Round Robin Status set to Active.";
                AddMessage(msg, msg, Messages);
            }
            return 1;
        }
    }
}