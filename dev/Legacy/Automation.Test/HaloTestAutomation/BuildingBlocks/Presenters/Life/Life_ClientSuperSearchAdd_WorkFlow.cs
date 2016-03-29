using BuildingBlocks.Presenters.LegalEntity;
using BuildingBlocks.Services.Contracts;
using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.Life
{
    public class Life_ClientSuperSearchAdd_WorkFlow : LifeClientSuperSearchAddWorkFlowControls
    {
        private ICommonService commonService;

        public Life_ClientSuperSearchAdd_WorkFlow()
        {
            commonService = ServiceLocator.Instance.GetService<ICommonService>();
        }

        /// <summary>
        /// This method will search for an existing legalentity and add it to the life offer.
        /// </summary>
        /// <param name="b">Instance of the TestBrowser</param>
        /// <param name="IDNumber">LegalEntiy IDNumber</param>
        public void AddExistingLegalEntity(string IDNumber)
        {
            base.BasicSearch.Click();
            base.ctl00MaintxtID.Value = IDNumber;
            base.ctl00MainbtnSearchBasic.Click();
            base.Document.DomContainer.WaitForComplete();
            base.ctl00MaingridSearchResults.Links[1].Click();
        }

        /// <summary>
        /// This method will click on the NewLegalEntity button, capture the legal entity detail and add it to the life offer.
        /// </summary>
        /// <param name="b"></param>
        /// <param name="id"></param>
        /// <param name="salutation"></param>
        /// <param name="initials"></param>
        /// <param name="firstname"></param>
        /// <param name="surname"></param>
        /// <param name="preferredName"></param>
        /// <param name="gender"></param>
        /// <param name="maritalStatus"></param>
        /// <param name="populationGroup"></param>
        /// <param name="education"></param>
        /// <param name="citizenshipType"></param>
        /// <param name="passportNumber"></param>
        /// <param name="taxNumber"></param>
        /// <param name="homeLanguage"></param>
        /// <param name="documentLanguage"></param>
        /// <param name="status"></param>
        /// <param name="homeCode"></param>
        /// <param name="homeNumber"></param>
        /// <param name="workCode"></param>
        /// <param name="workNumber"></param>
        /// <param name="faxCode"></param>
        /// <param name="faxNumber"></param>
        /// <param name="cellNumber"></param>
        /// <param name="emailAddress"></param>
        /// <param name="telemarketing"></param>
        /// <param name="marketing"></param>
        /// <param name="customerList"></param>
        /// <param name="email"></param>
        /// <param name="sms"></param>
        /// <param name="InsurableInterest"></param>
        public void AddNewLegalEntity(TestBrowser browser, string id, string salutation, string initials, string firstname, string surname,
            string preferredName, string gender, string maritalStatus, string populationGroup,
            string education, string citizenshipType, string passportNumber, string taxNumber,
            string homeLanguage, string documentLanguage, string status, string homeCode, string homeNumber,
            string workCode, string workNumber, string faxCode, string faxNumber, string cellNumber,
            string emailAddress, bool telemarketing, bool marketing, bool customerList, bool email, bool sms, string InsurableInterest)
        {
            base.ctl00MainbtnNewLegalEntity.Click();

            browser.Page<LegalEntityDetails>().AddLegalEntity
                (
                    false, id, null, false, salutation, initials, firstname, surname,
                    preferredName, gender, maritalStatus, populationGroup,
                    education, citizenshipType, passportNumber, taxNumber,
                    homeLanguage, documentLanguage, status, homeCode, homeNumber,
                    workCode, workNumber, faxCode, faxNumber, cellNumber,
                    emailAddress, telemarketing, marketing, customerList, email, sms,
                    InsurableInterest, commonService.GetDateOfBirthFromIDNumber(id)
                );
        }
    }
}