using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.FinancialDomain.CommandHandlers;
using SAHL.Services.FinancialDomain.Managers;
using SAHL.Services.Interfaces.FinancialDomain.Commands;
using SAHL.Services.Interfaces.FinancialDomain.Commands.Internal;
using SAHL.Services.Interfaces.FinancialDomain.Models;
using System;

namespace SAHL.Services.FinancialDomain.Specs.CommandHandlersSpec.FundNewBusinessApplication
{
    public class when_funding_new_business_application : WithCoreFakes
    {
        private static FundNewBusinessApplicationCommand command;
        private static FundNewBusinessApplicationCommandHandler handler;

        private static OfferInformationDataModel applicationInformation;
        private static OfferInformationVariableLoanDataModel applicationInformationVariableLoan;
        private static FeeApplicationAttributesModel feeApplicationAttributes;
        private static OfferDataModel application;
        private static IServiceQueryRouter serviceQueryRouter;
        private static IFinancialDataManager financialDataManager;
        private static IFinancialManager financialManager;
        private static int applicationNumber;
        private static OfferType applicationTypeKey = OfferType.NewPurchaseLoan;
        private static int accountKey = 8;
        private static int productKey = 9;
        private static decimal ltv;
        private static EmploymentType employmentType;
        private static decimal householdIncome;
        private static bool isStaffLoan;
        private static int determinedSPVKey;
        private static IDomainRuleManager<IApplicationModel> domainRuleManager;

        private Establish context = () =>
        {
            applicationNumber = 1;
            ltv = 0.9m;
            employmentType = EmploymentType.Salaried;
            householdIncome = 20000m;
            isStaffLoan = false;
            domainRuleManager = An<IDomainRuleManager<IApplicationModel>>();

            application = new OfferDataModel(applicationNumber, (int)applicationTypeKey, 1, null, null, null, "", null, null, accountKey, 1, 1);

            applicationInformation = new OfferInformationDataModel(DateTime.Now, 111, (int)application.OfferTypeKey,
                "someone", DateTime.Now.AddDays(1), (int)Product.NewVariableLoan);
            applicationInformation.OfferInformationKey = 222;
            applicationInformationVariableLoan =
                new OfferInformationVariableLoanDataModel(applicationInformation.OfferInformationKey, 1, 1, 1, 1, 1,
                    (double)householdIncome, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, (double)ltv, 1, 1, 1,
                    (int)employmentType, 1, 1, 1, 1);

            financialManager = An<IFinancialManager>();
            financialDataManager = An<IFinancialDataManager>();

            financialDataManager.WhenToldTo(x => x.GetLatestApplicationOfferInformation(applicationNumber))
                .Return(applicationInformation);

            financialDataManager.WhenToldTo(
                x => x.GetApplicationInformationVariableLoan(applicationInformation.OfferInformationKey))
                .Return(applicationInformationVariableLoan);

            feeApplicationAttributes = new FeeApplicationAttributesModel(false, isStaffLoan, true, true, false, false);
            financialDataManager.WhenToldTo(x => x.GetFeeApplicationAttributes(applicationNumber))
                                .Return(feeApplicationAttributes);

            financialDataManager.WhenToldTo(x => x.GetApplication(applicationNumber)).Return(application);

            determinedSPVKey = 100;

            serviceQueryRouter = An<IServiceQueryRouter>();
            serviceQueryRouter.WhenToldTo(x => x.HandleQuery(Param.IsAny<DetermineSPVQuery>()))
                .Return<DetermineSPVQuery>(y =>
                {
                    y.Result = new ServiceQueryResult<int>(new int[] { determinedSPVKey });
                    return SystemMessageCollection.Empty();
                });

            command = new FundNewBusinessApplicationCommand(applicationNumber);
            handler = new FundNewBusinessApplicationCommandHandler(serviceQueryRouter, financialDataManager, financialManager, eventRaiser, unitOfWorkFactory, domainRuleManager);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_check_if_the_appplications_latest_application_information_is_not_accepted = () =>
        {
            financialDataManager.WasToldTo(x => x.GetLatestApplicationOfferInformation(applicationNumber));
        };

        private It should_retrieve_the_appplications_latest_application_information_variableloan = () =>
        {
            financialDataManager.WasToldTo(x => x.GetApplicationInformationVariableLoan(applicationInformation.OfferInformationKey));
        };

        private It should_determine_an_SPV_for_the_application = () =>
        {
            serviceQueryRouter.WasToldTo(x => x.HandleQuery(Arg.Is<DetermineSPVQuery>(y => y.ApplicationNumber == application.OfferKey && y.EmploymentType == employmentType && y.HouseholdIncome == householdIncome && y.IsStaffLoan == isStaffLoan
                && y.LTV == ltv)));
        };

        private It should_update_the_latest_application_information_variableloans_SPV = () =>
        {
            financialManager.WasToldTo(x => x.SetApplicationInformationSPVKey(applicationInformation.OfferInformationKey, determinedSPVKey));
        };

        private It should_set_the_application_information_reset_configuration_key = () =>
        {
            financialDataManager.WasToldTo(x => x.SetApplicationResetConfiguration(command.ApplicationNumber, determinedSPVKey, productKey));
        };
    }
}