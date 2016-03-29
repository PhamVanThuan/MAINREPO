using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAHL.Common.Web.UI;

namespace SAHL.Web.Views.Common.Interfaces
{
    public interface IValuationSummary : IViewBase
    {
        event EventHandler btnCancelClicked;

        /// <summary>
        /// Render XML
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="styleSheet"></param>
        void RenderXml(string xml, string styleSheet);
    }
}