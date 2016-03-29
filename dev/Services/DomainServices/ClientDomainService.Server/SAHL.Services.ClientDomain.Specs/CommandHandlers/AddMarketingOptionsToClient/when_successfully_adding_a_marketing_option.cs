using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Services.ClientDomain.CommandHandlers;
using SAHL.Services.ClientDomain.Managers.AssetLiability;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Events;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;
using SAHL.Services.ClientDomain.Managers;
using System.Collections.Generic;
using SAHL.Core.Data.Models._2AM;

namespace SAHL.Services.ClientDomain.Specs.CommandHandlers.AddLifeAssuranceAssetToClient
{
    public class when_successfully_adding_a_marketing_option : WithCoreFakes
    {
        private static AddMarketingOptionsToClientCommandHandler handler;
        private static AddMarketingOptionsToClientCommand command;
        private static int clientKey, marketingOptionKey;
        private static MarketingOptionModel marketingOptionModel;
        private static IClientDataManager clientDataManager;
        private static List<MarketingOptionModel> marketingOptionsList;
       
        Establish context = () =>
        {
            clientDataManager = An<IClientDataManager>();
            handler = new AddMarketingOptionsToClientCommandHandler(clientDataManager, eventRaiser, unitOfWorkFactory);
            clientKey = 1234567;
            marketingOptionKey = 8;
            marketingOptionModel = new MarketingOptionModel(8,"x2");
            marketingOptionsList = new List<MarketingOptionModel>();
            marketingOptionsList.Add(marketingOptionModel);
            command = new AddMarketingOptionsToClientCommand(marketingOptionsList, clientKey);           
            unitOfWorkFactory.WhenToldTo(x => x.Build()).Return(unitOfWork);
            clientDataManager.WhenToldTo(x => x.DoesClientMarketingOptionExist(clientKey, marketingOptionKey)).Return(false);
        };

        Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_begin_unit_of_work = () =>
        {
            unitOfWorkFactory.WasToldTo(x => x.Build());
        };

        private It should_check_if_marketing_option_exists_for_client = () =>
        {
            clientDataManager.WasToldTo(x => x.DoesClientMarketingOptionExist(clientKey, marketingOptionKey));
        };

        private It should_add_marketing_options = () =>
        {
            clientDataManager.WasToldTo(x => x.AddNewMarketingOptions(Param.IsAny<LegalEntityMarketingOptionDataModel>()));
        };

        private It should_raise_a_marketing_option_added_event = () =>
        {
            eventRaiser.WasToldTo
                (x => x.RaiseEvent(Param.IsAny<DateTime>(), Arg.Is<MarketingOptionsAddedEvent>
                    (y => y.MarketingOptionCollection == marketingOptionsList), clientKey
                       , (int)GenericKeyType.LegalEntity, Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_complete_unit_of_work = () =>
        {
            unitOfWork.WasToldTo(x => x.Complete());
        };
    }
}
