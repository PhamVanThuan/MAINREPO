using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM.Managers;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Managers;
using SAHL.Services.ClientDomain.Rules;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Events;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ClientDomain.CommandHandlers
{
    public class AddEmployerCommandHandler : IDomainServiceCommandHandler<AddEmployerCommand, EmployerAddedEvent>
    {
        private IEmploymentDataManager EmploymentDataManager;
        private ILinkedKeyManager LinkedKeyManager;
        private EmployerAddedEvent @event;
        private IUnitOfWorkFactory uowFactory;
        private IEventRaiser eventRaiser;

        public AddEmployerCommandHandler(IEmploymentDataManager employmentDataManager, ILinkedKeyManager linkedKeyManager, IUnitOfWorkFactory uowFactory,
            IEventRaiser eventRaiser)
        {
            this.EmploymentDataManager = employmentDataManager;
            this.LinkedKeyManager = linkedKeyManager;
            this.uowFactory = uowFactory;
            this.eventRaiser = eventRaiser;
        }

        public ISystemMessageCollection HandleCommand(AddEmployerCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();
            using (var uow = uowFactory.Build())
            {
                int employerKey = 0;
                var employers = this.EmploymentDataManager.FindExistingEmployer(command.Employer);
                if (employers.Count() == 0)
                {
                    employerKey = this.EmploymentDataManager.SaveEmployer(command.Employer);
                    this.LinkedKeyManager.LinkKeyToGuid(employerKey, command.EmployerId);
                    @event = new EmployerAddedEvent(DateTime.Now, employerKey, command.Employer.EmployerName, command.Employer.TelephoneCode, command.Employer.TelephoneNumber,
                        command.Employer.ContactPerson, command.Employer.ContactEmail, command.Employer.EmployerBusinessType, command.Employer.EmploymentSector);
                    eventRaiser.RaiseEvent(DateTime.Now, @event, employerKey, (int)GenericKeyType.Employer, metadata);
                }
                else
                {
                    messages.AddMessage(new SystemMessage(String.Format("{0} is an existing employer and could not be added.", command.Employer.EmployerName), SystemMessageSeverityEnum.Error));
                }
                uow.Complete();
            }
            return messages;
        }
    }
}