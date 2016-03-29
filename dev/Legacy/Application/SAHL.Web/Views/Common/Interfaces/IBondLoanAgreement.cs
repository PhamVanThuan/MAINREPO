using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using System.Collections.Generic;


namespace SAHL.Web.Views.Common.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBondLoanAgreement : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        void BindBonds();

        /// <summary>
        /// 
        /// </summary>
        void BindLoanAgreement();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DeedsOffice"></param>
        void BindDeedsOffice(IEventList<IDeedsOffice> DeedsOffice);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Attorney"></param>
        void BindAttorney(Dictionary<int, string> Attorney);

        /// <summary>
        /// Populate the controls used to update the Bond record
        /// </summary>
        void PopulateUpdateBond(int BondIndex);

        /// <summary>
        /// Raised when the submit button is clicked.
        /// </summary>
        event EventHandler OnSubmitButtonClicked;

        /// <summary>
        /// Raised when the cancel button is clicked.
        /// </summary>
        event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnDeedsOfficeUpdate_SelectedIndexChanged;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnBondGrid_SelectedIndexChanged;

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        IEventList<IBond> Bonds
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        IEventList<ILoanAgreement> LoanAgreements
        {
            //get;
            set;
        }

        /// <summary>
        /// Get or set the Bond Grid Index
        /// </summary>
        int BondGridIndex { get; set;}

        /// <summary>
        /// Set the Visible property on the Bond gridview
        /// </summary>
        bool ShowBondGrid
        {
            set;
        }

        /// <summary>
        /// Set the Visible property on the Loan Agreement gridview
        /// </summary>
        bool ShowLoanAgreeGrid
        {
            set; 
        }

        /// <summary>
        /// Set the Visible property on the Bond Detail Row to display controls to update a Bond record
        /// </summary>
        bool UpdateBond
        {
            set; 
        }

        /// <summary>
        /// Set the Visible property on the Add Loan Agreement Row to display controls to add a new Loan Agreement record
        /// </summary>
        bool AddLoanAgreement
        {
            set; 
        }

        /// <summary>
        /// Set the Visible property on the Cancel button
        /// </summary>
        bool ShowCancel
        {
            set; 
        }

        /// <summary>
        /// Set the Visible property on the Submit button
        /// </summary>
        bool ShowSubmit
        {
            set; 
        }

        /// <summary>
        /// Set the display text on the Submit button
        /// </summary>
        string SubmitButtonText
        {
            set ; 
        }

        /// <summary>
        /// Get the value from the Date control
        /// </summary>
        DateTime? LoanAgreementDate
        {
            get;
            set;
        }

        /// <summary>
        /// Get the text value from the Amount input
        /// </summary>
        string LoanAgreementAmount
        {
            get;
        }

        /// <summary>
        /// Get the selected index from the DeedsOffice dropdown
        /// </summary>
        int DeedsOfficeSelectedValue
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        int AttorneySelectedValue
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        string BondRegNumber
        {
            get ; 
        }

        /// <summary>
        /// 
        /// </summary>
        double BondRegAmount
        {
            get ; 
        }

        /// <summary>
        /// 
        /// </summary>
        bool BondGridPostBack
        {
            set; 
        }

        /// <summary>
        /// 
        /// </summary>
        int BondGridSelectedKey
        {
            get;
        }

        #endregion

    }
}
