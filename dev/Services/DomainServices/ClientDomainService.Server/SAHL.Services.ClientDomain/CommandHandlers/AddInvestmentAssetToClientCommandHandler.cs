using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Data;
using SAHL.Core.Events;
using SAHL.Core.Services;
using SAHL.Core.SystemMessages;
using SAHL.Services.ClientDomain.Managers.AssetLiability;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Events;
using System;

namespace SAHL.Services.ClientDomain.CommandHandlers
{
    public class AddInvestmentAssetToClientCommandHandler : IDomainServiceCommandHandler<AddInvestmentAssetToClientCommand, InvestmentAssetAddedToClientEvent>
    {
        private IUnitOfWorkFactory unitOfWorkFactory;
        private IAssetLiabilityDataManager assetLiabilityDataManager;
        private IEventRaiser eventRaiser;
        public AddInvestmentAssetToClientCommandHandler(
              IAssetLiabilityDataManager assetLiabilityDataManager
            , IUnitOfWorkFactory unitOfWorkFactory
            , IEventRaiser eventRaiser
        )
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.assetLiabilityDataManager = assetLiabilityDataManager;
            this.eventRaiser = eventRaiser;
        }
        public Core.SystemMessages.ISystemMessageCollection HandleCommand(AddInvestmentAssetToClientCommand command, IServiceRequestMetadata metadata)
        {
            var systemMessages = SystemMessageCollection.Empty();
            using (var uow = unitOfWorkFactory.Build())
            {
                var assetliabilitykey = assetLiabilityDataManager.SaveInvestmentAsset(command.InvestmentAsset);
                var legalEntityAssetLiabilityKey = assetLiabilityDataManager.LinkAssetLiabilityToClient(command.ClientKey, assetliabilitykey);

                eventRaiser.RaiseEvent(
                       DateTime.Now
                    , new InvestmentAssetAddedToClientEvent(DateTime.Now, command.InvestmentAsset.InvestmentType, command.InvestmentAsset.CompanyName, command.InvestmentAsset.AssetValue)
                    , legalEntityAssetLiabilityKey
                    , (int)GenericKeyType.LegalEntityAssetLiability
                    , metadata
                );

                uow.Complete();
            }

            return systemMessages;
        }
    }
}
