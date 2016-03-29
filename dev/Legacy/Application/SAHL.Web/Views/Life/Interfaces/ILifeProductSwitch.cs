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
using SAHL.Common.Web.UI.Events;
using System.Collections.Generic;
using SAHL.Common.BusinessModel.Interfaces;
using SAHL.Common.Collections.Interfaces;

namespace SAHL.Web.Views.Life.Interfaces
{
    public interface ILifeProductSwitch : IViewBase
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
        event KeyChangedEventHandler PolicyTypeSelectedIndexChanged;

        /// <summary>
        /// 
        /// </summary>
        int PolicyTypeSelectedValue { get;set;}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lifePolicyTypes"></param>
        void BindLifePolicyTypes(IEventList<ILifePolicyType> lifePolicyTypes);

        /// <summary>
        /// 
        /// </summary>
        bool SubmitButtonVisible { set;}
    }
}
