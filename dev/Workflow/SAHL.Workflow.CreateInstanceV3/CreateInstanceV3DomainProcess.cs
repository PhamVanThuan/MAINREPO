using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Identity;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.Interfaces.ApplicationDomain;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Workflow.CreateInstanceV3
{
    public class CreateInstanceV3DomainProcess : ICreateInstanceV3DomainProcess
    {
        private IApplicationDomainServiceClient applicationDomainServiceClient;

        public CreateInstanceV3DomainProcess(IApplicationDomainServiceClient applicationDomainServiceClient)
        {
            this.applicationDomainServiceClient = applicationDomainServiceClient;
        }

        public bool CreateCase(ISystemMessageCollection messages)
        {
            var metadata = new ServiceRequestMetadata();
            var model = new NewPurchaseApplicationModel(OfferStatus.Open, 1, OriginationSource.SAHomeLoans, 55000, 955000, 122000, 240, Product.NewVariableLoan, "DomainProcess-xxx", 1, "bob");
            var command = new AddNewPurchaseApplicationCommand(model, CombGuid.Instance.Generate());

            var serviceMessages = this.applicationDomainServiceClient.PerformCommand(command, metadata);
            messages.Aggregate(serviceMessages);
            return !messages.HasErrors;
        }
    }
}
