using SAHL.Common.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAHL.Web.Views.Common.Interfaces
{
    public interface IWorkflowEmploymentTypeConfirmation : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnConfirmButtonClicked;

        /// <summary>
        /// 
        /// </summary>
        event EventHandler OnCancelButtonClicked;

        /// <summary>
        /// Get the key of the Employment Type selected in the drop down list
        /// </summary>
        int SelectedEmploymentTypeKey
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employmentTypes"></param>
        void BindEmploymentTypes(IDictionary<int,string> employmentTypes);
    }
}