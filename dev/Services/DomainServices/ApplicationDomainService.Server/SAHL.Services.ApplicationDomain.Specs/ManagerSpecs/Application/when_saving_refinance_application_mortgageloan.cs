using Machine.Specifications;
using Machine.Fakes;
using SAHL.Services.ApplicationDomain.Managers.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.Testing;
using SAHL.Core.BusinessModel.Enums;
using NSubstitute;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.Application
{
    public class when_saving_refinance_application_mortgageloan : WithCoreFakes
    {
        private static IApplicationManager applicationManager;
        private static IApplicationDataManager applicationDataManager;

        private static int applicationNumber;
        private static MortgageLoanPurpose mortgageLoanPurpose;
        private static int applicantCount;
        private static decimal estimatedPropertyValuation;

        Establish context = () =>
        {
            applicationNumber = 1;
            mortgageLoanPurpose = MortgageLoanPurpose.Refinance;
            applicantCount = 2;
            estimatedPropertyValuation = 100000;

            applicationDataManager = An<IApplicationDataManager>();
            applicationManager = new ApplicationManager(applicationDataManager);
        };

        Because of = () =>
        {
            applicationManager.SaveApplicationMortgageLoan(applicationNumber, mortgageLoanPurpose, applicantCount, null, estimatedPropertyValuation, null);
        };

        It should_ask_the_application_data_manager_to_save_the_application_mortgage_loan_model = () =>
        {
            applicationDataManager.WasToldTo(x => x.SaveApplicationMortgageLoan(Arg.Is<OfferMortgageLoanDataModel>(y =>
                    y.OfferKey == applicationNumber &&
                    y.MortgageLoanPurposeKey == (int)mortgageLoanPurpose &&
                    y.NumApplicants == applicantCount &&
                    y.ClientEstimatePropertyValuation == (double) estimatedPropertyValuation
                )));
        };
    }
}
