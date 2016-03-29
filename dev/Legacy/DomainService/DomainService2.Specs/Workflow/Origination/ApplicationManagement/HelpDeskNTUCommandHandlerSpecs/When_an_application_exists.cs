using DomainService2.Specs.DomainObjects;
using DomainService2.Workflow.Origination.ApplicationManagement;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService2.Specs.Workflow.Origination.ApplicationManagement.HelpDeskNTUCommandHandlerSpecs
{
    [Subject(typeof(HelpDeskNTUCommandHandler))]
    internal class When_an_application_exists : WithFakes
    {
        protected static IApplicationRepository applicationRepository;
        protected static IReasonRepository reasonRepository;
        protected static IDomainMessageCollection messages;
        protected static HelpDeskNTUCommandHandler handler;
        protected static HelpDeskNTUCommand command;
        protected static int applicationKey;
        protected static IApplication application;
        protected static IApplicationInformation applicationInformation;
        protected static StubReadOnlyEventList<IReasonDefinition> returnReasonList;
        protected static IReasonDefinition reasonDefinition;
        protected static IReason reason;
        Establish context = () =>
        {
            applicationKey = 1;
            reasonRepository = An<IReasonRepository>();
            applicationRepository = An<IApplicationRepository>();
            application = An<IApplication>();
            applicationInformation = An<IApplicationInformation>();
            reasonDefinition = An<IReasonDefinition>();
            returnReasonList = new StubReadOnlyEventList<IReasonDefinition>(new List<IReasonDefinition>{reasonDefinition});
            reason = An<IReason>();
            handler = new HelpDeskNTUCommandHandler(reasonRepository, applicationRepository);
            command = new HelpDeskNTUCommand(applicationKey);

            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(applicationKey)).Return(application);
            application.WhenToldTo(x => x.GetLatestApplicationInformation()).Return(applicationInformation);
            reasonRepository.WhenToldTo(x => x.GetReasonDefinitionsByReasonDescriptionKey(ReasonTypes.ApplicationNTU, (int)ReasonDescriptions.ClientDoesNotWishToProceed))
                .Return(returnReasonList);
            reasonRepository.WhenToldTo(x => x.CreateEmptyReason()).Return(reason);
        };
        Because of = () =>
        {
            handler.Handle(messages, command);
        };
        It should_get_the_offer_information = () =>
        {
            application.WasToldTo(x => x.GetLatestApplicationInformation());
        };
        It should_get_a_reason_by_definition_and_description = () =>
        {
            reasonRepository.WasToldTo(x => x.GetReasonDefinitionsByReasonDescriptionKey(ReasonTypes.ApplicationNTU, (int)ReasonDescriptions.ClientDoesNotWishToProceed));
        };
        It should_save_a_new_reason = () =>
        {
            reasonRepository.WasToldTo(x => x.SaveReason(Param.IsAny<IReason>()));
        };
        It should_save_instance = () =>
        {
            command.Result.ShouldBeTrue();
        };
    }
}
