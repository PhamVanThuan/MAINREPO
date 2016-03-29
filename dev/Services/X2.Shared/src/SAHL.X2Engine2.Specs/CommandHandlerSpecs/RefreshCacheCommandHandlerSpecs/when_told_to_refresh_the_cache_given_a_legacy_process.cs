using Machine.Fakes;
using Machine.Specifications;
using NSubstitute;
using SAHL.Core.SystemMessages;
using SAHL.Core.X2.AppDomain;
using SAHL.Core.X2.Messages;
using SAHL.X2Engine2.CommandHandlers;
using SAHL.X2Engine2.Commands;
using SAHL.X2Engine2.Node.Providers;
using SAHL.X2Engine2.Providers;
using SAHL.X2Engine2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAHL.X2Engine2.Specs.CommandHandlerSpecs.RefreshCacheCommandHandlerSpecs
{
    public class when_told_to_refresh_the_cache_given_a_legacy_process : WithFakes
    {
        private static StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<RefreshCacheCommandHandler> autoMocker;
        private static RefreshCacheCommand command;
        private static ISystemMessageCollection result;
        private static ProcessViewModel process;
        private static IX2TLSLegacyProcess legacyProcess;
        

        private Establish context = () =>
        {
            autoMocker = new StructureMap.AutoMocking.NSubstitute.NSubstituteAutoMocker<RefreshCacheCommandHandler>();
            IEnumerable<X2Process> processes = new List<X2Process>() { new X2Process("processName", new List<X2Workflow>()) };
            autoMocker.Get<IX2NodeConfigurationProvider>().WhenToldTo(x => x.GetAvailableProcesses()).Return(processes);
            process = new ProcessViewModel(1,"processName");
            IEnumerable<ProcessViewModel> processModels = new List<ProcessViewModel>(){process};
            autoMocker.Get<IWorkflowDataProvider>().WhenToldTo(x => x.GetConfiguredProcesses(Arg.Any<IEnumerable<string>>())).Return(processModels);
            legacyProcess = An<IX2TLSLegacyProcess>();
            autoMocker.Get<IProcessInstantiator>().WhenToldTo(x => x.GetProcess(process.ProcessID)).Return(legacyProcess);
            command = new RefreshCacheCommand("DomainService");
        };

        private Because of = () =>
        {
            result = autoMocker.ClassUnderTest.HandleCommand(command, null);
        };

        private It should_refresh_the_cache = () =>
        {
            legacyProcess.WasToldTo(x => x.ClearCache(Arg.Any<ISystemMessageCollection>(), command.Data));
        };
    }
}
