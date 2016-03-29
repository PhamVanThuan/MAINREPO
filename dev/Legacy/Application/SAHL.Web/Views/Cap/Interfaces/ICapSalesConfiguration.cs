using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.Web.UI;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Web.Views.Cap.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICapSalesConfiguration : IViewBase
    {
        #region Properties
        /// <summary>
        /// 
        /// </summary>
        GridPostBackType CapSalesConfigGridPostBackType { set; }
        /// <summary>
        /// 
        /// </summary>
        bool SubmitButtonEnabled { set; }
        /// <summary>
        /// 
        /// </summary>
        string SelectedCapResetConfigDate { get; }
        /// <summary>
        /// 
        /// </summary>
        int CapSalesConfigGridSelectedIndex { get; }
        /// <summary>
        /// 
        /// </summary>
        int TermValue { get; }
        /// <summary>
        /// 
        /// </summary>
        int SelectedCapType { get; }
        /// <summary>
        /// 
        /// </summary>
        double AdminFeeValue { get; }
        /// <summary>
        /// 
        /// </summary>
        double PremiumFeeValue { get; }
        /// <summary>
        /// 
        /// </summary>
        double RateFinanceValue { get; }
        /// <summary>
        /// 
        /// </summary>
        DateTime ApplicationStartDateValue { get; }
        /// <summary>
        /// 
        /// </summary>
        DateTime ApplicationEndDateValue { get; }
        #endregion

        #region Event Handlers
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnGridSelectedIndexChanged;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnSubmitButtonClicked;
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnCancelButtonClicked;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        void ShowDisplayControls();
        /// <summary>
        /// 
        /// </summary>
        void ShowUpdateControls();
        /// <summary>
        /// 
        /// </summary>
        void ShowAddControls();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resetDates"></param>
        void BindResetDateDropDown(DataTable resetDates);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="capConfigList"></param>
        void BindCapSalesGrid(IList<ICapTypeConfigurationDetail> capConfigList);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configDetail"></param>
        void BindLabels(ICapTypeConfigurationDetail configDetail);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configDetail"></param>
        /// <param name="refreshScreen"></param>
        void BindUpdateControls(ICapTypeConfigurationDetail configDetail, bool refreshScreen);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="statusList"></param>
        void BindStatusDropDown(IList<IGeneralStatus> statusList);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="capTypeList"></param>
        void BindCapTypeDropDown(IList<ICapType> capTypeList);
        /// <summary>
        /// 
        /// </summary>
        void ClearUpdateControls();
        /// <summary>
        /// 
        /// </summary>
        void ClearAddControls();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="capEffectiveDate"></param>
        /// <param name="capClosureDate"></param>
        /// <param name="capRate"></param>
        /// <param name="premium"></param>
        void BindAddControls(DateTime capEffectiveDate, DateTime capClosureDate, double capRate, double premium);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="capConfigDetail"></param>
        /// <param name="capConfig"></param>
        void GetUpdateValuesFromScreen(ICapTypeConfigurationDetail capConfigDetail, ICapTypeConfiguration capConfig);

    }
}
