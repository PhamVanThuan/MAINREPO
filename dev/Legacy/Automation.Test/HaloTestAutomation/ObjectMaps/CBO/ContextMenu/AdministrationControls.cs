using Common.Constants;
using ObjectMaps.NavigationControls;
using WatiN.Core;

namespace ObjectMaps.CBO
{
    public class AdministrationControls : BaseNavigation
    {
        #region Actions

        [FindBy(Title = "HALO Configuration")]
        protected Link HALOConfiguration { get; set; }

        [FindBy(Title = "X2 Configuration")]
        protected Link X2Configuration { get; set; }

        [FindBy(Title = "Flush Cache")]
        protected Link FlushCache { get; set; }

        [FindBy(Title = "Estate Agent Maintenance")]
        protected Link EstateAgentMaintenance { get; set; }

        [FindBy(Title = Features.UserStatusMaintenance)]
        protected Link UserStatusMaintenance { get; set; }

        [FindBy(Id = "ctl00_tabsMenu_pnlMenu_ctl03_External Organisation Structures_img")]
        protected Image ExternalOrganisationStructures { get; set; }

        [FindBy(Title = "Payment Distribution Agents")]
        protected Link PaymentDistributionAgents { get; set; }

        [FindBy(Title = "Market Rates")]
        protected Link MarketRates { get; set; }

        [FindBy(Title = "Update Market Rates")]
        protected Link UpdateMarketRates { get; set; }

        [FindBy(Text = "Attorney Details")]
        protected Link AttorneyDetails { get; set; }

        [FindBy(Text = "Add Attorney Details")]
        public Link AddAttorneyDetails { get; set; }

        [FindBy(Text = "Update Attorney Details")]
        protected Link UpdateAttorneyDetails { get; set; }

        [FindBy(Text = "Attorney Invoice")]
        protected Link AttorneyInvoice { get; set; }

        [FindBy(Text = "Add Subsidy Provider Details")]
        protected Link AddSubsidyProvider { get; set; }

        [FindBy(Text = "Update Subsidy Provider Details")]
        protected Link UpdateSubsidyProvider { get; set; }

        [FindBy(Text = "Subsidy Provider Details")]
        protected Link SubsidyProviderDetails { get; set; }

        [FindBy(Text = "Employer Details")]
        protected Link EmployerDetails { get; set; }

        [FindBy(Text = "Add Employer Details")]
        protected Link AddEmployerDetails { get; set; }

        [FindBy(Text = "Update Employer Details")]
        protected Link UpdateEmployerDetails { get; set; }

        [FindBy(Text = "Marketing Source Maintenance")]
        protected Link MarketingSourceMaintenance { get; set; }

        [FindBy(Text = "Add Marketing Source")]
        protected Link AddMarketingSource { get; set; }

        [FindBy(Text = "Update Marketing Source")]
        protected Link UpdateMarketingSource { get; set; }

        [FindBy(Text = "View My Profile Details")]
        protected Link ViewMyProfileDetails { get; set; }

        [FindBy(Text = "Update My Profile Details")]
        protected Link UpdateMyProfileDetails { get; set; }

        #endregion Actions
    }
}