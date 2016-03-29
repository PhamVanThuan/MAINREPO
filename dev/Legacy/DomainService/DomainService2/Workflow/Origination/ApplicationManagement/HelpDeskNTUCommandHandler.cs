using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService2.Workflow.Origination.ApplicationManagement
{
    public class HelpDeskNTUCommandHandler : IHandlesDomainServiceCommand<HelpDeskNTUCommand>
    {
        IReasonRepository reasonRepository;
        IApplicationRepository applicationRepository;

        public HelpDeskNTUCommandHandler(IReasonRepository reasonRepository, IApplicationRepository applicationRepository)
        {
            this.reasonRepository = reasonRepository;
            this.applicationRepository = applicationRepository;
        }

        public void Handle(IDomainMessageCollection messages, HelpDeskNTUCommand command)
        {
            IApplication application = applicationRepository.GetApplicationByKey(command.ApplicationKey);
            int offerInfoKey = application.GetLatestApplicationInformation().Key;

            IReadOnlyEventList<IReasonDefinition> reasonDefinitionList = reasonRepository.GetReasonDefinitionsByReasonDescriptionKey(ReasonTypes.ApplicationNTU, (int)ReasonDescriptions.ClientDoesNotWishToProceed);

            foreach (IReasonDefinition reasonDefinition in reasonDefinitionList)
            {
                IReason reason = reasonRepository.CreateEmptyReason();

                //populate the reason
                reason.GenericKey = offerInfoKey;
                reason.ReasonDefinition = reasonDefinition;

                //save
                reasonRepository.SaveReason(reason);
            }
            command.Result = true;
        }
    }
}
