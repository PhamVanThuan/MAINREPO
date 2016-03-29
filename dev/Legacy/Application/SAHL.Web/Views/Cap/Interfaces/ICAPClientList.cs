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
using System.Collections.Generic;
using SAHL.Common.Web.UI.Controls;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI.Events;

namespace SAHL.Web.Views.Cap.Interfaces
{
    public interface ICAPClientList : IViewBase
    {
        #region Event Handlers

        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnExtractButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnImportButtonClicked;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        bool FileNameRowVisible {set;}

        /// <summary>
        /// 
        /// </summary>
        bool SPVRowVisible {set;}

        /// <summary>
        /// 
        /// </summary>
        bool DateExcludeRowVisible {set;}

        /// <summary>
        /// 
        /// </summary>
        bool ArrearRowVisible { set;}

        /// <summary>
        /// 
        /// </summary>
        bool ExtractButtonVisible { set;}

        /// <summary>
        /// 
        /// </summary>
        bool ImportButtonVisible { set;}

        /// <summary>
        /// 
        /// </summary>
        string SelectedCapResetConfigDate { get;}

        /// <summary>
        /// 
        /// </summary>
        string SelectedOperatorValue { get;}

        /// <summary>
        /// 
        /// </summary>
        int SelectedCapType { get;}

        /// <summary>
        /// Gets the captured loan arrear balance.
        /// </summary>
        double? LoanArrearBalanceValue { get;}

        /// <summary>
        /// 
        /// </summary>
        DateTime? DateExcludeValue { get; set; }

        /// <summary>
        /// Gets a list of selected SPV keys.
        /// </summary>
        List<string> SPVList { get; }

        /// <summary>
        /// 
        /// </summary>
        string FileNameValue { get;}

        /// <summary>
        /// Gets the file posted.
        /// </summary>
        HttpPostedFile File { get;}
        
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resetDates"></param>
        void BindResetDateDropDown(DataTable resetDates);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="offerDateList"></param>
        void BindOfferDateDropDown(Dictionary<int, string> offerDateList);
        /// <summary>
        /// 
        /// </summary>
        void BindOperatorDropDown();
        /// <summary>
        /// 
        /// </summary>
        void BindGrid(IList<ICapTypeConfigurationDetail> detailList);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="spvList"></param>
        void BindSPVList(DataTable spvList);

    }
}
