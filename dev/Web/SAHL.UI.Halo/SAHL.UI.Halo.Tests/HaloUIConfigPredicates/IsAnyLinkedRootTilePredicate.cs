using SAHL.Core.Testing.Config.UI;
using System;
namespace SAHL.UI.Halo.Tests.HaloUIConfigPredicates
{
    public class IsAnyLinkedRootTilePredicate : IHaloUIConfigPredicate
    {
        private const string LinkedRootTile = "LinkedRootTile";

        public Predicate<HaloUIConfigItem> Get()
        {
            return new Predicate<HaloUIConfigItem>((expectedConfigItem) =>
             {
                 return expectedConfigItem.Type == LinkedRootTile;
             });
        }
    }
}
