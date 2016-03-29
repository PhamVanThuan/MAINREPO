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
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Globals;
using SAHL.Common.Web.UI.Controls;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces.Repositories;
using SAHL.Common.Factories;
using SAHL.Common.Web.UI.Events;


namespace SAHL.Web.Views.Common
{
    public partial class LoanDetail : SAHLCommonBaseView, ILoanDetail
    {
        #region Private Variables
 
        bool _showLabels;
        bool _showButtons;
        bool _cancellationTypeEnabled;
        bool _submitButtonEnabled;
        bool _deleteMode;

        string _submitButtonText;
        bool _updateMode;

        GridPostBackType _gridPostBackType;

        #endregion


        #region Event Handlers

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public event KeyChangedEventHandler OnSubmitButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        public event KeyChangedEventHandler OnGridSelectedIndexChanged;


        #endregion


        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public bool ShowLabels
        {
            set { _showLabels = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ShowButtons
        {
            set { _showButtons = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool CancellationTypeEnabled
        {
            set { _cancellationTypeEnabled = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SubmitButtonText
        {
            set { _submitButtonText = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool SubmitButtonEnabled
        {
            set { _submitButtonEnabled = value; }
        }
        

        public bool DeleteMode
        {
            set { _deleteMode = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public GridPostBackType DetailGridPostBackType
        {
            set { _gridPostBackType = value; }
        }


        /// <summary>
        /// 
        /// </summary>
	    public int UpdatedDetailClass
	    {
		    get {
                if ( !String.IsNullOrEmpty(Page.Request.Params.Get(ddlDetailClass.UniqueID)) && Page.Request.Params.Get(ddlDetailClass.UniqueID) != "-select-")
                    return Convert.ToInt32(Page.Request.Params.Get(ddlDetailClass.UniqueID));
                    else
                        return -1;
            }
	    }

        /// <summary>
        /// 
        /// </summary>
        public int UpdatedDetailType
	    {
		    get {
                if (!String.IsNullOrEmpty(Page.Request.Params.Get(ddlDetailType.UniqueID)) && Page.Request.Params.Get(ddlDetailType.UniqueID) != "-select-")
                    return Convert.ToInt32(Page.Request.Params.Get(ddlDetailType.UniqueID));
                    else
                        return -1;
            }
	    }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? UpdatedDetailDate
	    {
		    get {
                return dateLoanDetailDate.Date;
            }
	    }

        /// <summary>
        /// 
        /// </summary>
        public double UpdatedDetailAmount
	    {
		    get { 
                    if (!String.IsNullOrEmpty(txtDetailAmount.Text))
                        return Convert.ToDouble(txtDetailAmount.Text);
                    else
                        return 0;
            }
	    }

        /// <summary>
        /// 
        /// </summary>
        public string UpdatedDetailDescription
        {
            get
            {
                return (txtDescription.Text);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int UpdatedCancellationType
	    {
		    get { 
                    if (!String.IsNullOrEmpty(ddlCancellationType.SelectedValue) && ddlCancellationType.SelectedValue != "-select-")
                        return Convert.ToInt32( ddlCancellationType.SelectedValue  );
                    else
                        return -1;
            }
	    }

        /// <summary>
        /// 
        /// </summary>
        public bool UpdateMode
        {
            set { _updateMode = value; }
        }

       


	    #endregion

        #region Protected Functions Section

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!ShouldRunPage) return;

            lblViewDetailClass.Visible  = _showLabels;
            ddlDetailClass.Visible = !_showLabels;

            lblViewDetailType.Visible = _showLabels;
            ddlDetailType.Visible = !_showLabels;

            lblViewDetailDate.Visible = _showLabels;
            dateLoanDetailDate.Visible = !_showLabels;

            lblViewAmount.Visible = _showLabels;
            txtDetailAmount.Visible = !_showLabels;

            lblDescription.Visible = _showLabels;
            txtDescription.Visible = !_showLabels;

            lblCancellationType.Visible = _showLabels;
            ddlCancellationType.Visible = !_showLabels;
            ddlCancellationType.Enabled = _cancellationTypeEnabled;

            CancelButton.Visible = _showButtons;
            SubmitButton.Visible = _showButtons;
            SubmitButton.Text = _submitButtonText;
            SubmitButton.Enabled = _submitButtonEnabled;

            if (_updateMode)
            {
                ddlDetailClass.Visible = false;
                ddlDetailType.Visible = false;

                lblViewDetailClass.Visible = true;
                lblViewDetailType.Visible = true;
            }

            if (_deleteMode)
                SubmitButton.Attributes["onclick"] = "if(!confirm('Are you sure you want to delete this item?')) return false";
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdLoanDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ILookupRepository _lookupRepository = RepositoryFactory.GetRepository<ILookupRepository>();

            if (e.Row.DataItem != null)
            {
                IDetail ro = e.Row.DataItem as IDetail;
                e.Row.Cells[2].Text = ro.DetailType.DetailClass.Description;
                e.Row.Cells[3].Text = ro.DetailType.Description;
                string cancellationType = "";
                if (ro.LinkID.HasValue)
                {
                    if (_lookupRepository.CancellationTypes.ObjectDictionary.ContainsKey(ro.LinkID.ToString()))
                        cancellationType = _lookupRepository.CancellationTypes.ObjectDictionary[ro.LinkID.Value.ToString()].Description;
                }
                e.Row.Cells[4].Text = cancellationType;
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
            {
                OnCancelButtonClicked(sender, e);
            }
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
                KeyChangedEventArgs args = new KeyChangedEventArgs(grdLoanDetail.SelectedIndex);
                OnSubmitButtonClicked(sender, args);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdLoanDetail_SelectedIndexChanged(object sender, EventArgs e)
        {
            KeyChangedEventArgs args = new KeyChangedEventArgs(grdLoanDetail.SelectedIndex);
            if (OnGridSelectedIndexChanged != null)
            {
                OnGridSelectedIndexChanged(sender, args);
            }
        }

        //protected void ddlDetailClass_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (Request.Form[ddlDetailClass.UniqueID] != null && Request.Form[ddlDetailClass.UniqueID] != "-select-")
        //    {
        //        KeyChangedEventArgs args = new KeyChangedEventArgs(Convert.ToInt32( Request.Form[ddlDetailClass.UniqueID] ));
        //        if (OnDetailClassChanged != null)
        //        {
        //            OnDetailClassChanged(sender, args);
        //        }
        //    }
        //}

        //protected void ddlDetailType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (Request.Form[ddlDetailType.UniqueID] != null && Request.Form[ddlDetailType.UniqueID] != "-select-")
        //    {
        //        KeyChangedEventArgs args = new KeyChangedEventArgs(Convert.ToInt32(Request.Form[ddlDetailType.UniqueID]));
        //        if (OnDetailTypeChanged != null)
        //        {
        //            OnDetailTypeChanged(sender, args);
        //        }
        //    }
        //}
        


        #endregion

        #region ILoanDetail Members


        /// <summary>
        /// 
        /// </summary>
        /// <param name="loanDetails"></param>
        public void BindDetailGrid(IReadOnlyEventList<IDetail> loanDetails)
        {
            grdLoanDetail.PostBackType = _gridPostBackType;
            grdLoanDetail.Columns.Clear();
            grdLoanDetail.AddGridBoundColumn("DetailDate",SAHL.Common.Constants.DateFormat,GridFormatType.GridDate, "Detail Date", false,Unit.Percentage(15),HorizontalAlign.Center, true);
            grdLoanDetail.AddGridBoundColumn("Amount", SAHL.Common.Constants.CurrencyFormat, GridFormatType.GridNumber, "Amount",false, Unit.Percentage(15), HorizontalAlign.Left, true);
            grdLoanDetail.AddGridBoundColumn("", "Detail Class", Unit.Percentage(20), HorizontalAlign.Left, true);
            grdLoanDetail.AddGridBoundColumn("", "Detail Type", Unit.Percentage(28), HorizontalAlign.Left, true);
            grdLoanDetail.AddGridBoundColumn("", "Cancellation Type", Unit.Percentage(24), HorizontalAlign.Left, true);
            grdLoanDetail.DataSource = loanDetails;
            grdLoanDetail.DataBind();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="detailRow"></param>
        public void BindData(IDetail detailRow)
        {
            if (detailRow != null)
            {
                ddlDetailClass.SelectedValue = detailRow.DetailType.DetailClass.Key.ToString();
                lblViewDetailClass.Text = detailRow.DetailType.DetailClass.Description;

                ddlDetailType.SelectedValue = detailRow.DetailType.Key.ToString();
                lblViewDetailType.Text = detailRow.DetailType.Description;

                dateLoanDetailDate.Date = detailRow.DetailDate;
                lblViewDetailDate.Text = detailRow.DetailDate.ToString(SAHL.Common.Constants.DateFormat);

                if (detailRow.Amount.HasValue)
                {
                    txtDetailAmount.Text = detailRow.Amount.Value.ToString(SAHL.Common.Constants.NumberFormat);
                    lblViewAmount.Text = detailRow.Amount.Value.ToString(SAHL.Common.Constants.CurrencyFormat);
                }
                txtDescription.Text  = detailRow.Description;
                lblDescription.Text = detailRow.Description;

                if (detailRow.LinkID.HasValue)
                {
                    lblCancellationType.Text = "";
                    ILookupRepository lookupRepo = RepositoryFactory.GetRepository<ILookupRepository>();
                    if (lookupRepo.CancellationTypes.ObjectDictionary.ContainsKey(detailRow.LinkID.Value.ToString()))
                    {
                        ddlCancellationType.SelectedValue = detailRow.LinkID.Value.ToString();
                        lblCancellationType.Text = lookupRepo.CancellationTypes.ObjectDictionary[detailRow.LinkID.Value.ToString()].Description;
                    }
                }                
            }
        }

        /// <summary>
        /// Binds the detail type dropdown list
        /// </summary>
        public void BindDetailClassDropDown(IEventList<IDetailClass> detailClasses)
        {
            ddlDetailClass.DataSource = detailClasses;
            ddlDetailClass.DataValueField = "Key";
            ddlDetailClass.DataTextField = "Description";
            ddlDetailClass.DataBind();
        }

        /// <summary>
        /// binds the cancellation type dropdown list
        /// </summary>
        public void BindDetailCancellationTypeDropDown(IEventList<ICancellationType> cancellationTypes)
        {
            ddlCancellationType.DataSource = cancellationTypes;
            ddlCancellationType.DataValueField = "Key";
            ddlCancellationType.DataTextField = "Description";
            ddlCancellationType.DataBind();
        }

        /// <summary>
        /// binds the detail type drop down list
        /// </summary>
        /// <param name="detailTypeList"></param>
        public void BindDetailTypeDropDown( List<IDetailType> detailTypeList)
        {
            ddlDetailType.DataSource = detailTypeList;
            ddlDetailType.DataValueField = "Key";
            ddlDetailType.DataTextField = "Description";
            ddlDetailType.DataBind();

            
        }

        #endregion

    }
}
