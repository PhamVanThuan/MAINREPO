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
    public class when_saving_applicationInformation_interestonly : WithCoreFakes
    {
        private static IApplicationManager applicationManager;
        private static IApplicationDataManager applicationDataManager;
        private static int offerInformationKey;

        Establish context = () =>
        {
            offerInformationKey = 1;

            applicationDataManager = An<IApplicationDataManager>();
            applicationManager = new ApplicationManager(applicationDataManager);
        };

        Because of = () =>
        {
            applicationManager.SaveApplicationInformationInterestOnly(offerInformationKey);
        };

        It should_ask_the_application_data_manager_to_to_save_the_application_information_interest_only_model = () =>
        {
            applicationDataManager.WasToldTo(x => x.SaveApplicationInformationInterestOnly(Arg.Is<OfferInformationInterestOnlyDataModel>(y =>
                y.OfferInformationKey == offerInformationKey
            )));
        };
    }
}
