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
    public class AddLiabilityLoanToClientCommandHandler : IDomainServiceCommandHandler<AddLiabilityLoanToClientCommand, LiabilityLoanAddedToClientEvent>
    {

        private IAssetLiabilityDataManager assetLiabilityDataManager;
        private IUnitOfWorkFactory unitOfWorkFactory;
        private IEventRaiser eventRaiser;

        public AddLiabilityLoanToClientCommandHandler(
            IUnitOfWorkFactory unitOfWorkFactory
            , IEventRaiser eventRaiser
            , IAssetLiabilityDataManager assetLiabilityDataManager
            )
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.assetLiabilityDataManager = assetLiabilityDataManager;
            this.eventRaiser = eventRaiser;
        }

        public Core.SystemMessages.ISystemMessageCollection HandleCommand(AddLiabilityLoanToClientCommand command, IServiceRequestMetadata metadata)
        {
            var messages = SystemMessageCollection.Empty();

            using (var uow = unitOfWorkFactory.Build())
            {
                var assetLiabilityKey = assetLiabilityDataManager.SaveLiabilityLoan(command.LiabilityLoan);
                var legalEntityAssetLiabilityKey = assetLiabilityDataManager.LinkAssetLiabilityToClient(command.ClientKey, assetLiabilityKey);
                var liabilityLoanAddedToClientEvent = new LiabilityLoanAddedToClientEvent(DateTime.Now, command.LiabilityLoan.LoanType, command.LiabilityLoan.FinancialInstitution, 
                    command.LiabilityLoan.DateRepayable, command.LiabilityLoan.InstalmentValue, command.LiabilityLoan.LiabilityValue);

                eventRaiser.RaiseEvent(DateTime.Now, liabilityLoanAddedToClientEvent,
                    legalEntityAssetLiabilityKey, (int)GenericKeyType.LegalEntityAssetLiability, metadata);

                uow.Complete();
            }

            return messages;
        }
    }
}