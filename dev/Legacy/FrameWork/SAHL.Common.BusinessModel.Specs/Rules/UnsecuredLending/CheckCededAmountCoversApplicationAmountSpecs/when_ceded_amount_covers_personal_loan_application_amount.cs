using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Rules.UnsecuredLending.PersonalLoan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Fakes;
using SAHL.Common.BusinessModel.Interfaces.Repositories;

namespace SAHL.Common.BusinessModel.Specs.Rules.UnsecuredLending.CheckCededAmountCoversApplicationAmountSpecs
{
    [Subject(typeof(CheckCededAmountCoversApplicationAmount))]
    public class when_ceded_amount_covers_personal_loan_application_amount : RulesBaseWithFakes<CheckCededAmountCoversApplicationAmount>
    {
        private static IApplicationRepository applicationRepository;
        private static IApplicationUnsecuredLending applicationUnsecuredLending;
        private static IApplicationProductPersonalLoan applicationProductPersonalLoan;
        private static IApplicationInformationPersonalLoan applicationInfoProductPersonalLoan;
        private static double sumInsured;

        Establish context = () =>
        {

            sumInsured = 2D;
            var personalLoanAmount = 1D;
            applicationInfoProductPersonalLoan = An<IApplicationInformationPersonalLoan>();
            applicationInfoProductPersonalLoan.WhenToldTo(i => i.LoanAmount).Return(personalLoanAmount);
            applicationProductPersonalLoan = An<IApplicationProductPersonalLoan>();
            applicationProductPersonalLoan.WhenToldTo(a => a.ApplicationInformationPersonalLoan).Return(applicationInfoProductPersonalLoan);

            applicationUnsecuredLending = An<IApplicationUnsecuredLending>();
            applicationUnsecuredLending.WhenToldTo(app => app.GetLatestApplicationInformation()).Return(An<IApplicationInformation>());
            applicationUnsecuredLending.WhenToldTo(app => app.CurrentProduct).Return(applicationProductPersonalLoan);

            applicationRepository = An<IApplicationRepository>();
            applicationRepository.WhenToldTo(appRepo => appRepo.GetApplicationByKey(Param.IsAny<int>())).Return(applicationUnsecuredLending);

            businessRule = new CheckCededAmountCoversApplicationAmount(applicationRepository);

            int applicationKey = Param.IsAny<int>();
            parameters = new object[] {applicationKey, sumInsured }; //legal entity key
            RulesBaseWithFakes<CheckCededAmountCoversApplicationAmount>.startrule.Invoke();
        };

        Because of = () =>
        {
            businessRule.ExecuteRule(messages, parameters);
        };

        It rule_should_pass = () =>
        {
            messages.Count.ShouldEqual(0);
        };
    }
}
