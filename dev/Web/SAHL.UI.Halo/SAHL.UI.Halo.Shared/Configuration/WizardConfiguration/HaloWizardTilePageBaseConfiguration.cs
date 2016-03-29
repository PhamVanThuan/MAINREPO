using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public abstract class HaloWizardTilePageBaseConfiguration : IHaloWizardTilePageConfiguration
    {
        protected HaloWizardTilePageBaseConfiguration(string name, WizardPageType wizardPageType, int sequence = 0, string contentMessage = "")
        {
            if (string.IsNullOrWhiteSpace(name)) { throw new ArgumentNullException("name"); }

            this.Name           = name;
            this.WizardPageType = wizardPageType;
            this.Sequence       = sequence;
            this.ContentMessage = contentMessage;
        }

        public string Name { get; protected set; }
        public WizardPageType WizardPageType { get; protected set; }
        public int Sequence { get; protected set; }
        public string ContentMessage { get; protected set; }
    }
}
