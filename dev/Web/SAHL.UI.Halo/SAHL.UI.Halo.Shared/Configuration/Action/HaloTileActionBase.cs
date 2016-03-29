using System;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public abstract class HaloTileActionBase<TTile> : IHaloTileAction
        where TTile : IHaloTileConfiguration
    {
        private TTile tileConfiguration;

        protected HaloTileActionBase(string tileName, string iconName = "", string actionGroup = "", int sequence = 0)
        {
            this.Name              = tileName;
            this.IconName          = iconName;
            this.Group             = actionGroup;
            this.Sequence          = sequence;
            this.tileConfiguration = Activator.CreateInstance<TTile>();
        }

        public string Name { get; protected set; }
        public string IconName { get; protected set; }
        public string Group { get; protected set; }
        public int Sequence { get; protected set; }

        public Type TileConfigurationType
        {
            get { return typeof(TTile); }
        }

        public IHaloTileConfiguration TileConfiguration
        {
            get { return this.tileConfiguration; }
        }
    }
}
