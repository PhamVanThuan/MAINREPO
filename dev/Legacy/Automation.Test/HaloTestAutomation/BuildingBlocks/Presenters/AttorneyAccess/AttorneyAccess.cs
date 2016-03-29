using NUnit.Framework;
using ObjectMaps.Pages;

namespace BuildingBlocks.Presenters.AttorneyAccess
{
    public class AttorneyAccessView : AttorneyAccessControls
    {
        /// <summary>
        /// Log into the attorney access website
        /// </summary>
        /// <param name="userName">Username</param>
        /// <param name="password">Password</param>
        public void LoginUser(string userName, string password)
        {
            base.Login.Click();
            base.UserName.Value = userName;
            base.Password.Value = password;
            base.btnLogin.Click();
        }

        /// <summary>
        /// register a user
        /// </summary>
        /// <param name="emailAddress">email address to register with</param>
        public void RegisterUser(string emailAddress)
        {
            base.Register.Click();
            base.UserName.Value = emailAddress;
            base.btnRegister.Click();
        }

        /// <summary>
        /// Performs the forgotten password function
        /// </summary>
        /// <param name="emailAddress">Email address to send forgotten password to.</param>
        public void ForgotPassword(string emailAddress)
        {
            base.Login.Click();
            base.ForgottenPassword.Click();
            base.UserName.Value = emailAddress;
            base.btnSend.Click();
        }

        /// <summary>
        /// Navigates to the search and performs a search by account number
        /// </summary>
        /// <param name="accountNumber">Account Number</param>
        public void SearchByAccountNumber(string accountNumber)
        {
            base.Search.Click();
            base.AccountNumber.Value = (accountNumber.ToString());
            base.btnSearch.Click();
        }

        /// <summary>
        /// Navigates to the search and performs a search by ID number
        /// </summary>
        /// <param name="idNumber">ID Number</param>
        public void SearchByIDNumber(string idNumber)
        {
            base.Search.Click();
            base.IDNumber.Value = (idNumber);
            base.btnSearch.Click();
        }

        /// <summary>
        /// Navigates to the search and performs a search by the client name
        /// </summary>
        /// <param name="clientName">Client Name</param>
        public void SearchByClientName(string clientName)
        {
            base.Search.Click();
            base.LegalEntityName.Value = clientName;
            base.btnSearch.Click();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="accountKey"></param>
        public void LoadCaseLink(int accountKey)
        {
            base.LoadCase(accountKey).Click();
        }

        /// <summary>
        ///
        /// </summary>
        public void LogOffLink()
        {
            if (base.LogOff.Exists)
                base.LogOff.Click();
        }

        /// <summary>
        /// Navigate to the Business Event History Report and run it.
        /// </summary>
        /// <param name="_b">TestBrowser</param>
        /// <param name="fromDate">FromDate</param>
        /// <param name="toDate">ToDate</param>
        public void BusinessEventHistoryReport(string fromDate, string toDate)
        {
            base.BusinessEventHistory.Click();
            RunReport(fromDate, toDate);
        }

        /// <summary>
        /// Navigate to the Loan Statement Report and run it.
        /// </summary>
        /// <param name="_b">TestBrowser</param>
        /// <param name="fromDate">FromDate</param>
        /// <param name="toDate">ToDate</param>
        public void LoanStatementReport(string fromDate, string toDate)
        {
            base.LoanStatement.Click();
            RunReport(fromDate, toDate);
        }

        /// <summary>
        /// Navigate to the Proposal History screen
        /// </summary>
        public void ProposalHistoryScreen()
        {
            base.ProposalHistory.Click();
        }

        /// <summary>
        /// run the report
        /// </summary>
        /// <param name="_b"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        public void RunReport(string fromDate, string toDate)
        {
            base.FromDate.TypeText(fromDate);
            base.ToDate.TypeText(toDate);
            base.Document.DomContainer.DialogWatcher.CloseUnhandledDialogs = false;
            base.btnViewReport.ClickNoWait();
        }

        /// <summary>
        /// View the proposal history report
        /// </summary>
        /// <param name="_b">TestBrowser</param>
        /// <param name="proposalKey">Proposal Key for the report.</param>
        public void ProposalHistoryReport(int proposalKey)
        {
            base.ProposalHistory.Click();
            base.Document.DomContainer.DialogWatcher.CloseUnhandledDialogs = false;
            base.LoadProposal(proposalKey).ClickNoWait();
        }

        /// <summary>
        /// This will check that the Load link for an account exists on the search results screen.
        /// </summary>
        /// <param name="accountKey">accountKey</param>
        public void AssertAttorneyAccessSearchResults(int accountKey)
        {
            bool linkExists = base.LoadCaseLinkExists(accountKey);
            Assert.IsTrue(linkExists, string.Format("The Load link for Account {0} was not found in the search results.", accountKey));
        }

        /// This will check that the load link for a specific proposal exists on the proposal details screen.
        /// </summary>
        /// <param name="_b">TestBrowser</param>
        /// <param name="proposalKey">proposalKey</param>
        public void AssertAttorneyAccessProposalDetails(int proposalKey)
        {
            bool linkExists = base.LoadProposalLinkExists(proposalKey);
            Assert.IsTrue(linkExists, string.Format("The Load link for Proposal {0} was not found in the proposal details screen.", proposalKey));
        }

        /// <summary>
        /// Clicks the print note button
        /// </summary>
        public void PrintNotes()
        {
            base.btnPrintNote.Click();
        }

        /// <summary>
        ///
        /// </summary>
        public void NotesLink()
        {
            base.Notes.Click();
        }

        /// <summary>
        ///
        /// </summary>
        public void AddNotesLink()
        {
            base.AddNote.Click();
        }
    }
}