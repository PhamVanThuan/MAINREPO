using System;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public abstract class HaloModuleConfiguration : IHaloModuleConfiguration
    {
        protected HaloModuleConfiguration(string name, int sequence = 0, bool isTileBased = true, string nonTilePageState = "")
        {
            if (string.IsNullOrWhiteSpace(name)) { throw new ArgumentNullException("name"); }

            this.Name             = name;
            this.Sequence         = sequence;
            this.IsTileBased      = isTileBased;
            this.NonTilePageState = nonTilePageState;
        }

        public string Name { get; private set; }
        public int Sequence { get; private set; }
        public bool IsTileBased { get; private set; }
        public string NonTilePageState { get; private set; }
    }
}
