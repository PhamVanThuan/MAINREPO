using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Rules;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Applicant;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using System;
using System.Collections.Generic;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.SetApplicationEmploymentType
{
    public class when_an_event_is_raised : WithCoreFakes
    {
        private static SetApplicationEmploymentTypeCommand command;
        private static SetApplicationEmploymentTypeCommandHandler handler;
        private static IApplicationDataManager applicationDataManager;
        private static IApplicationManager applicationManager;
        private static IApplicantDataManager applicantDataManager;
        private static OfferInformationDataModel offerInformationDataModel;
        private static OfferInformationVariableLoanDataModel offerInformationVariableLoanDataModel;
        private static IEnumerable<EmploymentDataModel> employment;
        private static int applicationNumber;
        private static int applicationInformationNumber;
        private static EmploymentType determinedEmploymentType;
        private static IDomainRuleManager<OfferInformationDataModel> domainRuleManager;

        private Establish context = () =>
        {
            applicationDataManager = An<IApplicationDataManager>();
            applicationManager = An<IApplicationManager>();
            applicantDataManager = An<IApplicantDataManager>();
            domainRuleManager = An<IDomainRuleManager<OfferInformationDataModel>>();
            applicationNumber = 1;
            applicationInformationNumber = 123;

            offerInformationDataModel = new OfferInformationDataModel(applicationInformationNumber, DateTime.Today.AddDays(-1), applicationNumber, (int)OfferInformationType.AcceptedOffer, 
                "bob", null, null);
            applicationDataManager.WhenToldTo(x => x.GetLatestApplicationOfferInformation(applicationNumber)).Return(offerInformationDataModel);
            offerInformationVariableLoanDataModel = new OfferInformationVariableLoanDataModel(applicationNumber, null, null, null, null, null, null, null, null, null, null, null, null, null, 
                null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
            applicationDataManager.WhenToldTo(x => x.GetApplicationInformationVariableLoan(Param.IsAny<int>())).Return(offerInformationVariableLoanDataModel);

            employment = new List<EmploymentDataModel>()
            {
                new EmploymentDataModel(0, null, (int)EmploymentType.Salaried, (int)RemunerationType.Salaried, (int)EmploymentStatus.Current, 1, null, null, "", "", "", "", null, "", null, "", 
                    3, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "", 0, 0, null, null, null, null, null)
            };
            applicantDataManager.WhenToldTo(x => x.GetIncomeContributorApplicantsCurrentEmployment(Param.IsAny<int>())).Return(employment);

            determinedEmploymentType = EmploymentType.Salaried;
            applicationManager.WhenToldTo(x => x.DetermineEmploymentTypeForApplication(employment))
                              .Return(determinedEmploymentType);

            command = new SetApplicationEmploymentTypeCommand(applicationNumber);
            handler = new SetApplicationEmploymentTypeCommandHandler(unitOfWorkFactory, applicationDataManager, applicationManager, applicantDataManager, eventRaiser, domainRuleManager);
        };

        private Because of = () =>
        {
            handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_raise_the_employment_type_set_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(),
                                                    Arg.Is<ApplicationEmploymentTypeSetEvent>(y => y.ApplicationNumber == applicationNumber
                                                                                                && y.EmploymentTypeKey == (int)determinedEmploymentType),
                                                    applicationNumber,
                                                    (int)GenericKeyType.Offer,
                                                    serviceRequestMetaData));
        };
    }
}