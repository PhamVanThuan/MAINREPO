using System;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public abstract class HaloApplicationConfiguration : IHaloApplicationConfiguration
    {
        protected HaloApplicationConfiguration(string name, int sequence = 0)
        {
            if (string.IsNullOrWhiteSpace(name)) { throw new ArgumentNullException("name"); }

            this.Name = name;
            this.Sequence = sequence;
        }

        public string Name { get; private set; }

        public int Sequence { get; private set; }
    }
}