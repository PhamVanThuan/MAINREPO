using Machine.Fakes;
using Machine.Specifications;
using SAHL.Common.BusinessModel.Interfaces.Service;
using SAHL.Common.BusinessModel.Repositories;
using SAHL.Common.CacheData;

namespace SAHL.Common.BusinessModel.Specs.Repositories.WorkflowAssignment.Caching.Offer
{
    public class When_loading_the_cached_dataset_for_the_first_time : WithFakes
    {
        static WorkflowSecurityRepository workflowSecurityRepo;
        static ICastleTransactionsService castleTransactionsService;
        static IUIStatementService uistatementService;

        Establish context = () =>
            {
                uistatementService = An<IUIStatementService>();
                castleTransactionsService = An<ICastleTransactionsService>();
                castleTransactionsService.WhenToldTo(x => x.ExecuteScalarOnCastleTran(Param.IsAny<string>(), Param.IsAny<SAHL.Common.Globals.Databases>(), Param.IsAny<DataAccess.ParameterCollection>()))
                    .Return("");
                workflowSecurityRepo = new WorkflowSecurityRepository(castleTransactionsService, uistatementService);
                WorkflowSecurityRepositoryCacheHelper.Instance.ClearCache();
            };

        Because of = () =>
            {
                workflowSecurityRepo.GetOfferRoleOrganisationStructure();
            };

        It should_use_the_castle_transaction_service = () =>
            {
                castleTransactionsService.WasToldTo(x => x.ExecuteScalarOnCastleTran(Param.IsAny<string>(), Param.IsAny<SAHL.Common.Globals.Databases>(), Param.IsAny<DataAccess.ParameterCollection>())).OnlyOnce();
            };
    }
}