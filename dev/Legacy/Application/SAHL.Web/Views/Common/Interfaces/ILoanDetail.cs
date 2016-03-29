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
using SAHL.Common.Web.UI.Events;
using System.Collections.Generic;
using SAHL.Common.Web.UI.Controls;

namespace SAHL.Web.Views.Common.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILoanDetail : IViewBase
    {
        #region Properties
        /// <summary>
        /// 
        /// </summary>
        bool ShowLabels { set;}

        /// <summary>
        /// 
        /// </summary>
        bool ShowButtons { set;}

        /// <summary>
        /// 
        /// </summary>
        bool CancellationTypeEnabled { set;}

        /// <summary>
        /// 
        /// </summary>
        string SubmitButtonText { set;}

        /// <summary>
        /// 
        /// </summary>
        bool SubmitButtonEnabled { set;}

        /// <summary>
        /// 
        /// </summary>
        GridPostBackType DetailGridPostBackType {set;}


        /// <summary>
        /// 
        /// </summary>
        int UpdatedDetailClass {get;}
        /// <summary>
        /// 
        /// </summary>
        int UpdatedDetailType { get; }
        /// <summary>
        /// 
        /// </summary>
        DateTime? UpdatedDetailDate { get;}
        /// <summary>
        /// 
        /// </summary>
        double UpdatedDetailAmount { get;}
        /// <summary>
        /// 
        /// </summary>
        string UpdatedDetailDescription { get;}
        /// <summary>
        /// 
        /// </summary>
        int UpdatedCancellationType { get;}
        /// <summary>
        /// 
        /// </summary>
        bool UpdateMode { set;}

        bool DeleteMode { set;}

        #endregion

        #region Event Handlers
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnSubmitButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        event KeyChangedEventHandler OnGridSelectedIndexChanged;


        #endregion


        #region Methods
        /// <summary>
        /// Binds the list of detail types to the grid
        /// </summary>
        /// <param name="loanDetails"></param>
        void BindDetailGrid(IReadOnlyEventList<IDetail> loanDetails);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="detailRow"></param>
        void BindData(IDetail detailRow);

        /// <summary>
        /// Binds the detail type dropdown list
        /// </summary>
        void BindDetailClassDropDown(IEventList<IDetailClass> detailClasses);

        /// <summary>
        /// binds the cancellation type dropdown list
        /// </summary>
        void BindDetailCancellationTypeDropDown(IEventList<ICancellationType> cancellationTypes);

        /// <summary>
        /// binds the detail type drop down list
        /// </summary>
        /// <param name="detailTypeList"></param>
        void BindDetailTypeDropDown(List<IDetailType> detailTypeList);

        #endregion

    }
}
