using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.ApplicationBlocks.UIProcess;
using SAHL.Common;
using System.Text;
using SAHL.Common.Authentication;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Life.Interfaces;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;

namespace SAHL.Web.Views.Life
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Benefit : SAHLCommonBaseView, IBenefit
    {
        private bool _activityDone;
        private bool _proceedWithPolicy;
        private bool _refusedPolicy;
        private LifePolicyTypes _lifePolicyType;

        private CheckBox chkConfirmProceed;
        private CheckBox chkConfirmRefused;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNext_Click(object sender, EventArgs e)
        {
            ProceedWithPolicy = chkConfirmProceed.Checked;
            RefusedPolicy = chkConfirmRefused.Checked;

            OnNextButtonClicked(sender, e);
        }

        #region IBenefit Members

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnNextButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public bool ActivityDone
        {
            get { return _activityDone; }
            set { _activityDone = value; }
        }

        public bool ProceedWithPolicy
        {
            get { return _proceedWithPolicy; }
            set { _proceedWithPolicy = value; }
        }

        public bool RefusedPolicy
        {
            get { return _refusedPolicy; }
            set { _refusedPolicy = value; }
        }

        public LifePolicyTypes LifePolicyType
        {
            get { return _lifePolicyType; }
            set { _lifePolicyType = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textItems"></param>
        public void BindBenefit(IReadOnlyEventList<ITextStatement> textItems)
        {
            if (textItems.Count > 0)
            {
                lblIntro.Text = textItems[0].Statement;

                TableRow spacer = new TableRow();
                spacer.Height = new Unit(5);
                tblText1.Rows.Add(spacer);
                tblText2.Rows.Add(spacer);

                chkConfirmProceed = new CheckBox();
                chkConfirmRefused = new CheckBox();

                for (int i = 1; i < textItems.Count; i++)
                {
                    bool useCheckbox = false;
                    if (textItems[i].StatementTitle == "Benefit 4 - Accept Premium"
                        || textItems[i].StatementTitle == "Benefit 7 - Respect Decision"
                        || textItems[i].StatementTitle == "Benefit 5 - Accept Premium"
                        || textItems[i].StatementTitle == "Benefit 9 - Respect Decision")
                        useCheckbox = true;

                    TableRow row = new TableRow();
                    row.Cells.Add(new TableCell());
                    row.Cells.Add(new TableCell());

                    CheckBox cbx = new CheckBox();

                    cbx.ID = "cbx" + i.ToString();
                    cbx.Visible = useCheckbox;

                    Label lbl = new Label();
                    lbl.ID = "lbl" + i.ToString();
                    lbl.Text = textItems[i].Statement;
                    lbl.Font.Size = new FontUnit(FontSize.Small);

                    switch (textItems[i].StatementTitle)
                    {
                        case "Benefit 4 - Accept Premium":
                        case "Benefit 5 - Accept Premium":
                            row.Cells[0].Controls.Add(chkConfirmProceed);
                            break;

                        case "Benefit 7 - Respect Decision":
                        case "Benefit 9 - Respect Decision":
                            row.Cells[0].Controls.Add(chkConfirmRefused);
                            break;

                        default:
                            row.Cells[0].Controls.Add(cbx);
                            break;
                    }     
                    
                    row.Cells[0].VerticalAlign = VerticalAlign.Top;
                    row.Cells[1].Controls.Add(lbl);
                    row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                    row.Cells[1].VerticalAlign = VerticalAlign.Top;
                    row.Height = new Unit(40);

                    //Decide which table the response belongs in
                    if (textItems[i].TextStatementType.Description == TextStatementTypes.Benefits1.ToString()
                     || textItems[i].TextStatementType.Description == TextStatementTypes.AccidentalBenefits1.ToString())
                        tblText1.Rows.Add(row);
                    else //Benefits2
                        tblText2.Rows.Add(row);

                    if (_activityDone)
                    {
                        chkConfirmProceed.Checked = _proceedWithPolicy;
                        chkConfirmProceed.Enabled = false;

                        chkConfirmRefused.Checked = _refusedPolicy;
                        chkConfirmRefused.Enabled = false;
                    }

                }
            }
        }

        #endregion
    }
}