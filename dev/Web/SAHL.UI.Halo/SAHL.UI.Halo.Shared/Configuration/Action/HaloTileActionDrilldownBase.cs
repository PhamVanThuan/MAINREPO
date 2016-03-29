using System;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public abstract class HaloTileActionDrilldownBase<TTile, TRootTile> : HaloTileActionBase<TTile>, IHaloTileActionDrilldown
        where TTile : IHaloTileConfiguration
        where TRootTile : IHaloRootTileConfiguration
    {
        private TRootTile rootTileConfiguration;

        protected HaloTileActionDrilldownBase(string actionName) : base (actionName)
        {
            this.rootTileConfiguration = Activator.CreateInstance<TRootTile>();
        }

        public Type RootTileType
        {
            get { return typeof(TRootTile); }
        }

        public IHaloRootTileConfiguration RootTileConfiguration
        {
            get { return this.rootTileConfiguration; }
        }
    }
}
