using Machine.Specifications;
using Machine.Fakes;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.Managers.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using NSubstitute;

namespace SAHL.Services.ApplicationDomain.Specs.ManagerSpecs.Application
{
    public class when_saving_newpurchase_application_mortgageloan : WithCoreFakes
    {
        private static IApplicationManager applicationManager;
        private static IApplicationDataManager applicationDataManager;

        private static int applicationNumber;
        private static MortgageLoanPurpose mortgageLoanPurpose;
        private static int applicantCount;
        private static decimal purchasePrice;
        private static string transferAttorney;

        Establish context = () =>
        {
            applicationNumber = 1;
            mortgageLoanPurpose = MortgageLoanPurpose.Newpurchase;
            applicantCount = 1;
            purchasePrice = 1;
            transferAttorney = "transferAttorney";

            applicationDataManager = An<IApplicationDataManager>();
            applicationManager = new ApplicationManager(applicationDataManager);
        };

        Because of = () =>
        {
            applicationManager.SaveApplicationMortgageLoan(applicationNumber, mortgageLoanPurpose, applicantCount, purchasePrice, null, transferAttorney);
        };

        It should_ask_the_application_data_manager_to_save_the_application_mortgage_loan_model = () =>
        {
            applicationDataManager.WasToldTo(x => x.SaveApplicationMortgageLoan(Arg.Is<OfferMortgageLoanDataModel>(y => 
                    y.OfferKey == applicationNumber &&
                    y.MortgageLoanPurposeKey == (int) mortgageLoanPurpose &&
                    y.NumApplicants == applicantCount &&
                    y.PurchasePrice == (double)purchasePrice &&
                    y.ClientEstimatePropertyValuation == (double)purchasePrice &&
                    y.TransferringAttorney == (string)transferAttorney
                )));
        };
    }
}
