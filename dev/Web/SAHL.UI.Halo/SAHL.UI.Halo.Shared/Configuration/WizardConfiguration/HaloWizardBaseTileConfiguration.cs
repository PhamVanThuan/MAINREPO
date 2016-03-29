using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public abstract class HaloWizardBaseTileConfiguration : IHaloWizardTileConfiguration
    {
        protected HaloWizardBaseTileConfiguration(string name, WizardType wizardType)
        {
            if (string.IsNullOrWhiteSpace(name)) { throw new ArgumentNullException("name"); }

            this.Name       = name;
            this.WizardType = wizardType;
        }

        public string Name { get; protected set; }
        public WizardType WizardType { get; protected set; }
    }
}
