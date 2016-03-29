using ObjectMaps.CBO;

namespace BuildingBlocks.Navigation.CBO
{
    public class AdministrationActions : AdministrationControls
    {
        public void FlushCache()
        {
            base.FlushCache.Click();
        }

        public void UserStatusMaintenance()
        {
            base.UserStatusMaintenance.Click();
        }

        public void PaymentDistributionAgents()
        {
            base.ExternalOrganisationStructures.Click();
            base.PaymentDistributionAgents.Click();
        }

        public void UpdateMarketRates()
        {
            base.MarketRates.Click();
            base.UpdateMarketRates.Click();
        }

        public void AttorneyInvoice()
        {
            base.AttorneyInvoice.Click();
        }

        public void AttorneyDetails()
        {
            base.AttorneyDetails.Click();
        }

        public void AddAttorneyDetails()
        {
            base.AddAttorneyDetails.Click();
        }

        public void UpdateAttorneyDetails()
        {
            base.UpdateAttorneyDetails.Click();
        }

        public void UpdateSubsidyProvider()
        {
            base.SubsidyProviderDetails.Click();
            base.UpdateSubsidyProvider.Click();
        }

        public void AddSubsidyProvider()
        {
            base.SubsidyProviderDetails.Click();
            base.AddSubsidyProvider.Click();
        }

        public void AddEmployerDetails()
        {
            base.EmployerDetails.Click();
            base.AddEmployerDetails.Click();
        }

        public void UpdateEmployerDetails()
        {
            base.EmployerDetails.Click();
            base.UpdateEmployerDetails.Click();
        }

        public void EstateAgentMaintenance()
        {
            base.EstateAgentMaintenance.Click();
        }

        public void UpdateMarketingSource()
        {
            base.MarketingSourceMaintenance.Click();
            base.UpdateMarketingSource.Click();
        }

        public void AddMarketingSource()
        {
            base.MarketingSourceMaintenance.Click();
            base.AddMarketingSource.Click();
        }

        public void UpdateMyProfileDetails()
        {
            ViewMyProfileDetails();
            base.UpdateMyProfileDetails.Click();
        }

        public void ViewMyProfileDetails()
        {
            base.ViewMyProfileDetails.Click();
        }
    }
}