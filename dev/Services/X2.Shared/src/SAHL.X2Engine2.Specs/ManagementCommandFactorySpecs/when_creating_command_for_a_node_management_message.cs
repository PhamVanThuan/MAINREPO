using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Services;
using SAHL.Core.X2.Messages;
using SAHL.Core.X2.Messages.Management;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.Specs.ManagementCommandFactorySpecs
{
    public class when_creating_command_for_a_node_management_message : WithFakes
    {
        private static StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<ManagementCommandFactory> autoMocker;
        private static IX2NodeManagementMessage message;
        private static IEnumerable<IServiceCommand> result;

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<ManagementCommandFactory>();
            message = new X2NodeManagementMessage(X2ManagementType.RefreshCache, "DomainService");
        };

        private Because of = () =>
        {
            result = autoMocker.ClassUnderTest.CreateCommands(message);
        };

        private It should_return_command_for_refresh_cache = () =>
        {
            result.First().ShouldBeOfType(typeof(RefreshCacheCommand));
        };
    }
}
