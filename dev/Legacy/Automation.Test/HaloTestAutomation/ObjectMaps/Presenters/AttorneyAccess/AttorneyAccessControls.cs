using System.Text.RegularExpressions;
using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class AttorneyAccessControls : BasePageControls
    {
        /// <summary>
        /// User Name field
        /// </summary>
        [FindBy(Id = "UserName")]
        protected TextField UserName { get; set; }

        /// <summary>
        /// Password field
        /// </summary>
        [FindBy(Id = "Password")]
        protected TextField Password { get; set; }

        /// <summary>
        /// Login Button
        /// </summary>
        [FindBy(Value = "Log in")]
        protected Button btnLogin { get; set; }

        /// <summary>
        /// Register Link
        /// </summary>
        [FindBy(Text = "Register")]
        protected Link Register { get; set; }

        /// <summary>
        /// Login Link
        /// </summary>
        [FindBy(Text = "Login")]
        protected Link Login { get; set; }

        /// <summary>
        /// Register Button
        /// </summary>
        [FindBy(Value = "Register")]
        protected Button btnRegister { get; set; }

        /// <summary>
        /// Link to Forgotten Password page
        /// </summary>
        [FindBy(Text = "Forgotten Password")]
        protected Link ForgottenPassword { get; set; }

        /// <summary>
        /// Button to send Forgotten password
        /// </summary>
        [FindBy(Value = "Send")]
        protected Button btnSend { get; set; }

        /// <summary>
        /// Account Number search field
        /// </summary>
        [FindBy(Id = "CaseNumber")]
        protected TextField AccountNumber { get; set; }

        /// <summary>
        /// Id Number search field
        /// </summary>
        [FindBy(Id = "IDNumber")]
        protected TextField IDNumber { get; set; }

        /// <summary>
        /// Legal Entity Name search field
        /// </summary>
        [FindBy(Id = "LegalEntityName")]
        protected TextField LegalEntityName { get; set; }

        /// <summary>
        /// Search Button
        /// </summary>
        [FindBy(Value = "Search")]
        protected Button btnSearch { get; set; }

        /// <summary>
        /// Search menu item
        /// </summary>
        [FindBy(Text = "Search")]
        protected Link Search { get; set; }

        /// <summary>
        /// The Logoff Link
        /// </summary>
        [FindBy(Text = "Logoff")]
        protected Link LogOff { get; set; }

        /// <summary>
        /// Checks if the load link exists for the case provided.
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="debtCounsellingKey"></param>
        /// <returns></returns>
        protected bool LoadCaseLinkExists(int accountKey)
        {
            return this.LoadCase(accountKey).Exists;
        }

        /// <summary>
        /// Returns the load case link for a case
        /// </summary>
        /// <param name="accountKey">AccountKey</param>
        /// <param name="debtCounsellingKey">DebtCounsellingKey</param>
        /// <returns></returns>
        protected Link LoadCase(int accountKey)
        {
            Regex regex = new Regex(string.Format(@"^[\x20-\x7E]*accountKey={0}$", accountKey));
            return base.Document.Link(Find.By("href", regex));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="proposalKey"></param>
        /// <returns></returns>
        protected Link LoadProposal(int proposalKey)
        {
            Regex regex = new Regex(string.Format(@"^[\x20-\x7E]*reportKey={0}$", proposalKey));
            return base.Document.Link(Find.By("href", regex));
        }

        /// <summary>
        /// Checks if the load proposal link exists for the case provided.
        /// </summary>
        /// <param name="accountKey"></param>
        /// <param name="debtCounsellingKey"></param>
        /// <returns></returns>
        protected bool LoadProposalLinkExists(int proposalKey)
        {
            return this.LoadProposal(proposalKey).Exists;
        }

        /// <summary>
        /// Note Menu Item
        /// </summary>
        [FindBy(Text = "Notes")]
        protected Link Notes { get; set; }

        /// <summary>
        /// Add Note Menu Item
        /// </summary>
        [FindBy(Text = "Add Note")]
        protected Link AddNote { get; set; }

        /// <summary>
        /// Loan Statement Menu Item
        /// </summary>
        [FindBy(Text = "Loan Statement")]
        protected Link LoanStatement { get; set; }

        /// <summary>
        /// Proposal History Menu Item
        /// </summary>
        [FindBy(Text = "Proposal History")]
        protected Link ProposalHistory { get; set; }

        /// <summary>
        /// Business Event History Menu Item
        /// </summary>
        [FindBy(Text = "Business Event History")]
        protected Link BusinessEventHistory { get; set; }

        /// <summary>
        /// Report From Date SqlParameter
        /// </summary>
        [FindBy(Id = "FromDate")]
        protected TextField FromDate { get; set; }

        /// <summary>
        /// Report To Date SqlParameter
        /// </summary>
        [FindBy(Id = "ToDate")]
        protected TextField ToDate { get; set; }

        /// <summary>
        /// View Report button
        /// </summary>
        [FindBy(Value = "View Report")]
        protected Button btnViewReport { get; set; }

        /// <summary>
        /// Button to print the notes.
        /// </summary>
        [FindBy(Value = "Print")]
        protected Button btnPrintNote { get; set; }
    }
}