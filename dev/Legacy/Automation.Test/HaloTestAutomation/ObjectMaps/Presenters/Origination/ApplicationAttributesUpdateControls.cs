using System.Linq;
using WatiN.Core;

namespace ObjectMaps.Pages
{
    public abstract class ApplicationAttributesUpdateControls : BasePageControls
    {
        [FindBy(Id = "ctl00_Main_btnCancel")]
        protected Button btnCancel { get; set; }

        [FindBy(Id = "ctl00_Main_btnUpdate")]
        protected Button btnUpdate { get; set; }

        [FindBy(Id = "ctl00_Main_TxtTransferAttorney")]
        protected TextField txtTransferAttorney { get; set; }

        [FindBy(Id = "ctl00_Main_ddlMarketingSource")]
        protected SelectList ddlMarketingSource { get; set; }

        [FindBy(Id = "ctl00_Main_chkAttributes")]
        protected Table ApplicationLoanAttributes { get; set; }

        /// <summary>
        /// Returns the row containing the text of the attribute provided
        /// </summary>
        /// <param name="labelText"></param>
        /// <returns></returns>
        protected TableRow ApplicationLoanAttributesRow(string attributeDescription)
        {
            var row = (from a in ApplicationLoanAttributes.TableRows
                       where a.Text.Contains(attributeDescription)
                       select a).FirstOrDefault();
            return row;
        }
    }
}