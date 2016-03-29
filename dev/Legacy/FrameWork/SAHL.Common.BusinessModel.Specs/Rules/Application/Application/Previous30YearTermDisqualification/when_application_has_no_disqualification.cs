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
    public class when_application_has_no_disqualification : RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.ApplicationHasPrevious30YearTermDisqualification>
    {
        protected static IReasonRepository reasonRepository;
        protected static IApplication application;
        protected static IEventList<IApplicationInformation> applicationInformations;
        protected static IApplicationInformation previousApplicationInformation;
        protected static IApplicationInformation latestApplicationInformation;

        protected static int previousApplicationInformationKey = 1;
        protected static int latestApplicationInformationKey = 99;

        Establish Context = () =>
        {
            reasonRepository = An<IReasonRepository>();
            application = An<IApplication>();

            previousApplicationInformation = An<IApplicationInformation>();
            previousApplicationInformation.WhenToldTo(x => x.Key).Return(previousApplicationInformationKey);

            latestApplicationInformation = An<IApplicationInformation>();
            latestApplicationInformation.WhenToldTo(x => x.Key).Return(latestApplicationInformationKey);

            applicationInformations = new EventList<IApplicationInformation>();
            applicationInformations.Add(null, previousApplicationInformation);
            applicationInformations.Add(null, latestApplicationInformation);

            application.WhenToldTo(x => x.HasAttribute(Param.IsAny<OfferAttributeTypes>())).Return(false);
            application.WhenToldTo(x => x.ApplicationInformations).Return(applicationInformations);

            reasonRepository.WhenToldTo(x => x.GetReasonByGenericKeyListAndReasonTypeKey(Param.IsAny<List<int>>(), Param.IsAny<int>())).Return(new ReadOnlyEventList<IReason>());

            businessRule = new BusinessModel.Rules.Application.ApplicationHasPrevious30YearTermDisqualification(reasonRepository);

            RulesBaseWithFakes<SAHL.Common.BusinessModel.Rules.Application.ApplicationHasPrevious30YearTermDisqualification>.startrule.Invoke();
        };
        Because of = () =>
        {
            businessRule.ExecuteRule(messages, application);
        };
        It rule_should_check_reasons = () =>
        {
            reasonRepository.WasToldTo(x => x.GetReasonByGenericKeyListAndReasonTypeKey(Param.IsAny<List<int>>(), Param.IsAny<int>()));
        };
        It rule_should_pass = () =>
        {
            messages.Count.ShouldEqual(0);
        };
    }
}
