using System.Collections.Generic;
using DomainService2.Specs.DomainObjects;
using DomainService2.Workflow.Origination.FurtherLending;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.CacheData;
using SAHL.Common.Collections;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.DataAccess;
using SAHL.Common.Globals;

namespace DomainService2.Specs.Workflow.Origination.FurtherLending.AddAccountMemoMessageOnReceiptOfApplicationCommandHandlerSpecs
{
    [Subject(typeof(AddAccountMemoMessageOnReceiptOfApplicationCommandHandler))]
    public class When_valid_cellno : WithFakes
    {
        static AddAccountMemoMessageOnReceiptOfApplicationCommand command;
        static AddAccountMemoMessageOnReceiptOfApplicationCommandHandler handler;
        static IDomainMessageCollection messages;
        static IMemoRepository memoRepository;
        static ICastleTransactionsService service;

        Establish context = () =>
        {
            IApplicationRepository applicationRepository = An<IApplicationRepository>();
            memoRepository = An<IMemoRepository>();
            IOrganisationStructureRepository organisationStructureRepository = An<IOrganisationStructureRepository>();
            ILookupRepository lookupRepository = An<ILookupRepository>();
            service = An<ICastleTransactionsService>();

            IApplication application = An<IApplication>();
            IMemo memo = An<IMemo>();
            IADUser adUser = An<IADUser>();
            IGenericKeyType genericKeyType = An<IGenericKeyType>();
            IAccountSequence reservedAccount = An<IAccountSequence>();
            IDictionary<GeneralStatuses, IGeneralStatus> generalStatuses = new Dictionary<GeneralStatuses, IGeneralStatus>();
            IEventList<IRole> roles = new StubEventList<IRole>();
            IAccount account = An<IAccount>();
            IRole role = An<IRole>();
            IRoleType roletype = An<IRoleType>();
            ILegalEntity legalEntity = An<ILegalEntity>();
            ISAHLPrincipalCacheProvider principalCacheProvider = An<ISAHLPrincipalCacheProvider>();
            ISAHLPrincipalCache principalCache = An<ISAHLPrincipalCache>();
            principalCacheProvider.WhenToldTo(x => x.GetPrincipalCache())
                .Return(principalCache);

            principalCache.WhenToldTo(x => x.ExclusionSets)
                .Return(new List<RuleExclusionSets>());

            applicationRepository.WhenToldTo(x => x.GetApplicationByKey(Param.IsAny<int>())).Return(application);
            application.WhenToldTo(x => x.ReservedAccount).Return(reservedAccount);
            reservedAccount.WhenToldTo(x => x.Key).Return(Param.IsAny<int>());

            memoRepository.WhenToldTo(x => x.CreateMemo()).Return(memo);

            organisationStructureRepository.WhenToldTo(x => x.GetAdUserForAdUserName(Param.IsAny<string>())).Return(adUser);

            lookupRepository.WhenToldTo(x => x.GenericKeyType.ObjectDictionary[Param.IsAny<string>()]).Return(genericKeyType);
            generalStatuses.Add(GeneralStatuses.Active, null);
            lookupRepository.WhenToldTo(x => x.GeneralStatuses).Return(generalStatuses);

            memoRepository.WhenToldTo(x => x.SaveMemo(Param.IsAny<IMemo>()));

            application.WhenToldTo(x => x.Account).Return(account);
            account.WhenToldTo(x => x.Roles).Return(roles);
            roles.Add(null, role);
            role.WhenToldTo(x => x.RoleType).Return(roletype);
            roletype.WhenToldTo(x => x.Key).Return((int)OfferRoleTypes.PreviousInsurer);
            role.WhenToldTo(x => x.LegalEntity).Return(legalEntity);
            legalEntity.WhenToldTo(x => x.CellPhoneNumber).Return("1111");

            messages = new DomainMessageCollection();
            command = new AddAccountMemoMessageOnReceiptOfApplicationCommand(Param.IsAny<int>(), Param.IsAny<string>(), Param.IsAny<string>());
            handler = new AddAccountMemoMessageOnReceiptOfApplicationCommandHandler(applicationRepository, organisationStructureRepository,
                                                                                    memoRepository, lookupRepository, service, principalCacheProvider);
        };

        Because of = () =>
        {
            handler.Handle(messages, command);
        };

        It should_save_client_email = () =>
        {
            service.WasToldTo(x => x.ExecuteQueryOnCastleTran(Param.IsAny<string>(), Param.IsAny<Databases>(), Param.IsAny<ParameterCollection>()));
        };
    }
}