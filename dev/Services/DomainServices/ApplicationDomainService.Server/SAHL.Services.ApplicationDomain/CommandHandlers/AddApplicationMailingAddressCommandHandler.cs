using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.ApplicationDomain.Managers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using SAHL.Services.Interfaces.DomainQuery;
using SAHL.Services.Interfaces.DomainQuery.Queries;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.CommandHandlers
{
    public class AddApplicationMailingAddressCommandHandler : IDomainServiceCommandHandler<AddApplicationMailingAddressCommand, MailingAddressAddedToApplicationEvent>
    {
        private IDomainQueryServiceClient domainQueryService;
        private IApplicationDataManager applicationDataManager;
        private IApplicantDataManager applicantDataManager;
        private IDomainRuleManager<ApplicationMailingAddressModel> domainRuleManager;
        private IUnitOfWorkFactory unitOfWorkFactory;
        private IEventRaiser eventRaiser;

        public AddApplicationMailingAddressCommandHandler(IDomainQueryServiceClient domainQueryService, IApplicationDataManager applicationDataManager,
            IDomainRuleManager<ApplicationMailingAddressModel> domainRuleManager, IUnitOfWorkFactory unitOfWorkFactory, IEventRaiser eventRaiser,
                IApplicantDataManager applicantDataManager)
        {
            this.domainQueryService = domainQueryService;
            this.applicationDataManager = applicationDataManager;
            this.domainRuleManager = domainRuleManager;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.eventRaiser = eventRaiser;
            this.applicantDataManager = applicantDataManager;

            domainRuleManager.RegisterRule(new ClientAddressMustBelongToAnApplicantRule(applicantDataManager));
            domainRuleManager.RegisterRule(new ApplicationMailingAddressCannotBeAFreeTextAddressRule(domainQueryService));
            domainRuleManager.RegisterRule(new CheckIfOnlineFormatStatementIsRequiredRule());
            domainRuleManager.RegisterRule(new CheckIfClientForEmailCorrespondenceIsRequiredRule());
        }

        public ISystemMessageCollection HandleCommand(AddApplicationMailingAddressCommand command, IServiceRequestMetadata metadata)
        {
            var systemMessages = SystemMessageCollection.Empty();

            using (var uow = unitOfWorkFactory.Build())
            {
                var applicationMailingAddressExists = applicationDataManager.DoesApplicationMailingAddressExist(command.model.ApplicationNumber);
                if (applicationMailingAddressExists)
                {
                    systemMessages.AddMessage(new SystemMessage("Application has already been assigned a mailing address", SystemMessageSeverityEnum.Error));
                }

                var doesApplicationExist = applicationDataManager.DoesOpenApplicationExist(command.model.ApplicationNumber);
                if (!doesApplicationExist)
                {
                    systemMessages.AddMessage(new SystemMessage("The application number provided does not exist. No Mailing Address could be added.", SystemMessageSeverityEnum.Error));
                }

                var getClientAddressQuery = new GetClientAddressQuery(command.model.ClientAddressKey);
                domainQueryService.PerformQuery(getClientAddressQuery);
                if ((getClientAddressQuery.Result != null) && !getClientAddressQuery.Result.Results.Any())
                {
                    systemMessages.AddMessage(new SystemMessage("The client address provided does not exist.", SystemMessageSeverityEnum.Error));
                }

                if (!systemMessages.HasErrors)
                {
                    var getClientAddressQueryResult = getClientAddressQuery.Result.Results.First();

                    domainRuleManager.ExecuteRules(systemMessages, command.model);
                    if (!systemMessages.HasErrors)
                    {
                        var applicationMailingAddressKey = applicationDataManager.SaveApplicationMailingAddress(command.model, getClientAddressQueryResult.ClientKey, 
                            getClientAddressQueryResult.AddressKey);
                        eventRaiser.RaiseEvent(DateTime.Now, new MailingAddressAddedToApplicationEvent(DateTime.Now, command.model.ApplicationNumber, command.model.ClientAddressKey, 
                            command.model.CorrespondenceLanguage, command.model.OnlineStatementRequired, command.model.OnlineStatementFormat, command.model.CorrespondenceMedium, 
                            command.model.ClientToUseForEmailCorrespondence),
                            applicationMailingAddressKey, (int)GenericKeyType.OfferMailingAddress, metadata);
                    }
                }

                uow.Complete();
            }

            return systemMessages;
        }
    }
}