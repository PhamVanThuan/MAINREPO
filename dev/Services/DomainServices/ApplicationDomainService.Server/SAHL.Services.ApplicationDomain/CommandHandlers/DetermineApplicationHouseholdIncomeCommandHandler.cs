using System;
using System.Linq;

using SAHL.Core.Data;
using SAHL.Core.Events;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.ApplicationDomain.Rules;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.ApplicationDomain.Managers.Application;

namespace SAHL.Services.ApplicationDomain.CommandHandlers
{
    public class DetermineApplicationHouseholdIncomeCommandHandler : IDomainServiceCommandHandler<DetermineApplicationHouseholdIncomeCommand, ApplicationHouseholdIncomeDeterminedEvent>
    {
        private readonly IApplicationDataManager _applicationDataManager;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IApplicantDataManager _applicantDataManager;
        private readonly IEventRaiser _eventRaiser;
        private readonly IDomainRuleManager<OfferInformationDataModel> _domainRuleContext;

        private ApplicationHouseholdIncomeDeterminedEvent _applicationHouseholdIncomeDeterminedEvent;

        public DetermineApplicationHouseholdIncomeCommandHandler(IApplicationDataManager applicationDataManager, IUnitOfWorkFactory unitOfWorkFactory, 
                                                                 IApplicantDataManager applicantDataManager, IEventRaiser eventRaiser,
                                                                 IDomainRuleManager<OfferInformationDataModel> domainRuleContext)
        {
            _applicationDataManager = applicationDataManager;
            _applicantDataManager   = applicantDataManager;
            _unitOfWorkFactory      = unitOfWorkFactory;
            _eventRaiser            = eventRaiser;
            _domainRuleContext      = domainRuleContext;

            _domainRuleContext.RegisterRule(new ApplicationMayNotBeAcceptedWhenDeterminingApplicationHouseholdIncomeRule());
        }

        public ISystemMessageCollection HandleCommand(DetermineApplicationHouseholdIncomeCommand command, IServiceRequestMetadata metadata)
        {
            ISystemMessageCollection messages = SystemMessageCollection.Empty();

            var offerInformation = _applicationDataManager.GetLatestApplicationOfferInformation(command.ApplicationNumber);
            _domainRuleContext.ExecuteRules(messages, offerInformation);
            if (messages.HasErrors) { return messages; }

            using (IUnitOfWork unitOfWork = _unitOfWorkFactory.Build())
            {
                var offerInformationVariableLoan = _applicationDataManager.GetApplicationInformationVariableLoan(offerInformation.OfferInformationKey);

                if (offerInformationVariableLoan != null)
                {
                    var totalIncome = 0d;

                    var incomeContributorEmployments = this._applicantDataManager.GetIncomeContributorApplicantsCurrentEmployment(command.ApplicationNumber);
                    if (incomeContributorEmployments != null && incomeContributorEmployments.Any()) // what happens if empty?
                    {
                        totalIncome = incomeContributorEmployments.Sum(x =>
                        {
                            if (x.ConfirmedBasicIncome.HasValue) // Logic copied from Application.CalculateHouseHoldIncome(bool) in Framework
                            {
                                return x.ConfirmedIncome;
                            }
                            else
                            {
                                return x.MonthlyIncome;
                            }
                        }); 
                    }

                    offerInformationVariableLoan.HouseholdIncome = totalIncome;
                    _applicationDataManager.UpdateApplicationInformationVariableLoan(offerInformationVariableLoan);

                    _applicationHouseholdIncomeDeterminedEvent = new ApplicationHouseholdIncomeDeterminedEvent(DateTime.Now, command.ApplicationNumber,
                        offerInformationVariableLoan.HouseholdIncome.Value);
                    _eventRaiser.RaiseEvent(DateTime.Now, _applicationHouseholdIncomeDeterminedEvent, command.ApplicationNumber, (int)GenericKeyType.Offer, metadata);
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
