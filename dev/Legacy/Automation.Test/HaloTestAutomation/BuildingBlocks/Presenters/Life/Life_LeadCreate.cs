using Common.Extensions;
using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.Life
{
    public class Life_LeadCreate : LifeLeadCreateControls
    {
        /// <summary>
        /// Will do a search and select the first valid client from the list
        /// </summary>
        public void SearchAndCreateLead(string accountKey, string lifeConsultant)
        {
            base.ctl00MaintxtAccountNumber.AppendText(accountKey);
            base.ctl00MainbtnSearch.Click();
            base.ctl00MainSearchGridctl01ctl00.Checked = true;
            lifeConsultant = lifeConsultant.RemoveDomainPrefix();
            base.ctl00MainddlConsultant.Option(new System.Text.RegularExpressions.Regex(lifeConsultant)).Select();
            base.ctl00MainbtnCreateLeads.Click();
        }
    }
}