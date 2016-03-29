using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.Capitec.Managers.Security;
using System;

namespace SAHL.Services.Capitec.Specs.SecurityDataServiceSpecs
{
    public class when_adding_a_branch : WithFakes
    {
        private static SecurityDataManager service;
        private static Guid suburbId;
        private static string branchName;
        private static string branchCode;
        private static bool isActive;
        private static FakeDbFactory dbFactory;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            service = new SecurityDataManager(dbFactory);
            suburbId = Guid.NewGuid();
            branchName = "Langkawi";
            isActive = true;
        };

        private Because of = () =>
        {
            service.AddBranch(branchName, isActive, suburbId, branchCode);
        };

        private It should_insert_a_branch_with_the_details_provided = () =>
        {
            dbFactory.FakedDb.DbContext.WasToldTo(x => x.Insert(Arg.Is<BranchDataModel>(f => f.BranchName == branchName && f.IsActive == isActive && f.SuburbId == suburbId)));
        };
    }
}