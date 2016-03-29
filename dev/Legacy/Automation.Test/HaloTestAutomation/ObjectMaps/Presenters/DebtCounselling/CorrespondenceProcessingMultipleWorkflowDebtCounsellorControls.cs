using System.Text.RegularExpressions;
using WatiN.Core;

namespace ObjectMaps.Pages
{
    public class CorrespondenceProcessingMultipleWorkflowDebtCounsellorControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_gridRecipientsMultiple")]
        protected Table ctl00MaingridRecipientsMultiple;

        protected TableRow ctl00MaingridRecipientsMultipleRow(int legalEntityKey)
        {
            return ctl00MaingridRecipientsMultiple.TableCell(Find.ByText(legalEntityKey.ToString())).ContainingTableRow;
        }

        protected Link linkToggleExtraInfo(int legalEntityKey)
        {
            return ctl00MaingridRecipientsMultipleRow(legalEntityKey).Links[0];
        }

        protected CheckBox chkFax(int legalEntityKey)
        {
            return base.Document.CheckBox(Find.ById(new Regex(string.Format(@"^ctl00_Main_gridRecipientsMultiple_ctl[0-9]*_pnlOptions_{0}_chkFax$", legalEntityKey))));
        }

        protected TextField txtFaxCODE(int legalEntityKey)
        {
            return base.Document.TextField(Find.ById(new Regex(string.Format(@"^ctl00_Main_gridRecipientsMultiple_ctl[0-9]*pnlOptions_{0}_txtFax__CODE$", legalEntityKey))));
        }

        protected TextField txtFaxNUMB(int legalEntityKey)
        {
            return base.Document.TextField(Find.ById(new Regex(string.Format(@"^ctl00_Main_gridRecipientsMultiple_ctl[0-9]*_pnlOptions_{0}_txtFax__NUMB$", legalEntityKey))));
        }

        protected CheckBox chkEmail(int legalEntityKey)
        {
            return base.Document.CheckBox(Find.ById(new Regex(string.Format(@"^ctl00_Main_gridRecipientsMultiple_ctl[0-9]*_pnlOptions_{0}_chkEmail$", legalEntityKey))));
        }

        protected TextField txtEmail(int legalEntityKey)
        {
            return base.Document.TextField(Find.ById(new Regex(string.Format(@"^ctl00_Main_gridRecipientsMultiple_ctl[0-9]*_pnlOptions_{0}_txtEmail$", legalEntityKey))));
        }

        protected CheckBox chkPost(int legalEntityKey)
        {
            return base.Document.CheckBox(Find.ById(new Regex(string.Format(@"^ctl00_Main_gridRecipientsMultiple_ctl[0-9]*_pnlOptions_{0}_chkPost$", legalEntityKey))));
        }

        [FindBy(Id = "ctl00_Main_btnPreview")]
        protected Button ctl00MainbtnPreview;

        [FindBy(Id = "ctl00_Main_btnSend")]
        protected Button ctl00MainbtnSend;

        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button ctl00MainbtnCancel;

        [FindBy(Id = "ctl00_valSummary_Body")]
        protected Div divValidationSummaryBody;

        [FindBy(Name = "ctl00$valSummary$ctl05")]
        protected Button divContinueButton;
    }
}