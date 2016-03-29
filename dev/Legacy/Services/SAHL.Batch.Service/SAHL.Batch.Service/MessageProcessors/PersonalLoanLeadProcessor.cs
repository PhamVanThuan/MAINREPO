using SAHL.Batch.Common;
using SAHL.Batch.Common.ServiceContracts;
using SAHL.Shared.Messages.PersonalLoanLead;
using StructureMap;
using System;

namespace SAHL.Batch.Service.MessageProcessors
{
    public class PersonalLoanLeadProcessor : IMessageProcessor<PersonalLoanLeadMessage>
    {
        private IPersonalLoanLeadCreationService personalLoanLeadService;

        public PersonalLoanLeadProcessor()
            : this(ObjectFactory.GetInstance<IPersonalLoanLeadCreationService>())
        {

        }

        public PersonalLoanLeadProcessor(IPersonalLoanLeadCreationService personalLoanLeadService)
        {
            this.personalLoanLeadService = personalLoanLeadService;
        }

        public bool Process(PersonalLoanLeadMessage message)
        {
            var status = personalLoanLeadService.CreatePersonalLoanLeadFromIdNumber(message.IdNumber, message.Id);
            return status;
        }
    }
}   