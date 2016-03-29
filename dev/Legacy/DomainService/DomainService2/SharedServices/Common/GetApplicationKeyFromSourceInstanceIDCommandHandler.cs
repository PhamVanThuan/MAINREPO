using System;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.X2.BusinessModel.Interfaces;

namespace DomainService2.SharedServices.Common
{
    public class GetApplicationKeyFromSourceInstanceIDCommandHandler : IHandlesDomainServiceCommand<GetApplicationKeyFromSourceInstanceIDCommand>
    {
        private IX2Repository x2Repository;
        private IX2WorkflowService x2WorkflowService;

        public GetApplicationKeyFromSourceInstanceIDCommandHandler(IX2Repository x2Repository, IX2WorkflowService x2WorkflowService)
        {
            this.x2Repository = x2Repository;
            this.x2WorkflowService = x2WorkflowService;
        }

        public void Handle(IDomainMessageCollection messages, GetApplicationKeyFromSourceInstanceIDCommand command)
        {
            IInstance instance = this.x2Repository.GetInstanceByKey(command.InstanceID);

            if (null != instance.SourceInstanceID)
            {
                IInstance sourceinstance = this.x2Repository.GetInstanceByKey((Int64)instance.SourceInstanceID);
                IDictionary<string, object> data = this.x2WorkflowService.GetX2DataRow(sourceinstance.ID);
                command.ApplicationKeyResult = Convert.ToInt32(data["ApplicationKey"]);
            }
            else
            {
                throw new ArgumentException(Strings.GetApplicationIDFromSourceInstanceID);
            }
        }
    }
}