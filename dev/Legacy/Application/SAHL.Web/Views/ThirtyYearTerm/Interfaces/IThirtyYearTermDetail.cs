using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Web.UI;
using SAHL.Common.Web.UI.Events;
using SAHL.V3.Framework.Model;

namespace SAHL.Web.Views.ThirtyYearTerm.Interfaces
{
    /// <summary>
    ///
    /// </summary>
    public interface IThirtyYearTermDetail : IViewBase
    {
        /// <summary>
        ///
        /// </summary>
        event EventHandler OnCancelButtonClicked;

        /// <summary>
        ///
        /// </summary>
        event EventHandler OnSubmitButtonClicked;

        /// <summary>
        ///
        /// </summary>
        bool ShowCancelButton { set; }

        /// <summary>
        ///
        /// </summary>
        bool ShowSubmitButton { set; }

        /// <summary>
        /// 
        /// </summary>
        string SubmitButtonText { set; }

        bool ApplicationQualifiesFor30Year { get;  set; }

        IList<string> DecisionTreeMessages { set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="termDetail"></param>
        void DisplayCurrentTermDetails(ApplicationLoanDetail termDetail);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="termDetail"></param>
        void Display30YearTermDetails(ApplicationLoanDetail termDetail);
    }

}