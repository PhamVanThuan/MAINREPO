using System;
using System.Linq;

namespace SAHL.UI.Halo.Shared.Configuration.LinkedRootTileConfiguration
{
    public abstract class HaloRootTileLinkConfiguration : IHaloRootTileLinkedConfiguration
    {
        protected HaloRootTileLinkConfiguration()
        {
            var linkedAttributes = this.GetType().GetCustomAttributes(typeof (HaloTileLinkedAttribute), true);
            if (linkedAttributes == null || !linkedAttributes.Any()) { return; }

            this.Name = ((HaloTileLinkedAttribute) linkedAttributes.First()).Name;
        }

        public string Name { get; protected set; }


    }
}
