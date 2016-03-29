using System;

namespace SAHL.UI.Halo.Shared.Configuration
{
    public abstract class HaloTileWizardActionBase<TTile, TWizard> : HaloTileActionBase<TTile>, IHaloTileActionWizard
        where TTile : IHaloTileConfiguration
        where TWizard : IHaloWizardTileConfiguration
    {
        protected HaloTileWizardActionBase(string tileName, string iconName = "", string actionGroup = "", int sequence = 0, string contextData = null)
            : base(tileName, iconName, actionGroup, sequence)
        {
            this.WizardTileConfiguration = Activator.CreateInstance<TWizard>();
            this.ContextData = contextData;
        }

        public IHaloWizardTileConfiguration WizardTileConfiguration
        { get; private set; }

        public string ContextData
        { get; private set; }
    }
}