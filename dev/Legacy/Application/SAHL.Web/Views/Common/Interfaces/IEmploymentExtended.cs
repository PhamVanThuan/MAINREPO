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
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Web.Controls;
using SAHL.Common.Web.UI.Events;
using System.Collections.Generic;

namespace SAHL.Web.Views.Common.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEmploymentExtended : IViewBase
    {

        #region Events

        /// <summary>
        /// Raised when the back button is clicked.
        /// </summary>
        event EventHandler BackButtonClicked;

        /// <summary>
        /// Raised when the cancel button is clicked.
        /// </summary>
        event EventHandler CancelButtonClicked;

        /// <summary>
        /// Raised when the save button is clicked.
        /// </summary>
        event EventHandler SaveButtonClicked;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        bool PanelConfirmedEnabled { get;}

        /// <summary>
        /// 
        /// </summary>
        bool VerificationProcessPanelEnabled { get;set;}

        /// <summary>
        /// 
        /// </summary>
        bool ConfirmedDetailsEnabled { get;set;}

        /// <summary>
        /// Gets/sets whether the confirmed income can be edited or not.
        /// </summary>
        bool ConfirmedIncomeReadOnly { get; set; }

        /// <summary>
        /// Gets/sets whether the confirmed income is enabled or not.
        /// </summary>
        bool ConfirmedIncomeEnabled { get; set; }

        /// <summary>
        /// Gets/sets whether the confirmed income can be seen.
        /// </summary>
        bool ConfirmedIncomeVisible { get; set; }

        /// <summary>
        /// Gets/sets whether the monthly income can be edited or not.
        /// </summary>
        bool MonthlyIncomeReadOnly { get; set; }

        /// <summary>
        /// Gets/sets whether the monthly income is enabled or not.
        /// </summary>
        bool MonthlyIncomeEnabled { get; set; }

        /// <summary>
        /// Gets/sets the text on the Save button.
        /// </summary>
        string SaveButtonText { get; set; }

        /// <summary>
        /// Gets/sets the visibility of the save button.
        /// </summary>
        bool SaveButtonVisible { get; set; }

        /// <summary>
        /// Gets/sets the visibility of the back button.
        /// </summary>
        bool BackButtonVisible { get; set; }

        bool ContactPersonReadOnly { set;}

        bool PhoneNumberReadOnly { set;}

        bool DepartmentReadOnly { set;}

        bool SalaryPayDayReadOnly { set; }

        bool ConfirmationSourceReadOnly { set;}

        bool VerificationProcessReadOnly { set;}

        bool UnionMemberReadOnly { set; }

        #endregion

        #region Methods

        /// <summary>
        /// Takes the supplied <c>employment</c> record and populates it with values entered in the form.
        /// </summary>
        /// <param name="employment"></param>
        void GetExtendedDetails(IEmployment employment);

        /// <summary>
        /// Sets the employment entity we are currently working with, and populates the form with details 
        /// from the employment entity.
        /// </summary>
        void SetEmployment(IEmployment employment);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtVerificationProcess"></param>
        void BindVerificationProcessList(DataTable dtVerificationProcess);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="confirmLst"></param>
        void BindConfirmationSourceList(IEventList<IEmploymentConfirmationSource> confirmLst);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employment"></param>
        void SetConfirmationEdit(IEmployment employment);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employment"></param>
        void SetConfirmationDisplay(IEmployment employment);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employment"></param>
        void SetExtendedEmployment(IEmployment employment);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        List<int> GetVerificationProcessList { get;}

        void BindUniomMemberShipList();

        #endregion

    }
}
