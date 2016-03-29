using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Controls;

namespace SAHL.Web.Controls
{
    /// <summary>
    /// Displays a panel with SuperLo Information. The information is presented in the from of line items from year 1 to 5.
    /// </summary>
    public class SuperLoInfoPanel: Panel, INamingContainer
    {
        //private IList<SuperLoInfoItem> _superLoInfoItems;
        private string _titleText = "Super Lo Info";
        private HtmlTable _htmlTable;
        private IApplicationProductSuperLoLoan _applicationProductSuperLoLoan;

        private SAHLLabel _lbAnnual1 = new SAHLLabel();
        private SAHLLabel _lbAnnual2 = new SAHLLabel();
        private SAHLLabel _lbAnnual3 = new SAHLLabel();
        private SAHLLabel _lbAnnual4 = new SAHLLabel();
        private SAHLLabel _lbAnnual5 = new SAHLLabel();
        private SAHLLabel _lbCumulative1 = new SAHLLabel();
        private SAHLLabel _lbCumulative2 = new SAHLLabel();
        private SAHLLabel _lbCumulative3 = new SAHLLabel();
        private SAHLLabel _lbCumulative4 = new SAHLLabel();
        private SAHLLabel _lbCumulative5 = new SAHLLabel();
        private SAHLLabel _lbLoyalty1 = new SAHLLabel();
        private SAHLLabel _lbLoyalty2 = new SAHLLabel();
        private SAHLLabel _lbLoyalty3 = new SAHLLabel();
        private SAHLLabel _lbLoyalty4 = new SAHLLabel();
        private SAHLLabel _lbLoyalty5 = new SAHLLabel();


        //private bool _shouldRunPage = true;
        

        /// <summary>
        /// Constructor: Sets basic properties such as the default title, width.
        /// </summary>
        public SuperLoInfoPanel()
        {
            //_superLoInfoItems = new List<SuperLoInfoItem>();

            base.GroupingText = _titleText;
            _htmlTable = new HtmlTable();
            base.Width = new Unit(500, UnitType.Pixel);
            _htmlTable.Width = "99%";

            if (DesignMode)
                return;
        }


        #region Protected Methods
        /// <summary>
        /// Draws the table with the data supplied.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //if (!_shouldRunPage)
            //    return;

            base.GroupingText = _titleText;

            HtmlTableRow htmlTableRowTitle = new HtmlTableRow();
            AddCell(htmlTableRowTitle, " ", false);
            AddCell(htmlTableRowTitle, "Annual", true);
            AddCell(htmlTableRowTitle, "Cumulative", true);
            AddCell(htmlTableRowTitle, "Loyalty Benefit", true);
            _htmlTable.Rows.Add(htmlTableRowTitle);

            HtmlTableRow rowYear1 = new HtmlTableRow();
            AddCell(rowYear1, "Year 1", false);
            AddCell(rowYear1, _lbAnnual1);
            AddCell(rowYear1, _lbCumulative1);
            AddCell(rowYear1, _lbLoyalty1);
            _htmlTable.Rows.Add(rowYear1);

            HtmlTableRow rowYear2 = new HtmlTableRow();
            AddCell(rowYear2, "Year 2", false);
            AddCell(rowYear2, _lbAnnual2);
            AddCell(rowYear2, _lbCumulative2);
            AddCell(rowYear2, _lbLoyalty2);
            _htmlTable.Rows.Add(rowYear2);

            HtmlTableRow rowYear3 = new HtmlTableRow();
            AddCell(rowYear3, "Year 3", false);
            AddCell(rowYear3, _lbAnnual3);
            AddCell(rowYear3, _lbCumulative3);
            AddCell(rowYear3, _lbLoyalty3);
            _htmlTable.Rows.Add(rowYear3);

            HtmlTableRow rowYear4 = new HtmlTableRow();
            AddCell(rowYear4, "Year 4", false);
            AddCell(rowYear4, _lbAnnual4);
            AddCell(rowYear4, _lbCumulative4);
            AddCell(rowYear4, _lbLoyalty4);
            _htmlTable.Rows.Add(rowYear4);

            HtmlTableRow rowYear5 = new HtmlTableRow();
            AddCell(rowYear5, "Year 5", false);
            AddCell(rowYear5, _lbAnnual5);
            AddCell(rowYear5, _lbCumulative5);
            AddCell(rowYear5, _lbLoyalty5);
            _htmlTable.Rows.Add(rowYear5);

            base.Controls.Add(_htmlTable);
        }

