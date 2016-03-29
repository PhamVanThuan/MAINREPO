using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public enum WizardPageType
    {
        Edit,
        YesNo
    }

    public interface IHaloWizardTilePageConfiguration
    {
        string Name { get; }
        WizardPageType WizardPageType { get; }
        int Sequence { get; }
        string ContentMessage { get; }
    }

    public interface IHaloWizardTilePageConfiguration<T> : IHaloWizardTilePageConfiguration 
        where T : IHaloWizardTileConfiguration
    {
    }
}
