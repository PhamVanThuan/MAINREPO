using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public enum WizardType
    {
        Sequential,
    }

    public interface IHaloWizardTileConfiguration
    {
        string Name { get; }
        WizardType WizardType { get; } 
    }
}
