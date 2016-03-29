using System;
using System.Collections.Generic;
namespace SAHL.Core.Testing.Config.UI
{
    public sealed class HaloUIConfigItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsAlternative { get; set; }
        public bool HasChildTileDrilldown { get; set; }
        public bool HasChildPageDrilldown { get; set; }
        public bool HasContentDataProvider { get; set; }
        public bool HasServiceDataProvider { get;set; }
        public bool HasDataProvider { get; set; }
        public string CommonDataProviderName { get; set; }
        public HaloUIConfigItem DynamicTile { get; set; }
        public List<HaloUIConfigItem> ChildTiles { get; set; }
        public List<HaloUIConfigItem> Actions { get; set; }
        public List<HaloUIConfigItem> Wizards { get; set; }
        public List<HaloUIConfigItem> RootTiles { get; set; }
        public List<HaloUIConfigItem> LinkedRootTiles { get; set; }
    }
}