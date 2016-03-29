using BuildingBlocks.Timers;
using System.Linq;

namespace BuildingBlocks.Presenters.Admin
{
    public sealed class SubsidyProviderDetailsUpdate : SubsidyProviderBase
    {
        public override void Populate(Automation.DataModels.SubsidyProvider subsidyProvider)
        {
            base.SubsidyProvider.TypeText(subsidyProvider.LegalEntity.RegisteredName);
            this.SelectSubsidyProviderIfExist(subsidyProvider.LegalEntity.RegisteredName);
            base.Populate(subsidyProvider);
        }

        public override void PopulateWithoutAddress(Automation.DataModels.SubsidyProvider subsidyProvider)
        {
            base.SubsidyProvider.TypeText(subsidyProvider.LegalEntity.RegisteredName);
            this.SelectSubsidyProviderIfExist(subsidyProvider.LegalEntity.RegisteredName);
            base.PopulateWithoutAddress(subsidyProvider);
        }

        public override void PopulateWithoutAddressBoxNumber(Automation.DataModels.SubsidyProvider subsidyProvider)
        {
            base.SubsidyProvider.TypeText(subsidyProvider.LegalEntity.RegisteredName);
            this.SelectSubsidyProviderIfExist(subsidyProvider.LegalEntity.RegisteredName);
            base.PopulateWithoutAddressBoxNumber(subsidyProvider);
        }

        public override void PopulateWithoutAddressPostOffice(Automation.DataModels.SubsidyProvider subsidyProvider)
        {
            base.SubsidyProvider.TypeText(subsidyProvider.LegalEntity.RegisteredName);
            this.SelectSubsidyProviderIfExist(subsidyProvider.LegalEntity.RegisteredName);
            base.PopulateWithoutAddressPostOffice(subsidyProvider);
        }

        public override void EnterWrongEmail(Automation.DataModels.SubsidyProvider subsidyProvider)
        {
            base.SubsidyProvider.TypeText(subsidyProvider.LegalEntity.RegisteredName);
            this.SelectSubsidyProviderIfExist(subsidyProvider.LegalEntity.RegisteredName);
            base.EnterWrongEmail(subsidyProvider);
        }

        public override void ClearContactPerson(Automation.DataModels.SubsidyProvider subsidyProvider)
        {
            base.SubsidyProvider.TypeText(subsidyProvider.LegalEntity.RegisteredName);
            this.SelectSubsidyProviderIfExist(subsidyProvider.LegalEntity.RegisteredName);
            base.ClearContactPerson(subsidyProvider);
        }

        public override void ClearContactNumber(Automation.DataModels.SubsidyProvider subsidyProvider)
        {
            base.SubsidyProvider.TypeText(subsidyProvider.LegalEntity.RegisteredName);
            this.SelectSubsidyProviderIfExist(subsidyProvider.LegalEntity.RegisteredName);
            base.ClearContactNumber(subsidyProvider);
        }

        #region Helpers

        private void SelectSubsidyProviderIfExist(string providerName)
        {
            GeneralTimer.Wait(2000);
            var autoCompleteDev = (from div in base.SAHLAutoComplete_DefaultItem_Collection()
                                   where div.Text.Equals(providerName)
                                   select div).FirstOrDefault();
            if (autoCompleteDev != null)
            {
                autoCompleteDev.MouseDown();
                GeneralTimer.Wait(2000);
            }
        }

        #endregion Helpers
    }
}