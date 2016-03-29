using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Data.Models.Capitec.Statements;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.Capitec.Managers.Security;
using System;

namespace SAHL.Services.Capitec.Specs.SecurityDataServiceSpecs
{
    public class when_changing_the_details_for_a_branch : WithFakes
    {
        private static SecurityDataManager service;
        private static Guid branchId, suburbId;
        private static string branchName;
        private static bool isActive;
        private static FakeDbFactory dbFactory;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            service = new SecurityDataManager(dbFactory);
            branchId = Guid.NewGuid();
            suburbId = Guid.NewGuid();
            branchName = "Test";
            isActive = true;
        };

        private Because of = () =>
        {
            service.ChangeBranchDetails(branchId, branchName, isActive, suburbId);
        };

        private It should_update_the_branch_with_the_details_provided = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Update<BranchDataModel>(Arg.Is<ChangeBranchDetailsQuery>(
                y => y.Id == branchId && y.BranchName == branchName && y.SuburbId == suburbId && y.IsActive == isActive
            )));
        };
    }
}