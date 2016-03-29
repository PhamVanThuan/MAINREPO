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
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using System.Collections.Generic;

namespace SAHL.Web.Views.Common
{
    public partial class ApplicationWarnings : SAHLCommonBaseView,IApplicationWarnings
    {
        #region Private Members
        List<string> _legalEntityErrors = new List<string>();
        List<string> _applicationOfferErrors = new List<string>();
        List<string> _creditErrors = new List<string>();
        List<string> _detailTypes = new List<string>();
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ShouldRunPage)
                return;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!ShouldRunPage)
                return;
            SAHLMaster master = Page.Master as SAHLMaster;
            master.ValidationSummary.Visible = false;

            tblLegalEntity.Rows.Clear();
            for (int x = 0; x < _legalEntityErrors.Count; x++)
            {
                HtmlTableRow row = new HtmlTableRow();
                row.Height = "24";
                HtmlTableCell cell = new HtmlTableCell();
                cell.InnerText = "* " + _legalEntityErrors[x];
                row.Cells.Add(cell);
                tblLegalEntity.Rows.Add(row);
            }

            tblApplicationOffer.Rows.Clear();
            for (int x = 0; x < _applicationOfferErrors.Count; x++)
            {
                HtmlTableRow row = new HtmlTableRow();
                row.Height = "24";
                HtmlTableCell cell = new HtmlTableCell();
                cell.InnerText = "* " + _applicationOfferErrors[x];
                row.Cells.Add(cell);
                tblApplicationOffer.Rows.Add(row);
            }

            tblCredit.Rows.Clear();
            for (int x = 0; x < _creditErrors.Count; x++)
            {
                HtmlTableRow row = new HtmlTableRow();
                row.Height = "24";
                HtmlTableCell cell = new HtmlTableCell();
                cell.InnerText = "* " + _creditErrors[x];
                row.Cells.Add(cell);
                tblCredit.Rows.Add(row);
            }

            tblDetailTypes.Rows.Clear();
            for (int x = 0; x < _detailTypes.Count; x++)
            {
                HtmlTableRow row = new HtmlTableRow();
                row.Height = "24";
                HtmlTableCell cell = new HtmlTableCell();
                cell.InnerText = "* " + _detailTypes[x];
                row.Cells.Add(cell);
                tblDetailTypes.Rows.Add(row);
            }
        }

        #region IApplicationWarnings Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="leErrors"></param>
        public void PopulateLegalEntityErrors(List<string> leErrors)
        {
           _legalEntityErrors.AddRange(leErrors);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aoErrors"></param>
        public void PopulateApplicationOfferRules(List<string> aoErrors)
        {
           _applicationOfferErrors.AddRange(aoErrors);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmErrors"></param>
        public void PopulateCreditMatrixRules(List<string> cmErrors)
        {
            _creditErrors.AddRange(cmErrors); 
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="detailTypes"></param>
        public void PopulateDetailTypes(List<string> detailTypes)
        {
            _detailTypes.AddRange(detailTypes);
        }

        public bool ShowDetailTypes
        {
            get { return pnlDetailTypes.Visible; }
            set { pnlDetailTypes.Visible = value; }
        }

        #endregion
    }
}
