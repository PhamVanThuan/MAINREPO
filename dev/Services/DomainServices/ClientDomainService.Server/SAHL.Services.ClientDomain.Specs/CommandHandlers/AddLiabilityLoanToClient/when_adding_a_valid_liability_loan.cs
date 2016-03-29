using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.BusinessModel.Enums;
using SAHL.Core.Rules;
using SAHL.Core.Services;
using SAHL.Core.Testing;
using SAHL.Services.ClientDomain.CommandHandlers;
using SAHL.Services.ClientDomain.Managers.AssetLiability;
using SAHL.Services.Interfaces.ClientDomain.Commands;
using SAHL.Services.Interfaces.ClientDomain.Events;
using SAHL.Services.Interfaces.ClientDomain.Models;
using System;

namespace SAHL.Services.ClientDomain.Specs.CommandHandlers.AddLiabilityLoanToClient
{
    public class when_adding_a_valid_liability_loan : WithCoreFakes
    {
        private static AddLiabilityLoanToClientCommandHandler handler;
        private static AddLiabilityLoanToClientCommand command;
        private static int clientKey;
        private static LiabilityLoanModel liabilityLoan;
        private static IDomainRuleManager<LiabilityLoanModel> domainRuleManager;
        private static IAssetLiabilityDataManager assetLiabilityDataManager;
        private static int assetLiabilityKey, legalEntityAssetLiabilityKey;

        Establish context = () =>
        {
            domainRuleManager = An<IDomainRuleManager<LiabilityLoanModel>>();
            assetLiabilityDataManager = An<IAssetLiabilityDataManager>();

            handler = new AddLiabilityLoanToClientCommandHandler(unitOfWorkFactory, eventRaiser, assetLiabilityDataManager);

            clientKey = 1111;
            liabilityLoan = new LiabilityLoanModel(AssetLiabilitySubType.PersonalLoan, "Financial institution", DateTime.Now, 0d, 1d);
            command = new AddLiabilityLoanToClientCommand(clientKey, liabilityLoan);

            assetLiabilityKey = 2222;
            assetLiabilityDataManager.WhenToldTo(x => x.SaveLiabilityLoan(Param.IsAny<LiabilityLoanModel>())).Return(assetLiabilityKey);

            legalEntityAssetLiabilityKey = 3333;
            assetLiabilityDataManager.WhenToldTo
                (x => x.LinkAssetLiabilityToClient(Param.IsAny<int>(), Param.IsAny<int>())).Return(legalEntityAssetLiabilityKey);
        };

        Because of = () =>
        {
            messages = handler.HandleCommand(command, serviceRequestMetaData);
        };

        private It should_begin_unit_of_work = () =>
        {
            unitOfWorkFactory.WasToldTo(x => x.Build());
        };

        private It should_add_fixed_property_asset = () =>
        {
            assetLiabilityDataManager.WasToldTo(x => x.SaveLiabilityLoan(Arg.Is<LiabilityLoanModel>
                (y => y.FinancialInstitution.Equals(liabilityLoan.FinancialInstitution))));
        };

        private It should_link_fixed_property_asset_to_client = () =>
        {
            assetLiabilityDataManager.WasToldTo(x => x.LinkAssetLiabilityToClient(clientKey, assetLiabilityKey));
        };

        private It should_raise_a_fixed_property_asset_added_event = () =>
        {
            eventRaiser.WasToldTo
                (x => x.RaiseEvent(Param.IsAny<DateTime>(), Arg.Is<LiabilityLoanAddedToClientEvent>
                    (y => y.FinancialInstitution == liabilityLoan.FinancialInstitution), 
                        legalEntityAssetLiabilityKey, (int)GenericKeyType.LegalEntityAssetLiability, Param.IsAny<IServiceRequestMetadata>()));
        };

        private It should_complete_unit_of_work = () =>
        {
            unitOfWork.WasToldTo(x => x.Complete());
        };
    }
}
