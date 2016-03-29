using SAHL.Core.Testing.Config.UI;
using System;
namespace SAHL.UI.Halo.Tests.HaloUIConfigPredicates
{
    public class IsNonDynamicTilesWithDataProvidersPredicate : IHaloUIConfigPredicate
    {
        private const string ChildTile = "ChildTile";
        private const string RootTile = "RootTile";

        public Predicate<HaloUIConfigItem> Get()
        {
            return new Predicate<HaloUIConfigItem>((expectedConfigItem) =>
            {
                return expectedConfigItem.HasDataProvider && expectedConfigItem.DynamicTile == null && (expectedConfigItem.Type == ChildTile || expectedConfigItem.Type == RootTile);
            });
        }
    }
}
