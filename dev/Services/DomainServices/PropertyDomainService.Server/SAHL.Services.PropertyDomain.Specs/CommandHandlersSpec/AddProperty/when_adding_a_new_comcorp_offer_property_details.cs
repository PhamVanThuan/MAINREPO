using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data.Models._2AM;
using SAHL.Core.Events;
using SAHL.Core.Testing;
using SAHL.Services.Interfaces.PropertyDomain.Commands;
using SAHL.Services.Interfaces.PropertyDomain.Models;
using SAHL.Services.PropertyDomain.CommandHandlers;
using SAHL.Services.PropertyDomain.Managers;
using System;

namespace SAHL.Services.PropertyDomain.Specs.CommandHandlersSpec.AddProperty
{
    public class when_adding_a_new_comcorp_offer_property_details : WithCoreFakes
    {
        private static IPropertyDataManager propertyDataManager;
        private static IEventRaiser eventRaiser;
        private static AddComcorpOfferPropertyDetailsCommand command;
        private static AddComcorpOfferPropertyDetailsCommandHandler handler;
        private static ComcorpOfferPropertyDetailsModel comcorpOfferPropertyDetailsModel;
        private static ComcorpOfferPropertyDetailsDataModel comcorpOfferPropertyDetailsDataModel;

        private Establish context = () =>
        {
            propertyDataManager = An<IPropertyDataManager>();
            eventRaiser = An<IEventRaiser>();
            comcorpOfferPropertyDetailsModel = new ComcorpOfferPropertyDetailsModel(
                 null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);

            comcorpOfferPropertyDetailsDataModel = null;

            propertyDataManager.WhenToldTo(x => x.FindExistingComcorpOfferPropertyDetails(Param.IsAny<int>())).Return(comcorpOfferPropertyDetailsDataModel);

            command = new AddComcorpOfferPropertyDetailsCommand(0, comcorpOfferPropertyDetailsModel);
            handler = new AddComcorpOfferPropertyDetailsCommandHandler(propertyDataManager, eventRaiser);
        };

        private Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_not_return_any_messages = () =>
        {
            messages.AllMessages.ShouldBeEmpty();
        };

        private It should_save_the_property = () =>
        {
            propertyDataManager.WasToldTo(x => x.InsertComcorpOfferPropertyDetails(Param.IsAny<ComcorpOfferPropertyDetailsDataModel>()));
        };
    }
}