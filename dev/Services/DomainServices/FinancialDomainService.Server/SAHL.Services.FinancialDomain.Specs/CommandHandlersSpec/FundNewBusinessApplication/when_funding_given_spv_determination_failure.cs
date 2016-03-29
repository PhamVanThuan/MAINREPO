using Machine.Fakes;
using Machine.Specifications;
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
using System.Linq;

namespace SAHL.Services.FinancialDomain.Specs.CommandHandlersSpec.FundNewBusinessApplication
{
    public class when_funding_given_spv_determination_failure : WithCoreFakes
    {
        private static FundNewBusinessApplicationCommand command;
        private static FundNewBusinessApplicationCommandHandler handler;

        private static OfferInformationDataModel applicationInformation;
        private static OfferInformationVariableLoanDataModel applicationInformationVariableLoan;
        private static FeeApplicationAttributesModel feeApplicationAttributes;
        private static OfferDataModel application;
        private static IFinancialDataManager financialDataManager;
        private static IFinancialManager financialManager;
        private static int applicationNumber;
        private static OfferType applicationTypeKey = OfferType.NewPurchaseLoan;
        private static int accountKey = 8;
        private static decimal ltv;
        private static EmploymentType employmentType;
        private static decimal householdIncome;
        private static bool isStaffLoan;
        private static string SPVDetermineErrorMessage;
        private static IDomainRuleManager<IApplicationModel> domainRuleManager;
        private static IServiceQueryRouter serviceQueryRouter;

        private Establish context = () =>
        {
            applicationNumber = 1;
            ltv = 0.9m;
            employmentType = EmploymentType.Salaried;
            householdIncome = 20000m;
            isStaffLoan = false;
            domainRuleManager = An<IDomainRuleManager<IApplicationModel>>();
            serviceQueryRouter = An<IServiceQueryRouter>();

            applicationInformation = new OfferInformationDataModel(DateTime.Now, 111, (int)OfferType.NewPurchaseLoan,
                "someone", DateTime.Now.AddDays(1), (int)Product.NewVariableLoan);
            applicationInformation.OfferInformationKey = 222;
            applicationInformationVariableLoan =
                new OfferInformationVariableLoanDataModel(applicationInformation.OfferInformationKey, 1, 1, 1, 1, 1,
                    (double)householdIncome, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, (double)ltv, 1, 1, 1,
                    (int)employmentType, 1, 1, 1, 1);

            financialDataManager = An<IFinancialDataManager>();

            financialDataManager.WhenToldTo(x => x.GetLatestApplicationOfferInformation(applicationNumber))
                .Return(applicationInformation);

            financialDataManager.WhenToldTo(
                x => x.GetApplicationInformationVariableLoan(applicationInformation.OfferInformationKey))
                .Return(applicationInformationVariableLoan);

            financialManager = An<IFinancialManager>();

            feeApplicationAttributes = new FeeApplicationAttributesModel(false, isStaffLoan, true, true, false, false);
            financialDataManager.WhenToldTo(x => x.GetFeeApplicationAttributes(applicationNumber))
                .Return(feeApplicationAttributes);

            application = new OfferDataModel((int)applicationTypeKey, 1, null, null, null, "", null, null, accountKey,
                1, 1);
            financialDataManager.WhenToldTo(x => x.GetApplication(applicationNumber)).Return(application);

            SPVDetermineErrorMessage = "Unable to determine an SPV for this application";
            var messages1 = SystemMessageCollection.Empty();
            messages1.AddMessage(new SystemMessage(SPVDetermineErrorMessage,
                SystemMessageSeverityEnum.Error));
            serviceQueryRouter.WhenToldTo(x => x.HandleQuery(Param.IsAny<DetermineSPVQuery>()))
                .Return(messages1);

            command = new FundNewBusinessApplicationCommand(applicationNumber);
            handler = new FundNewBusinessApplicationCommandHandler(serviceQueryRouter, financialDataManager, financialManager, eventRaiser, unitOfWorkFactory, domainRuleManager);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_add_an_error_message_to_the_messages_collection = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual(SPVDetermineErrorMessage);
        };

        private It should_not_update_the_latest_application_information_variableloans_SPV = () =>
        {
            financialDataManager.WasNotToldTo(x => x.SaveOfferInformationVariableLoan(applicationInformationVariableLoan));
        };
    }
}