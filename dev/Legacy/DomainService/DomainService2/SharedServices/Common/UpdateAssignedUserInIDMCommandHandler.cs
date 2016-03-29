using System;
using System.Data;
using System.Text;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.Collections.Interfaces;

namespace DomainService2.SharedServices.Common
{
    public class UpdateAssignedUserInIDMCommandHandler : IHandlesDomainServiceCommand<UpdateAssignedUserInIDMCommand>
    {
        private ICastleTransactionsService castleTransactionsService;
        private IApplicationRepository applciationRepository;
        private IWorkflowSecurityRepository securityRepository;

        public UpdateAssignedUserInIDMCommandHandler(ICastleTransactionsService castleTransactionsService, IApplicationRepository applicationRepository, IWorkflowSecurityRepository securityRepository)
        {
            this.castleTransactionsService = castleTransactionsService;
            this.applciationRepository = applicationRepository;
            this.securityRepository = securityRepository;
        }

        public void Handle(IDomainMessageCollection messages, UpdateAssignedUserInIDMCommand command)
        {
            StringBuilder sb = new StringBuilder();
            int offerRoleTypeKey = (int)SAHL.Common.Globals.OfferRoleTypes.NewBusinessProcessorD; // 694

            if (command.IsFurtherLoan)
                offerRoleTypeKey = (int)SAHL.Common.Globals.OfferRoleTypes.FLProcessorD; // 857

            sb.AppendFormat("select top 1 ADUserKey from vw_WFAssignment wfa where wfa.IID={0} and OfferRoleTypeKey={1} order by id desc", command.InstanceID, offerRoleTypeKey);

            DataSet ds = new DataSet();
            object aduserKeyObj = this.castleTransactionsService.ExecuteScalarOnCastleTran(sb.ToString(), SAHL.Common.Globals.Databases.X2, new SAHL.Common.DataAccess.ParameterCollection());
            if (aduserKeyObj != null)
            {
                int ADUSerKey = Convert.ToInt32(aduserKeyObj);
                int AccountKey, OfferStatusKey;
                string AssignedUser;
                IApplication app = this.applciationRepository.GetApplicationByKey(command.ApplicationKey);
                AccountKey = app.ReservedAccount.Key;
                OfferStatusKey = app.ApplicationStatus.Key;

                SAHL.Common.BusinessModel.Interfaces.Repositories.WorkflowRole.WorkflowAssignment.ADUserRow aduser = this.securityRepository.GetADUser(ADUSerKey);
                AssignedUser = aduser.ADUserName.Replace(@"SAHL\", "");

                string cmd = string.Format("exec [ImageIndex]..pr_UpdateAssignedUserAndStateFromX2 @Accountkey={0}, @OfferStatusKey={1}, @LoginName='{2}'", command.ApplicationKey, OfferStatusKey, AssignedUser);

                this.castleTransactionsService.ExecuteNonQueryOnCastleTran(cmd, SAHL.Common.Globals.Databases.X2, null);
            }
        }
    }
}