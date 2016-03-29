using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.CommandHandlers
{
    public class SetApplicationEmploymentTypeCommandHandler : IDomainServiceCommandHandler<SetApplicationEmploymentTypeCommand, ApplicationEmploymentTypeSetEvent>
    {
        private IApplicationDataManager applicationDataManager;
        private IApplicantDataManager applicantDataManager;
        private IApplicationManager applicationManager;
        private IUnitOfWorkFactory unitOfWorkFactory;
        private IEventRaiser eventRaiser;
        private IDomainRuleManager<OfferInformationDataModel> domainRuleManager;

        private ApplicationEmploymentTypeSetEvent applicationEmploymentTypeSetEvent;

        public SetApplicationEmploymentTypeCommandHandler(IUnitOfWorkFactory unitOfWorkFactory, IApplicationDataManager applicationDataManager, IApplicationManager applicationManager, 
            IApplicantDataManager applicantDataManager, IEventRaiser eventRaiser, IDomainRuleManager<OfferInformationDataModel> domainRuleManager)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.applicationDataManager = applicationDataManager;
            this.applicationManager = applicationManager;
            this.applicantDataManager = applicantDataManager;
            this.eventRaiser = eventRaiser;
            this.domainRuleManager = domainRuleManager;
            this.domainRuleManager.RegisterRule(new ApplicationMayNotBeAcceptedWhenSettingApplicationEmploymentTypeRule());
        }

        public ISystemMessageCollection HandleCommand(SetApplicationEmploymentTypeCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();

            OfferInformationDataModel offerInformation = applicationDataManager.GetLatestApplicationOfferInformation(command.ApplicationNumber);
            domainRuleManager.ExecuteRules(messages, offerInformation);
            if (messages.HasErrors) 
            { 
                return messages; 
            }

            using (IUnitOfWork unitOfWork = unitOfWorkFactory.Build())
            {
                var offerInformationVariableLoan = applicationDataManager.GetApplicationInformationVariableLoan(offerInformation.OfferInformationKey);

                if (offerInformationVariableLoan != null)
                {
                    int employmentTypeKey = (int)EmploymentType.Unknown;
                    IEnumerable<EmploymentDataModel> employment = applicantDataManager.GetIncomeContributorApplicantsCurrentEmployment(command.ApplicationNumber);
                    if (employment != null && employment.Any())
                    {
                        employmentTypeKey = (int)applicationManager.DetermineEmploymentTypeForApplication(employment);
                    }
                    offerInformationVariableLoan.EmploymentTypeKey = employmentTypeKey;
                    applicationDataManager.UpdateApplicationInformationVariableLoan(offerInformationVariableLoan);

                    applicationEmploymentTypeSetEvent = new ApplicationEmploymentTypeSetEvent(DateTime.Now, command.ApplicationNumber, employmentTypeKey);
                    eventRaiser.RaiseEvent(DateTime.Now, applicationEmploymentTypeSetEvent, command.ApplicationNumber, (int)GenericKeyType.Offer, metadata);
                }
                else
                {
                    messages.AddMessage(new SystemMessage(string.Format("No OfferInformationVariableLoan was found for application {0}.", command.ApplicationNumber), 
                        SystemMessageSeverityEnum.Error));
                }

                unitOfWork.Complete();
            }

            return messages;
        }
    }
}