using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Data;
using SAHL.Core.Data.Models.Capitec;
using SAHL.Core.Data.Models.Capitec.Statements;
using SAHL.Core.Testing.Fakes;
using SAHL.Core.Testing.Providers;
using SAHL.Services.Capitec.Managers.Security;
using System;
using System.Linq;

namespace SAHL.Services.Capitec.Specs.SecurityDataServiceSpecs
{
    public class when_checking_if_a_branch_exists_and_it_does_not : WithFakes
    {
        private static SecurityDataManager service;
        private static FakeDbFactory dbFactory;
        private static BranchDataModel model;
        private static bool result;

        private Establish context = () =>
        {
            result = true;
            dbFactory = new FakeDbFactory();
            service = new SecurityDataManager(dbFactory);
            model = null;
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.SelectOne(Param.IsAny<DoesBranchIdExistQuery>())).Return(model);
        };

        private Because of = () =>
        {
            result = service.DoesBranchIdExist(Param.IsAny<Guid>());
        };

        private It should_return_false = () =>
        {
            result.ShouldBeFalse();
        };
    }
}