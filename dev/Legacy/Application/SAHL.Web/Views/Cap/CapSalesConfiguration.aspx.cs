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
using SAHL.Common.Web.UI.Controls;
using System.Collections.Generic;
using SAHL.Common.Web.UI;
using SAHL.Web.Views.Common.Interfaces;
using System.Diagnostics.CodeAnalysis;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Web.UI.Events;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Globals;
using SAHL.Web.Views.Cap.Interfaces;
using System.Text;

namespace SAHL.Web.Views.Cap
{

    /// <summary>
    /// 
    /// </summary>
    public partial class CapSalesConfiguration :  SAHLCommonBaseView, ICapSalesConfiguration
    {


        #region Private Variables 

        public const string NumberFormat5Decimal = "0.00000";

        #endregion

        #region Event Handlers

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnGridSelectedIndexChanged;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnSubmitButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnCancelButtonClicked;


        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public GridPostBackType CapSalesConfigGridPostBackType
        {
            set
            {
                CapSalesConfigGrid.PostBackType = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool SubmitButtonEnabled
        {
            set
            {
                SubmitButton.Enabled = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int CapSalesConfigGridSelectedIndex
        {
            get
            {
                return CapSalesConfigGrid.SelectedIndexInternal;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SelectedCapResetConfigDate
        {
            get
            {
                if (Request.Form[CapResetConfigDate.UniqueID] != null && Request.Form[CapResetConfigDate.UniqueID] != "-select-")
                    return Request.Form[CapResetConfigDate.UniqueID];
                else
                    return "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int SelectedCapType
        {
            get
            {
                if (Request.Form[CapTypeValueUpdate.UniqueID] != null && Request.Form[CapTypeValueUpdate.UniqueID] != "-select-")
                    return Convert.ToInt32( Request.Form[CapTypeValueUpdate.UniqueID] );
                else
                    return -1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int TermValue
        {
            get
            {
                if (!String.IsNullOrEmpty(Request.Form[TermUpdate.UniqueID]))
                {
                    return Convert.ToInt32(Request.Form[TermUpdate.UniqueID]);
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double AdminFeeValue
        {
            get
            {
                if (!String.IsNullOrEmpty(Request.Form[AdminFeeUpdate.UniqueID]))
                {
                    return Convert.ToDouble(Request.Form[AdminFeeUpdate.UniqueID]);
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double PremiumFeeValue
        {
            get
            {
                if (!String.IsNullOrEmpty(Request.Form[PremiumFeeUpdate.UniqueID]))
                {
                    return Convert.ToDouble(Request.Form[PremiumFeeUpdate.UniqueID]);
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double RateFinanceValue
        {
            get
            {
                if (!String.IsNullOrEmpty(Request.Form[FinanceRateUpdate.UniqueID]))
                {
                    return Convert.ToDouble(Request.Form[FinanceRateUpdate.UniqueID]);
                }
                else
                {
                    return 0;
                }
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public DateTime ApplicationStartDateValue
        {
            get
            {
                if (OfferStartDateUpdate.Date.HasValue)
                    return OfferStartDateUpdate.Date.Value;
                else
                    return DateTime.Now;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime ApplicationEndDateValue
        {
            get
            {
                if (OfferEndDateUpdate.Date.HasValue)
                    return OfferEndDateUpdate.Date.Value;
                else
                    return DateTime.Now;
            }
        }
            
        #endregion

        #region Protected Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CapTypeValue_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CapSalesConfigGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ICapTypeConfigurationDetail ro = e.Row.DataItem as ICapTypeConfigurationDetail;
                e.Row.Cells[2].Text = ro.Rate.ToString(SAHL.Common.Constants.RateFormat);

                if (ro.CapType != null)
                {
                    e.Row.Cells[1].Text = ro.CapType.Description;
                    e.Row.Cells[7].Text = ro.GeneralStatus.Description;
                }
                if (ro.CapTypeConfiguration != null)
                {
                    e.Row.Cells[3].Text = ro.CapTypeConfiguration.Term.ToString() + " months";
                    e.Row.Cells[4].Text = ro.CapTypeConfiguration.ApplicationStartDate.ToString(SAHL.Common.Constants.DateFormat);
                    e.Row.Cells[5].Text = ro.CapTypeConfiguration.ApplicationEndDate.ToString(SAHL.Common.Constants.DateFormat);
                    e.Row.Cells[6].Text = ro.CapTypeConfiguration.CapEffectiveDate.ToString(SAHL.Common.Constants.DateFormat);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CapSalesConfigGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OnGridSelectedIndexChanged != null)
            {
                OnGridSelectedIndexChanged(sender, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            if (OnCancelButtonClicked != null)
                OnCancelButtonClicked(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            if (OnSubmitButtonClicked != null)
            {
                OnSubmitButtonClicked(sender, e);
            }
        }

        #endregion

        #region ICapSalesConfiguration Members

        /// <summary>
        /// 
        /// </summary>
        public void ShowDisplayControls()
        {
            StatusRow.Visible = true;
            Status.Visible = true;
            StatusUpdate.Visible = false;

            CapTypeValue.Visible = true;
            OfferStartDate.Visible = true;
            OfferEndDate.Visible = true;
            PremiumFee.Visible = true;
            AdminFee.Visible = true;
            Term.Visible = true;
            FinanceRate.Visible = true;
            Premium.Visible = true;

            CapTypeValueUpdate.Visible = false;
            OfferStartDateUpdate.Visible = false;
            OfferEndDateUpdate.Visible = false;
            PremiumFeeUpdate.Visible = false;
            AdminFeeUpdate.Visible = false;
            TermUpdate.Visible = false;
            FinanceRateUpdate.Visible = false;

            SubmitButton.Visible = false;
            CancelButton.Visible = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowUpdateControls()
        {
            StatusRow.Visible = true;
            Status.Visible = false;
            StatusUpdate.Visible = true;
            CapTypeValue.Visible = true;
            CapTypeValueUpdate.Visible = false;

            OfferStartDate.Visible = false;
            OfferEndDate.Visible = false;
            PremiumFee.Visible = false;
            AdminFee.Visible = false;
            Term.Visible = false;
            FinanceRate.Visible = false;
            Premium.Visible = true;

            OfferStartDateUpdate.Visible = true;
            OfferEndDateUpdate.Visible = true;
            PremiumFeeUpdate.Visible = true;
            AdminFeeUpdate.Visible = true;
            TermUpdate.Visible = true;
            FinanceRateUpdate.Visible = true;

            SubmitButton.Text = "Update";
            SubmitButton.AccessKey = "U";
            SubmitButton.Visible = true;
            CancelButton.Visible = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowAddControls()
        {
            StatusRow.Visible = false;
            CapTypeValue.Visible = false;
            CapTypeValueUpdate.Visible = true;
           
            OfferStartDate.Visible = false;
            OfferEndDate.Visible = false;
            PremiumFee.Visible = false;
            AdminFee.Visible = false;
            Term.Visible = false;
            FinanceRate.Visible = false;
            Premium.Visible = true;

            OfferStartDateUpdate.Visible = true;
            OfferEndDateUpdate.Visible = true;
            PremiumFeeUpdate.Visible = true;
            AdminFeeUpdate.Visible = true;
            TermUpdate.Visible = true;
            FinanceRateUpdate.Visible = true;

            SubmitButton.Text = "Add";
            SubmitButton.AccessKey = "A";
            SubmitButton.Visible = true;
            CancelButton.Visible = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resetDates"></param>
        public void BindResetDateDropDown(DataTable resetDates)
        {
            CapResetConfigDate.Items.Clear();
            for (int i = 0; i < resetDates.Rows.Count; i++)
            {
                CapResetConfigDate.Items.Add(new ListItem(
                    Convert.ToDateTime( resetDates.Rows[i]["ResetDate"] ).ToString(SAHL.Common.Constants.DateFormat),
                    Convert.ToDateTime( resetDates.Rows[i]["ResetDate"] ).ToString(SAHL.Common.Constants.DateFormat)
                    ));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="statusList"></param>
        public void BindStatusDropDown(IList<IGeneralStatus> statusList)
        {
            StatusUpdate.DataSource = statusList;
            StatusUpdate.DataTextField = "Description";
            StatusUpdate.DataValueField = "Key";
            StatusUpdate.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capTypeList"></param>
        public void BindCapTypeDropDown(IList<ICapType> capTypeList)
        {
            CapTypeValueUpdate.DataSource = capTypeList;
            CapTypeValueUpdate.DataTextField = "Description";
            CapTypeValueUpdate.DataValueField = "Key";
            CapTypeValueUpdate.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capConfigList"></param>
        public void BindCapSalesGrid(IList<ICapTypeConfigurationDetail> capConfigList)
        {
            CapSalesConfigGrid.Columns.Clear();
            CapSalesConfigGrid.AddGridBoundColumn("Key", "Key", Unit.Percentage(10), HorizontalAlign.Left, false);
            CapSalesConfigGrid.AddGridBoundColumn("", "Cap Type", Unit.Percentage(10), HorizontalAlign.Left, true);  //changed thes on row date bound
            CapSalesConfigGrid.AddGridBoundColumn("Rate", "Cap Base Rate", Unit.Percentage(10), HorizontalAlign.Left, true);

            CapSalesConfigGrid.AddGridBoundColumn("", "Term", Unit.Percentage(10), HorizontalAlign.Left, true);
            CapSalesConfigGrid.AddGridBoundColumn("", "Offer Start Date", Unit.Percentage(10), HorizontalAlign.Left, true);
            CapSalesConfigGrid.AddGridBoundColumn("", "Offer End Date", Unit.Percentage(10), HorizontalAlign.Left, true);
            CapSalesConfigGrid.AddGridBoundColumn("", "Cap Effective Date", Unit.Percentage(10), HorizontalAlign.Left, true);
            CapSalesConfigGrid.AddGridBoundColumn("", "Status", Unit.Percentage(10), HorizontalAlign.Left, true);

            CapSalesConfigGrid.DataSource = capConfigList;
            CapSalesConfigGrid.DataBind();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configDetail"></param>
        public void BindLabels(ICapTypeConfigurationDetail configDetail)
        {
            if (configDetail != null)
            {
                CapTypeValue.Text = configDetail.CapType.Description;
                CapBaseRate.Text = configDetail.Rate.ToString(SAHL.Common.Constants.RateFormat);
                OfferStartDate.Text = configDetail.CapTypeConfiguration.ApplicationStartDate.ToString(SAHL.Common.Constants.DateFormat);
                OfferEndDate.Text = configDetail.CapTypeConfiguration.ApplicationEndDate.ToString(SAHL.Common.Constants.DateFormat);
                PremiumFee.Text = configDetail.FeePremium.ToString(NumberFormat5Decimal);
                AdminFee.Text = configDetail.FeeAdmin.ToString(NumberFormat5Decimal);
                Status.Text = configDetail.GeneralStatus.Description;
                CapEffectiveDate.Text = configDetail.CapTypeConfiguration.CapEffectiveDate.ToString(SAHL.Common.Constants.DateFormat);
                Term.Text = configDetail.CapTypeConfiguration.Term.ToString() + " months";
                CapClosureDate.Text = configDetail.CapTypeConfiguration.CapClosureDate.ToString(SAHL.Common.Constants.DateFormat);
                FinanceRate.Text = configDetail.RateFinance.ToString(NumberFormat5Decimal);
                Premium.Text = configDetail.Premium.ToString(NumberFormat5Decimal);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configDetail"></param>
        /// <param name="refreshScreen"></param>
        public void BindUpdateControls(ICapTypeConfigurationDetail configDetail, bool refreshScreen)
        {
            if (configDetail != null && refreshScreen)
            {
                CapTypeValue.Text = configDetail.CapType.Description;
                CapBaseRate.Text = configDetail.Rate.ToString(SAHL.Common.Constants.RateFormat);
                CapEffectiveDate.Text = configDetail.CapTypeConfiguration.CapEffectiveDate.ToString(SAHL.Common.Constants.DateFormat);
                CapClosureDate.Text = configDetail.CapTypeConfiguration.CapClosureDate.ToString(SAHL.Common.Constants.DateFormat);

                OfferStartDateUpdate.Date = configDetail.CapTypeConfiguration.ApplicationStartDate;
                OfferEndDateUpdate.Date = configDetail.CapTypeConfiguration.ApplicationEndDate;
                PremiumFeeUpdate.Text = configDetail.FeePremium.ToString(NumberFormat5Decimal);
                AdminFeeUpdate.Text = configDetail.FeeAdmin.ToString(NumberFormat5Decimal);
                StatusUpdate.SelectedValue = configDetail.GeneralStatus.Key.ToString();
                TermUpdate.Text = configDetail.CapTypeConfiguration.Term.ToString();
                FinanceRateUpdate.Text = configDetail.RateFinance.ToString(NumberFormat5Decimal);
                Premium.Text = configDetail.Premium.ToString(NumberFormat5Decimal);

            }
            else
            {
                CapTypeValue.Text = configDetail.CapType.Description;
                CapBaseRate.Text = configDetail.Rate.ToString(SAHL.Common.Constants.RateFormat);
                CapEffectiveDate.Text = configDetail.CapTypeConfiguration.CapEffectiveDate.ToString(SAHL.Common.Constants.DateFormat);
                CapClosureDate.Text = configDetail.CapTypeConfiguration.CapClosureDate.ToString(SAHL.Common.Constants.DateFormat);

                double adminFee = 0;
                double premiumFee = 0;

                if (Request.Form[PremiumFeeUpdate.UniqueID] != null)
                {
                    PremiumFeeUpdate.Text = Request.Form[PremiumFeeUpdate.UniqueID];
                    if (!string.IsNullOrEmpty(Request.Form[PremiumFeeUpdate.UniqueID]))
                        premiumFee = double.Parse(PremiumFeeUpdate.Text);
                }

                if (Request.Form[AdminFeeUpdate.UniqueID] != null)
                {
                    AdminFeeUpdate.Text = Request.Form[AdminFeeUpdate.UniqueID];
                    if (!string.IsNullOrEmpty(Request.Form[AdminFeeUpdate.UniqueID]))
                        adminFee = double.Parse(AdminFeeUpdate.Text);
                }

                if (!string.IsNullOrEmpty(Request.Form[TermUpdate.UniqueID]))
                    TermUpdate.Text = Request.Form[TermUpdate.UniqueID];
                else
                    TermUpdate.Text = "0";

                if (Request.Form[FinanceRateUpdate.UniqueID] != null)
                    FinanceRateUpdate.Text = Request.Form[FinanceRateUpdate.UniqueID];

                Premium.Text = ((double)(premiumFee + adminFee)).ToString(NumberFormat5Decimal);

                if (Request.Form[StatusUpdate.UniqueID] != null)
                    StatusUpdate.SelectedValue = Request.Form[StatusUpdate.UniqueID];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearUpdateControls()
        {
            OfferStartDateUpdate.Enabled = false;
            PremiumFeeUpdate.Enabled = false;
            AdminFeeUpdate.Enabled = false;
            StatusUpdate.Enabled = false;
            TermUpdate.Enabled = false;
            FinanceRateUpdate.Enabled = false;
            OfferEndDateUpdate.Enabled = false;

            PremiumFeeUpdate.Text = "";
            AdminFeeUpdate.Text = "";
            StatusUpdate.Text = "";
            TermUpdate.Text = "0";
            FinanceRateUpdate.Text = "";
            Premium.Text = "0.00000";

            CapTypeValue.Text = "-";
            CapBaseRate.Text = "-";
            CapEffectiveDate.Text = "-";
            CapClosureDate.Text = "-";
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearAddControls()
        {
            TermUpdate.Text = "0";
            PremiumFeeUpdate.Text = "";
            AdminFeeUpdate.Text = "";
            FinanceRateUpdate.Text = "";
            Premium.Text = "0.00000";
            CapTypeValueUpdate.SelectedValue = "-select-";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capEffectiveDate"></param>
        /// <param name="capClosureDate"></param>
        /// <param name="capRate"></param>
        /// <param name="premium"></param>
        public void BindAddControls(DateTime capEffectiveDate, DateTime capClosureDate, double capRate, double premium)
        {
            CapEffectiveDate.Text = capEffectiveDate.ToString(SAHL.Common.Constants.DateFormat);
            CapClosureDate.Text = capClosureDate.ToString(SAHL.Common.Constants.DateFormat);
            CapBaseRate.Text = capRate.ToString(SAHL.Common.Constants.RateFormat);
            Premium.Text = premium.ToString(NumberFormat5Decimal);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capConfigDetail"></param>
        /// <param name="capConfig"></param>
        public void GetUpdateValuesFromScreen(ICapTypeConfigurationDetail capConfigDetail, ICapTypeConfiguration capConfig)
        {
            ILookupRepository _lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
            double adminFee = 0;
            double premiumFee = 0;

            if (Request.Form[PremiumFeeUpdate.UniqueID] != null)
            {
                if (!string.IsNullOrEmpty(Request.Form[PremiumFeeUpdate.UniqueID]))
                {
                    premiumFee = double.Parse(Request.Form[PremiumFeeUpdate.UniqueID]);
                    capConfigDetail.FeePremium = premiumFee;
                }
            }

            if (Request.Form[AdminFeeUpdate.UniqueID] != null)
            {
                if (!string.IsNullOrEmpty(Request.Form[AdminFeeUpdate.UniqueID]))
                {
                    adminFee = double.Parse(Request.Form[AdminFeeUpdate.UniqueID]);
                    capConfigDetail.FeeAdmin = adminFee;
                }
            }

            if (!string.IsNullOrEmpty(Request.Form[TermUpdate.UniqueID]))
            {
                capConfig.Term = int.Parse(Request.Form[TermUpdate.UniqueID]);
            }

            if (!string.IsNullOrEmpty(Request.Form[FinanceRateUpdate.UniqueID]))
            {
                capConfigDetail.RateFinance = double.Parse(Request.Form[FinanceRateUpdate.UniqueID]);
            }

            capConfigDetail.Premium = premiumFee + adminFee;

            if (OfferStartDateUpdate.Date.HasValue)
                capConfig.ApplicationStartDate = OfferStartDateUpdate.Date.Value ;
            //
            if (OfferEndDateUpdate.Date.HasValue)
            {
                string _appEndDateStr = OfferEndDateUpdate.Date.Value.ToString(SAHL.Common.Constants.DateFormat) + " 23:59:59";
                DateTime _appEndDate = DateTime.ParseExact(_appEndDateStr, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                capConfig.ApplicationEndDate = _appEndDate;
            }

            if (Request.Form[StatusUpdate.UniqueID] != null && Request.Form[StatusUpdate.UniqueID] != "-select-")
                capConfigDetail.GeneralStatus = _lookupRepo.GeneralStatuses[(GeneralStatuses)Convert.ToInt32(Request.Form[StatusUpdate.UniqueID])];

        }

        #endregion
    }
}
