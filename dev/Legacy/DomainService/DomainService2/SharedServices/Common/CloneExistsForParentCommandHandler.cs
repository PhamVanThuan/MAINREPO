using System;
using System.Data;
using System.Linq;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace DomainService2.SharedServices.Common
{
    public class CloneExistsForParentCommandHandler : IHandlesDomainServiceCommand<CloneExistsForParentCommand>
    {
        private ICastleTransactionsService service;
        private IUIStatementService uistatementservice;

        public CloneExistsForParentCommandHandler(ICastleTransactionsService service, IUIStatementService uistatementservice)
        {
            this.service = service;
            this.uistatementservice = uistatementservice;
        }

        public void Handle(IDomainMessageCollection messages, CloneExistsForParentCommand command)
        {
            string query = uistatementservice.GetStatement("DomainService.Repository", "CloneExistsForParent");
            string stateFilter = "'" + String.Join("','", command.States.Select((o) => o.ToString()).ToArray()) + "'";
            query = string.Format(query, stateFilter);
            query = query.Replace("@InstanceID", Convert.ToString(command.ChildInstanceId));

            DataSet dsClone = service.ExecuteQueryOnCastleTran(query, Databases.TwoAM, null);
            // IF
            //  there are child instances at these states for the parent instance then dont create clones
            // ELSE
            //  create a clone.
            if (dsClone.Tables[0].Rows.Count > 0)
                command.Result = false;
            else
                command.Result = true;
        }
    }
}