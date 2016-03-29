using SAHL.Core.Testing.Config.UI;
using System;
namespace SAHL.UI.Halo.Tests.HaloUIConfigPredicates
{
    public class IsAnyHaloModulePredicate : IHaloUIConfigPredicate
    {
        private const string HaloModule = "HaloModule";

        public Predicate<HaloUIConfigItem> Get()
        {
            return new Predicate<HaloUIConfigItem>((expectedConfigItem) =>
             {
                 return expectedConfigItem.Type == HaloModule;
             });
        }
    }
}
