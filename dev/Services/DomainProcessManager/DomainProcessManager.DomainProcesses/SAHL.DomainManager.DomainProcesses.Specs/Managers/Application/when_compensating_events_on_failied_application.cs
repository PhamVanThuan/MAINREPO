using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Testing;
using SAHL.Core.Testing.Fakes;
using SAHL.DomainProcessManager.DomainProcesses.Managers.Application;
using SAHL.DomainProcessManager.DomainProcesses.Managers.Application.Statements;
using System.Collections.Generic;

namespace SAHL.DomainProcessManager.DomainProcesses.Specs.Managers.Application
{
    public class when_compensating_events_on_failied_application : WithCoreFakes
    {
        private static ApplicationDataManager manager;
        private static FakeDbFactory dbFactory;
        private static List<int> employmentKeys = new List<int> { 123, 546 };
        private static int applicationNumber = 123456;

        private Establish context = () =>
        {
            dbFactory = new FakeDbFactory();
            manager = new ApplicationDataManager(dbFactory);
        };

        private Because of = () =>
        {
            manager.RollbackCriticalPathApplicationData(applicationNumber, employmentKeys);
        };

        private It should_execute_the_rollback_transaction_procedure = () =>
        {
            dbFactory.FakedDb.InAppContext().WasToldTo(x => x.ExecuteNonQuery(Param<RollbackApplicationStatement>.Matches(y =>
                y.ApplicationNumber == applicationNumber
                && y.EmploymentKeys == string.Join(",", employmentKeys)
                )));
        };

        private It should_complete_the_transaction = () =>
        {
            dbFactory.FakedDb.InAppContext().WasToldTo(x => x.Complete());
        };

    }
}