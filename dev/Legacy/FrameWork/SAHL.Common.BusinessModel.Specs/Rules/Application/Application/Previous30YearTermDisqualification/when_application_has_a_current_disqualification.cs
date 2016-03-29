using Machine.Specifications;
using Machine.Fakes;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Collections;
using System.Collections.Generic;
using SAHL.Common.Globals;


namespace SAHL.Common.BusinessModel.Specs.Rules.Application.Application.Previous30YearTermDisqualification
{
    [Subject(typeof(SAHL.Common.BusinessModel.Rules.Application.ApplicationHasPrevious30YearTermDisqualification))]
    public class when_application_has_a_current_disqualification : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.ApplicationHasPrevious30YearTermDisqualification>
    {
        protected static IReasonRepository reasonRepository;
        protected static IApplication application;

        Establish Context = () =>
        {
            reasonRepository = An<IReasonRepository>();
            application = An<IApplication>();

            application.WhenToldTo(x => x.HasAttribute(Param.IsAny<OfferAttributeTypes>())).Return(true);
            businessRule = new BusinessModel.Rules.Application.ApplicationHasPrevious30YearTermDisqualification(reasonRepository);

            RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.ApplicationHasPrevious30YearTermDisqualification>.startrule.Invoke();
        };
        Because of = () =>
        {
            businessRule.ExecuteRule(messages, application);
        };
        It rule_should_not_check_reasons = () =>
        {
            reasonRepository.WasNotToldTo(x => x.GetReasonByGenericKeyListAndReasonTypeKey(Param.IsAny<List<int>>(), Param.IsAny<int>()));
        };
        It rule_should_pass = () =>
        {
            messages.Count.ShouldEqual(0);
        };
    }
}
