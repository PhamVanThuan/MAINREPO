using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Rules.UnsecuredLending.PersonalLoan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Fakes;

namespace SAHL.Common.BusinessModel.Specs.Rules.UnsecuredLending.CheckExternalPolicyIsCededSpecs
{
    [Subject(typeof(CheckExternalPolicyIsCeded))]
    public class when_external_life_is_not_fully_captured : RulesBaseWithFakes<CheckExternalPolicyIsCeded>
    {
        private static IApplicationRepository applicationRepository;
        private static IApplicationUnsecuredLending applicationUnsecuredLending;
        private static IApplicationProductPersonalLoan applicationProductPersonalLoan;
        private static IExternalLifePolicy externalLifePolicy;
        private static ILifePolicyStatus lifePolicyStatus;
        private static IApplicationInformationPersonalLoan applicationInformationPersonalLoan;

        Establish context = () =>
        {
            lifePolicyStatus = An<ILifePolicyStatus>();
            lifePolicyStatus.WhenToldTo(l => l.Description).Return("Inforce");

            externalLifePolicy = An<IExternalLifePolicy>();
            externalLifePolicy.WhenToldTo(e => e.Insurer).Return(An<IInsurer>());
            externalLifePolicy.WhenToldTo(e => e.LifePolicyStatus).Return(lifePolicyStatus);
            externalLifePolicy.WhenToldTo(e => e.PolicyNumber).Return(string.Empty);
            externalLifePolicy.WhenToldTo(e => e.CommencementDate).Return(Param.IsAny<DateTime>());
            externalLifePolicy.WhenToldTo(e => e.CloseDate).Return(Param.IsAny<DateTime>());
            externalLifePolicy.WhenToldTo(e => e.SumInsured).Return(1D);
            externalLifePolicy.WhenToldTo(e => e.PolicyCeded).Return(false);

            applicationProductPersonalLoan = An<IApplicationProductPersonalLoan>();
            applicationProductPersonalLoan.WhenToldTo(a => a.ExternalLifePolicy).Return(externalLifePolicy);

            applicationInformationPersonalLoan = An<IApplicationInformationPersonalLoan>();
            // Having a value greater than zero set on this property is interpreted to mean SAHL Life is elected
            applicationInformationPersonalLoan.WhenToldTo(appInfo => appInfo.LifePremium).Return(0D);

            applicationProductPersonalLoan = An<IApplicationProductPersonalLoan>();
            applicationProductPersonalLoan.WhenToldTo(appProduct => appProduct.ApplicationInformationPersonalLoan).Return(applicationInformationPersonalLoan);

            applicationUnsecuredLending = An<IApplicationUnsecuredLending>();
            applicationUnsecuredLending.WhenToldTo(app => app.GetLatestApplicationInformation()).Return(An<IApplicationInformation>());
            applicationUnsecuredLending.WhenToldTo(app => app.CurrentProduct).Return(applicationProductPersonalLoan);

            applicationRepository = An<IApplicationRepository>();
            applicationRepository.WhenToldTo(appRepo => appRepo.GetApplicationByKey(Param.IsAny<int>())).Return(applicationUnsecuredLending);

            businessRule = new CheckExternalPolicyIsCeded(applicationRepository);

            int applicationKey = Param.IsAny<int>();
            parameters = new object[] { applicationKey }; //legal entity key
            RulesBaseWithFakes<CheckExternalPolicyIsCeded>.startrule.Invoke();
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
