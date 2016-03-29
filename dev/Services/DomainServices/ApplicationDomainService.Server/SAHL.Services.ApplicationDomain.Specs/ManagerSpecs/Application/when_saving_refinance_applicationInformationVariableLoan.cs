using Machine.Specifications;
using Machine.Fakes;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.Managers.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.Application
{
    public class when_saving_refinance_applicationInformationVariableLoan : WithCoreFakes
    {
        private static IApplicationManager applicationManager;
        private static IApplicationDataManager applicationDataManager;
        private static int offerInformationKey;
        private static int term;
        private static decimal estimatedPropertyValue;
        private static decimal cashOut;
        private static decimal loanAmountNoFees;

        Establish context = () =>
        {
            applicationDataManager = An<IApplicationDataManager>();
            offerInformationKey = 1;
            term = 240;
            estimatedPropertyValue = 900000;
            cashOut = 70000;
            loanAmountNoFees = cashOut;

            applicationManager = new ApplicationManager(applicationDataManager);
        };

        Because of = () =>
        {
            applicationManager.SaveRefinanceApplicationInformationVariableLoan(offerInformationKey, term, estimatedPropertyValue, cashOut, loanAmountNoFees);
        };

        It should_ask_the_application_data_manager_to_to_save_the_application_information_variable_loan_model = () =>
        {
            applicationDataManager.WasToldTo(x => x.SaveApplicationInformationVariableLoan(Arg.Is<OfferInformationVariableLoanDataModel>(
                    y => y.OfferInformationKey == offerInformationKey &&
                         y.Term == term &&
                         y.ExistingLoan == null &&
                         y.PropertyValuation == (double)estimatedPropertyValue &&
                         y.RequestedCashAmount == (double)cashOut
                )));
        };
    }
}
