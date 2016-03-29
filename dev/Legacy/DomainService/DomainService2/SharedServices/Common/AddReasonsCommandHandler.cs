using System;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace DomainService2.SharedServices.Common
{
    public class AddReasonsCommandHandler : IHandlesDomainServiceCommand<AddReasonsCommand>
    {
        private IReasonRepository reasonRepository;

        public AddReasonsCommandHandler(IReasonRepository reasonRepository)
        {
            this.reasonRepository = reasonRepository;
        }

        public void Handle(IDomainMessageCollection messages, AddReasonsCommand command)
        {
            IReadOnlyEventList<IReasonDefinition> reasonDefinitionList = reasonRepository.GetReasonDefinitionsByReasonDescriptionKey((ReasonTypes)Enum.Parse(typeof(ReasonTypes), command.ReasonTypeKey.ToString()), command.ReasonDescriptionKey);

            foreach (IReasonDefinition reasonDefinition in reasonDefinitionList)
            {
                IReason reason = reasonRepository.CreateEmptyReason();

                //populate the reason
                reason.GenericKey = command.GenericKey;
                reason.ReasonDefinition = reasonDefinition;

                //save
                reasonRepository.SaveReason(reason);
            }
        }
    }
}