using Machine.Fakes;
using Machine.Specifications;
using SAHL.Batch.Service.CapitecService;
using SAHL.Batch.Service.Services;
using SAHL.Services.Capitec.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.Batch.Service.Test.CapitecWebServiceClientSpecs
{
    public class when_asked_to_create_new_purchase_application : WithFakes
    {
        static CapitecWebServiceClient capitecWebServiceClient;
        static ICapitec client;
        static CapitecApplication switchLoanApplication;
        static int messageID;

        Establish context = () =>
        {
            client = An<ICapitec>();
            capitecWebServiceClient = new CapitecWebServiceClient(client);

            messageID = 1;
            switchLoanApplication = CapitecApplicationStubs.CreateDummySwitchLoanApp();
        };

        Because of = () =>
        {
            capitecWebServiceClient.CreateApplication(switchLoanApplication, messageID);
        };

        It should_ask_client_to_create_switch_application = () =>
        {
            client.WasToldTo(c => c.CreateCapitecSwitchLoanApplication((SwitchLoanApplication)switchLoanApplication, messageID));
        };
    }
}
