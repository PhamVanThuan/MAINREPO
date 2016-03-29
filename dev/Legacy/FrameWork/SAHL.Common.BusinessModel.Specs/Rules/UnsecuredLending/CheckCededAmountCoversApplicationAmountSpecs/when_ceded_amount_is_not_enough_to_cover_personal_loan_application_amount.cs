using Machine.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Fakes;
using SAHL.Common.BusinessModel.Rules.UnsecuredLending.PersonalLoan;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;

namespace SAHL.Common.BusinessModel.Specs.Rules.UnsecuredLending.CheckCededAmountCoversApplicationAmountSpecs
{
    [Subject(typeof(CheckCededAmountCoversApplicationAmount))]
    public class when_ceded_amount_is_not_enough_to_cover_personal_loan_application_amount : RulesBaseWithFakes<CheckCededAmountCoversApplicationAmount>
    {
        private static IApplicationRepository applicationRepository;
        private static IApplicationUnsecuredLending applicationUnsecuredLending;
        private static IApplicationProductPersonalLoan applicationProductPersonalLoan;
        private static IApplicationInformationPersonalLoan applicationInfoProductPersonalLoan;
        private static double sumInsured;

        Establish context = () =>
        {

            sumInsured = 1D;
            var personalLoanAmount = 2D;
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
            parameters = new object[] { applicationKey, sumInsured }; //legal entity key
            RulesBaseWithFakes<CheckCededAmountCoversApplicationAmount>.startrule.Invoke();
        };

        Because of = () =>
        {
            businessRule.ExecuteRule(messages, parameters);
        };

        It rule_should_fail = () =>
        {
            messages.Count.ShouldEqual(1);
        };
    }
}
