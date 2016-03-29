using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class DebtCounsellingCreateCaseControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_dte17pt1Date")]
        protected TextField _17Pt1Date { get; set; }

        [FindBy(Id = "ctl00_Main_txtPassportNumber")]
        protected TextField IDPassportNumber { get; set; }

        [FindBy(Id = "ctl00_Main_txtReferenceNo")]
        protected TextField TestReference { get; set; }

        protected TableRowCollection PeopleofImportance
        {
            get
            {
                Div SAHLTreeview = base.Document.Div(Find.ById("ctl00_Main_trvPeople"));
                return SAHLTreeview.TableRows;
            }
        }

        protected TableRowCollection AccountsofImportance
        {
            get
            {
                Div SAHLTreeview = base.Document.Div(Find.ById("ctl00_Main_trvAccounts"));
                return SAHLTreeview.TableRows;
            }
        }

        protected TableRowCollection LegalEntitySearchResults
        {
            get
            {
                TableBody tableBody = base.Document.TableBody(Find.ById("bodyQueryResults"));
                return tableBody.OwnTableRows;
            }
        }

        [FindBy(Id = "ctl00_Main_btnSubmit")]
        protected Button CreateCase { get; set; }

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button Cancel { get; set; }

        [FindBy(Id = "ctl00_Main_trvAccounts")]
        protected Div Accounts { get; set; }
    }
}