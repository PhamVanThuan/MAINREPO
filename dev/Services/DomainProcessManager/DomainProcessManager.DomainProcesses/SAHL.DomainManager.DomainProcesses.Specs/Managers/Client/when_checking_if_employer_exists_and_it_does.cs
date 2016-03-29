using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.DomainProcessManager.DomainProcesses.Managers.Client.Statements;
using SAHL.Services.DomainProcessManager.DomainProcesses.Managers.Client;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.Managers.Client
{
    public class when_checking_if_employer_exists_and_it_does : WithCoreFakes
    {
        private static FakeDbFactory dbFactory;
        private static ClientDataManager clientDataManager;
        private static string employerName;
        private static int employerKey;
        private static int? result;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            clientDataManager = new ClientDataManager(dbFactory);
            employerName = "ACME";
            employerKey = 114;
            dbFactory.FakedDb.DbReadOnlyContext.WhenToldTo(x => x.SelectOne(Param.IsAny<DoesEmployerExistStatement>())).Return(employerKey);
        };

        private Because of = () =>
        {
            result = clientDataManager.GetEmployerKey(employerName);
        };

        private It should_query_if_the_employer_exists = () =>
        {
            dbFactory.FakedDb.DbReadOnlyContext.WasToldTo(x => x.SelectOne(Param<DoesEmployerExistStatement>.Matches(m => m.EmployerName == employerName)));
        };

        private It should_return_the_employer_key = () =>
        {
            result.ShouldEqual(employerKey);
        };
    }
}