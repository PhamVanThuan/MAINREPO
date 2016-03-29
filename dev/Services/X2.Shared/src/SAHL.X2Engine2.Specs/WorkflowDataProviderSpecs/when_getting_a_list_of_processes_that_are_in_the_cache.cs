﻿using System.Collections.Generic;
using System.Linq;
using Machine.Fakes;
using Machine.Specifications;
using SAHL.Core.Caching;
using SAHL.X2Engine2.Providers;
using SAHL.X2Engine2.ViewModels;
using StructureMap.AutoMocking;
using StructureMap.AutoMocking.NSubstitute;

namespace SAHL.X2Engine2.Specs.WorkflowDataProviderSpecs
{
    public class when_getting_a_list_of_processes_that_are_in_the_cache : WithFakes
    {
        static AutoMocker<WorkflowDataProvider> automocker = new NSubstituteAutoMocker<WorkflowDataProvider>();
        private static ProcessViewModel process;
        private static List<ProcessViewModel> returnedModels = new List<ProcessViewModel>();
        static IEnumerable<string> configuredProcessNames;

        Establish context = () =>
        {
            configuredProcessNames = new List<string>(new string[] { "processName" });
            process = new ProcessViewModel(1, "processName");
            returnedModels.Add(process);

            automocker.Get<ICacheKeyGenerator>().WhenToldTo(x => x.GetKey<INamedCacheKey, IEnumerable<ProcessViewModel>>(Param.IsAny<INamedCacheKey>())).Return("Key");
            automocker.Get<ICache>().WhenToldTo(x => x.Contains("Key")).Return(true);
            automocker.Get<ICache>().WhenToldTo(x => x.GetItem<IEnumerable<ProcessViewModel>>("Key")).Return(returnedModels);
        };

        Because of = () =>
        {
            returnedModels = automocker.ClassUnderTest.GetConfiguredProcesses(configuredProcessNames).ToList();
        };

        It should_get_them_from_the_cache = () =>
        {
            automocker.Get<ICache>().WasToldTo(x => x.GetItem<IEnumerable<ProcessViewModel>>("Key"));
        };

        It should_return_the_correct_process = () =>
        {
            returnedModels.ShouldContain<ProcessViewModel>(x => x.ProcessName == "processName" && x.ProcessID == 1);
        };
    }
}