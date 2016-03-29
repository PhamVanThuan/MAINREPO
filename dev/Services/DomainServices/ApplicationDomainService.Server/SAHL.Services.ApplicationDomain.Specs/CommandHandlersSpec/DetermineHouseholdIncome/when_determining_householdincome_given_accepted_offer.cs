using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Machine.Fakes;
using Machine.Specifications;

using SAHL.Core.Rules;
using SAHL.Core.Testing;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.ApplicationDomain.Managers.Application;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.DetermineHouseholdIncome
{
    class when_determining_householdincome_given_accepted_offer : WithCoreFakes
    {
        private static IApplicationDataManager _applicationDataManager;
        private static IApplicantDataManager _applicantDataManager;
        private static IDomainRuleManager<OfferInformationDataModel> _domainRuleManager;

        private static DetermineApplicationHouseholdIncomeCommand        _command;
        private static DetermineApplicationHouseholdIncomeCommandHandler _commandHandler;

        private static int _applicationNumber;
        private static OfferInformationDataModel _offerInformationDataModel;

        private Establish context = () =>
        {
            _applicationDataManager = An<IApplicationDataManager>();
            _applicantDataManager   = An<IApplicantDataManager>();
            _domainRuleManager      = new DomainRuleManager<OfferInformationDataModel>();

            _applicationNumber = 1;

            _offerInformationDataModel = new OfferInformationDataModel(DateTime.Today.AddDays(-1), _applicationNumber, (int)OfferInformationType.AcceptedOffer, "user", null, null);
            _applicationDataManager.WhenToldTo(x => x.GetLatestApplicationOfferInformation(_applicationNumber)).Return(_offerInformationDataModel);

            _command        = new DetermineApplicationHouseholdIncomeCommand(_applicationNumber);
            _commandHandler = new DetermineApplicationHouseholdIncomeCommandHandler(_applicationDataManager, unitOfWorkFactory, _applicantDataManager, eventRaiser, _domainRuleManager);
        };

        private Because of = () =>
        {
            messages = _commandHandler.HandleCommand(_command, serviceRequestMetaData);
        };

        private It should_return_error_message = () =>
        {
            messages.HasErrors.ShouldBeTrue();
            messages.ErrorMessages().ShouldContain(x => x.Message.Contains("Household Income cannot be determined once the application has been accepted"));
        };
    }
}
