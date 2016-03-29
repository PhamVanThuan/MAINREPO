using SAHL.Core.Testing.Config.UI;
using System;
namespace SAHL.UI.Halo.Tests.HaloUIConfigPredicates
{
    public interface IHaloUIConfigPredicate 
    {
        Predicate<HaloUIConfigItem> Get();
    }
}
