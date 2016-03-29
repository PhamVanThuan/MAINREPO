using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Core.Testing;
using SAHL.Services.ApplicationDomain.CommandHandlers;
using SAHL.Services.ApplicationDomain.Managers.Application;
using SAHL.Services.Interfaces.ApplicationDomain.Commands;
using SAHL.Services.Interfaces.ApplicationDomain.Events;
using SAHL.Services.Interfaces.ApplicationDomain.Models;
using System;
using System.Linq;

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.SetExternalOriginatorAttribute
{
    public class when_setting_external_originator_attribute_sahl : WithCoreFakes
    {
        private static SetExternalOriginatorAttributeCommand command;
        private static SetExternalOriginatorAttributeCommandHandler handler;
        private static IApplicationDataManager applicationDataManager;

        private Establish context = () =>
        {
            applicationDataManager = An<IApplicationDataManager>();

            command = new SetExternalOriginatorAttributeCommand(1, OriginationSource.SAHomeLoans);
            handler = new SetExternalOriginatorAttributeCommandHandler(applicationDataManager, eventRaiser, unitOfWorkFactory);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_not_save_the_offer_attribute = () =>
        {
            applicationDataManager.WasNotToldTo(x => x.SaveExternalOriginatorAttribute(Param.IsAny<OfferAttributeDataModel>()));
        };

        private It should_return_a_message = () =>
        {
            messages.ErrorMessages().First().Message.ShouldEqual("The originator attribute can only be set for Comcorp or Capitec originated applications");
        };

        private It should_not_raise_an_external_originator_attribute_set_event = () =>
        {
            eventRaiser.WasNotToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Param.IsAny<ExternalOriginatorAttributeSetEvent>()
                , Param.IsAny<int>(), Param.IsAny<int>(), Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}