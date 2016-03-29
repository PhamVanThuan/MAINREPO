using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
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

namespace SAHL.Services.ApplicationDomain.Specs.CommandHandlersSpec.SetExternalOriginatorAttribute
{
    public class when_setting_external_originator_attribute_capitec : WithCoreFakes
    {
        private static SetExternalOriginatorAttributeCommand command;
        private static SetExternalOriginatorAttributeCommandHandler handler;
        private static IApplicationDataManager applicationDataManager;

        private Establish context = () =>
        {
            applicationDataManager = An<IApplicationDataManager>();
            command = new SetExternalOriginatorAttributeCommand(1, OriginationSource.Capitec);
            handler = new SetExternalOriginatorAttributeCommandHandler(applicationDataManager, eventRaiser, unitOfWorkFactory);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_save_the_offer_attribute = () =>
        {
            applicationDataManager.WasToldTo(x => x.SaveExternalOriginatorAttribute(Param.IsAny<OfferAttributeDataModel>()));
        };

        private It should_not_have_any_errors = () =>
        {
            messages.HasErrors.ShouldBeFalse();
        };

        private It should_raise_an_external_originator_attribute_set_event = () =>
        {
            eventRaiser.WasToldTo(x => x.RaiseEvent(Param.IsAny<DateTime>(), Arg.Is<ExternalOriginatorAttributeSetEvent>
                (y => y.OriginationSourceKey == (int)command.OriginationSource &&
                    y.ApplicationNumber == command.ApplicationNumber), command.ApplicationNumber, (int)GenericKeyType.Offer, Param.IsAny<IServiceRequestMetadata>()));
        };
    }
}