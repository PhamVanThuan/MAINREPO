using SAHL.Core.Testing.Config.UI;
using System;
namespace SAHL.UI.Halo.Tests.HaloUIConfigPredicates
{
    public class IsAnyRootTileWithLinkedRootTilesPredicate : IHaloUIConfigPredicate
    {
        private const string RootTile = "RootTile";

        public Predicate<HaloUIConfigItem> Get()
        {
            return new Predicate<HaloUIConfigItem>((expectedConfigItem) =>
             {
                 return expectedConfigItem.Type == RootTile && expectedConfigItem.LinkedRootTiles.Count > 0;
             });
        }
    }
}
