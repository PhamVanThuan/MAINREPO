using System;
using System.Data;
using System.Web.UI.WebControls;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Controls;
using SAHL.Web.Views.Common.Interfaces;

namespace SAHL.Web.Views.Common
{
    public partial class ValuationLightstoneView : SAHLCommonBaseView, IValuationLightstoneView
    {
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler ddlMapProviderSelectedIndexChanged;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler btnBackClicked;

        protected void btnBack_Click(object sender, EventArgs e)
        {
            // Return to Default Screen
            if (btnBackClicked != null)
                btnBackClicked(sender, e);
        }


        /// <summary>
        /// 
        /// </summary>
        public event EventHandler TabsActiveTabChanged;

        //***********************************************************************************

        protected void ddlMapProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlMapProviderSelectedIndexChanged != null)
                ddlMapProviderSelectedIndexChanged(sender, e);

        }

        protected void Tabs_ActiveTabChanged(object sender, EventArgs e)
        {
            if (TabsActiveTabChanged != null)
                TabsActiveTabChanged(sender, e);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="DT"></param>
        public void BindgrdOwners(DataTable DT)
        {
            grdOwners.AutoGenerateColumns = false;
            grdOwners.AddGridBoundColumn("BUYER_NAME", "Name", Unit.Percentage(50), HorizontalAlign.Left, true);
            grdOwners.AddGridBoundColumn("BUYER_IDCK", "Identity", Unit.Percentage(25), HorizontalAlign.Left, true);
            grdOwners.AddGridBoundColumn("PERSON_TYPE_ID", "Legal Entity Type", Unit.Percentage(25), HorizontalAlign.Left, true);
            grdOwners.DataSource = DT;
            grdOwners.DataBind();

            

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DT"></param>
        public void BindgrdComparativeSales(DataTable DT)
        {
            grdComparativeSales.AutoGenerateColumns = false;
            grdComparativeSales.AddGridBoundColumn("Prop_ID", "Key", Unit.Percentage(0), HorizontalAlign.Left, false);
            grdComparativeSales.AddGridBoundColumn("FullAddress", "Street Address", Unit.Percentage(30), HorizontalAlign.Left, true);
            grdComparativeSales.AddGridBoundColumn("distance", SAHL.Common.Constants.NumberFormat, GridFormatType.GridNumber, "Distance", false, Unit.Percentage(6), HorizontalAlign.Left, true);
            grdComparativeSales.AddGridBoundColumn("PREV_DATE", "Sale Date", Unit.Percentage(10), HorizontalAlign.Left, true);
            grdComparativeSales.AddGridBoundColumn("LASTPRICE", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Sale Price", false, Unit.Percentage(20), HorizontalAlign.Left, true);
            grdComparativeSales.AddGridBoundColumn("ERF", "ERF", Unit.Percentage(10), HorizontalAlign.Left, true);
            grdComparativeSales.AddGridBoundColumn("PORTION", "Portion", Unit.Percentage(10), HorizontalAlign.Left, true);
            grdComparativeSales.AddGridBoundColumn("ERF_SIZE", SAHL.Common.Constants.NumberFormat, GridFormatType.GridNumber, "ERF Size", false, Unit.Percentage(10), HorizontalAlign.Left, true);
            grdComparativeSales.DataSource = DT;
            grdComparativeSales.DataBind();

        }


        //***********************************************************************************
        // SHOW / HIDE SCREEN OBJECTS


        /// <summary>
        /// Gets or Sets the Active Tab Index for the Main Tab Control
        /// </summary>
        public int SetTabsActiveTabIndex
        {
            set { Tabs.ActiveTabIndex = value; }
            get { return Tabs.ActiveTabIndex; }
        }


        /// <summary>
        /// Method to show/hide  the 'pnlOwners' Panel
        /// </summary>
        public bool SetpnlOwners
        {
            set { pnlOwners.Visible = value; }
        }

        /// <summary>
        /// Method to set show/hide  the 'MapLiteral' Literals' text to add and populate the Iframe
        /// </summary>
        public string SetMapLiteral
        {
            set { MapLiteral.Text = value; }
        }



        /// <summary>
        /// Get the Map URL from the ddlMapProviderDropDownlist
        /// </summary>
        public string GetddlMapProviderURL
        {
            get { return ddlMapProvider.SelectedValue; }
        }

        /// <summary>
        /// Get the Map URL from the ddlMapProviderDropDownlist Index 1=google 2=MapX
        /// </summary>
        public int GetdllMapProviderIndex
        {
            get { return ddlMapProvider.SelectedIndex; }
            set { ddlMapProvider.SelectedIndex = value; }
        }
        //***********************************************************************************
        // SET UP THE PROPERTY CHANGE METHODS


        /// <summary>
        /// Method to set the 'lblReferenceNumberValue' text property
        /// </summary>
        public string SetlblReferenceNumberValue
        {
            set { lblReferenceNumberValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblLastSaleDateValue' text property
        /// </summary>
        public string SetlblLastSaleDateValue
        {
            set { lblLastSaleDateValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblAutomatedValueDateValue' text property
        /// </summary>
        public string SetlblAutomatedValueDateValue
        {
            set { lblAutomatedValueDateValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblLastSalePriceValue' text property
        /// </summary>
        public string SetlblLastSalePriceValue
        {
            set { lblLastSalePriceValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblERFSize' text property
        /// </summary>
        public string SetlblERFSizeValue
        {
            set { lblERFSize.Text = value; }
        }

        
        /// <summary>
        /// Method to set the 'lblEstimatedMarketValue' text property
        /// </summary>
        public string SetlblEstimatedMarketValue
        {
            set { lblEstimatedMarketValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblAreaQualityGradeValue' text property
        /// </summary>
        public string SetlblAreaQualityGradeValue
        {
            set { lblAreaQualityGradeValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblConfidenceScoreValue' text property
        /// </summary>
        public string SetlblConfidenceScoreValue
        {
            set { lblConfidenceScoreValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblAreaExposureValue' text property
        /// </summary>
        public string SetlblAreaExposureValue
        {
            set { lblAreaExposureValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblExpectedHighValue' text property
        /// </summary>
        public string SetlblExpectedHighValue
        {
            set { lblExpectedHighValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblAutomatedValuationDecisionValue' text property
        /// </summary>
        public string SetlblAutomatedValuationDecisionValue
        {
            set { lblAutomatedValuationDecisionValue.Text = value; }
        }

        /// <summary>
        /// Method to set the 'lblExpectedLowValue' text property
        /// </summary>
        public string SetlblExpectedLowValue
        {
            set { lblExpectedLowValue.Text = value; }
        }



        //***********************************************************************************

    }
}
