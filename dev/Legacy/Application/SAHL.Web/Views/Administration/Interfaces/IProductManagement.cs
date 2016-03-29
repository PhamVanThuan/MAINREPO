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

namespace SAHL.Web.Views.Administration.Interfaces
{
    public interface IProductManagement : IViewBase
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler onLoadButtonClicked;

        event EventHandler onSubmitButtonClicked;

        event EventHandler onOriginationSource_SelectedIndexChanged;

        event EventHandler onProduct_SelectedIndexChanged;

        void BindOriginationSources(IDictionary<string, string> originationSource, string defaultValue, bool pleaseSelect);

        void BindProducts(IDictionary<string, string> product, string defaultValue, bool pleaseSelect);

        int SelectedOriginationSourceKey { get; set; }

        int SelectedProductKey { get; set; }

        bool UpdateMode { get; set;}

        void BindCreditCriteria(DataSet dsCreditCriteria);

    }
}