        private void PopulateControls()
        {
            // Get the threshold stuff from the  IApplicationInformationSuperLoLoan
            ISupportsSuperLoApplicationInformation supportsSuperLoApplicationInformation = _applicationProductSuperLoLoan as ISupportsSuperLoApplicationInformation;
            if (supportsSuperLoApplicationInformation != null)
            {
                double cumulativeThreshold = 0.0;

                cumulativeThreshold += supportsSuperLoApplicationInformation.SuperLoInformation.PPThresholdYr1;
                
                _lbAnnual1.Text = supportsSuperLoApplicationInformation.SuperLoInformation.PPThresholdYr1.ToString(SAHL.Common.Constants.CurrencyFormat);
                _lbCumulative1.Text = cumulativeThreshold.ToString(SAHL.Common.Constants.CurrencyFormat);
                _lbLoyalty1.Text = "-";

                cumulativeThreshold += supportsSuperLoApplicationInformation.SuperLoInformation.PPThresholdYr2;
                
                _lbAnnual2.Text = supportsSuperLoApplicationInformation.SuperLoInformation.PPThresholdYr2.ToString(SAHL.Common.Constants.CurrencyFormat);
                _lbCumulative2.Text = cumulativeThreshold.ToString(SAHL.Common.Constants.CurrencyFormat);
                _lbLoyalty2.Text = _applicationProductSuperLoLoan.LoyaltyBonusY2.ToString(SAHL.Common.Constants.CurrencyFormat);

                cumulativeThreshold += supportsSuperLoApplicationInformation.SuperLoInformation.PPThresholdYr3;
                
                _lbAnnual3.Text = supportsSuperLoApplicationInformation.SuperLoInformation.PPThresholdYr3.ToString(SAHL.Common.Constants.CurrencyFormat);
                _lbCumulative3.Text = cumulativeThreshold.ToString(SAHL.Common.Constants.CurrencyFormat);
                _lbLoyalty3.Text = _applicationProductSuperLoLoan.LoyaltyBonusY3.ToString(SAHL.Common.Constants.CurrencyFormat);

                cumulativeThreshold += supportsSuperLoApplicationInformation.SuperLoInformation.PPThresholdYr4;
                _lbAnnual4.Text = supportsSuperLoApplicationInformation.SuperLoInformation.PPThresholdYr4.ToString(SAHL.Common.Constants.CurrencyFormat);
                _lbCumulative4.Text = cumulativeThreshold.ToString(SAHL.Common.Constants.CurrencyFormat);
                _lbLoyalty4.Text = _applicationProductSuperLoLoan.LoyaltyBonusY4.ToString(SAHL.Common.Constants.CurrencyFormat);

                cumulativeThreshold += supportsSuperLoApplicationInformation.SuperLoInformation.PPThresholdYr5;
                _lbAnnual5.Text = supportsSuperLoApplicationInformation.SuperLoInformation.PPThresholdYr5.ToString(SAHL.Common.Constants.CurrencyFormat);
                _lbCumulative5.Text = cumulativeThreshold.ToString(SAHL.Common.Constants.CurrencyFormat);
                _lbLoyalty5.Text = _applicationProductSuperLoLoan.LoyaltyBonusY5.ToString(SAHL.Common.Constants.CurrencyFormat);
            }
        }

        /// <summary>
        /// Adds a new cell to a given row.
        /// </summary>
        /// <param name="htmlTableRow"></param>
        /// <param name="cellCaption"></param>
        /// <param name="isBold"></param>
        protected static void AddCell(HtmlTableRow htmlTableRow, string cellCaption, bool isBold)
        {
            HtmlTableCell htmlTableCell = new HtmlTableCell();
            htmlTableCell.InnerText = cellCaption;
            if (isBold)
                htmlTableCell.Style.Add("class", "TitleText");
            htmlTableRow.Cells.Add(htmlTableCell);
        }

        /// <summary>
        /// Adds a new cell to a given row.
        /// </summary>
        /// <param name="htmlTableRow"></param>
        /// <param name="control"></param>
        protected static void AddCell(HtmlTableRow htmlTableRow, WebControl control)
        {
            HtmlTableCell htmlTableCell = new HtmlTableCell();
            htmlTableCell.Controls.Add(control);
            htmlTableRow.Cells.Add(htmlTableCell);
        }

        /// <summary>
        /// Determines whether we are in design mode (standard DesignMode not reliable).
        /// </summary>
        protected static new bool DesignMode
        {
            get
            {
                return (HttpContext.Current == null);
            }
        }

        #endregion


        #region Public Methods

        public IApplicationProductSuperLoLoan ApplicationProductSuperLoLoan
        {
            set
            {
                _applicationProductSuperLoLoan = value;
                PopulateControls();
            }
        }

        #region ShouldRunPage from Rodders
        ///// <summary>
        ///// 
        ///// </summary>
        //public bool ShouldRunPage
        //{
        //    set
        //    {
        //        _shouldRunPage = value;
        //    }
        //}
        #endregion
        
        #endregion


    }
}
