using System;
using System.Collections.Generic;
using System.Text;
using SAHL.Common.Web.UI;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.BusinessModel.Interfaces;
using System.Data;

namespace SAHL.Web.Views.Common.Interfaces
{
    public interface ISantamPolicy : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        void BindPolicyDetails(ISANTAMPolicyTracking PolicyDetails);

        void DisplayNoPolicy();
    }
}
